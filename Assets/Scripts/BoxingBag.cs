using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BoxingBag : MonoBehaviour, IPunchable
{
    [Header("Physics Settings")]
    [SerializeField] private float swingForce = 5f;
    [SerializeField] private float returnSpeed = 1f;
    [SerializeField] private float maxSwingAngle = 45f;
    [SerializeField] private float punchForceMultiplier = 1.5f;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip punchImpactSound;
    [SerializeField] private float minPitchVariation = 0.8f;
    [SerializeField] private float maxPitchVariation = 1.2f;

    [Header("Visual Feedback")]
    [SerializeField] private ParticleSystem hitEffect;
    [SerializeField] private float effectDuration = 1f;

    [Header("Training Mode Settings")]
    [SerializeField] private bool enableTrainingFeedback = true;
    [SerializeField] private float comboResetTime = 2f;

    private Rigidbody rb;
    private ConfigurableJoint joint;
    private AudioSource audioSource;
    private Vector3 initialRotation;
    private bool isSwinging = false;
    
    // Training feedback variables
    private int hitCount = 0;
    private float lastHitTime = 0f;

    private void Start()
    {
        // Get or add required components
        rb = GetComponent<Rigidbody>();
        SetupAudio();
        SetupJoint();
        initialRotation = transform.eulerAngles;
    }

    private void SetupAudio()
    {
        // Add AudioSource component if not present
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1f; // Make the sound 3D
            audioSource.maxDistance = 20f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
        }
    }

    private void SetupJoint()
    {
        // Add ConfigurableJoint for swinging behavior
        joint = gameObject.AddComponent<ConfigurableJoint>();
        
        // Lock all motion except rotation around Z axis
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;
        
        joint.angularXMotion = ConfigurableJointMotion.Limited;
        joint.angularYMotion = ConfigurableJointMotion.Limited;
        joint.angularZMotion = ConfigurableJointMotion.Limited;

        // Set up spring settings for natural swinging
        var spring = new SoftJointLimitSpring 
        { 
            spring = returnSpeed,
            damper = 0.5f
        };
        
        joint.angularXLimitSpring = spring;
        joint.angularYZLimitSpring = spring;

        // Set rotation limits
        var limit = new SoftJointLimit 
        { 
            limit = maxSwingAngle,
            bounciness = 0.5f
        };

        joint.highAngularXLimit = limit;
        joint.lowAngularXLimit = limit;
        joint.angularYLimit = limit;
        joint.angularZLimit = limit;
    }

    // IPunchable interface implementation
    public void OnPunch(Vector3 punchDirection, float force)
    {
        // Apply punch force with multiplier
        float finalForce = force * punchForceMultiplier;
        
        // Calculate torque for swinging motion
        Vector3 torque = Vector3.Cross(punchDirection, Vector3.up) * finalForce;
        
        // Apply the torque for swinging
        rb.AddTorque(torque, ForceMode.Impulse);
        
        // Also add some direct force for more dynamic reaction
        rb.AddForce(punchDirection * finalForce * 0.5f, ForceMode.Impulse);
        
        isSwinging = true;

        // Play punch impact sound with random pitch
        if (punchImpactSound != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(minPitchVariation, maxPitchVariation);
            audioSource.PlayOneShot(punchImpactSound);
        }

        // Spawn hit effect
        if (hitEffect != null)
        {
            var effect = Instantiate(hitEffect, transform.position + punchDirection * -0.5f, Quaternion.LookRotation(-punchDirection));
            Destroy(effect.gameObject, effectDuration);
        }

        // Training feedback
        if (enableTrainingFeedback)
        {
            HandleTrainingFeedback();
        }

        Debug.Log($"Boxing bag hit with force: {finalForce}");
    }

    private void HandleTrainingFeedback()
    {
        float currentTime = Time.time;
        
        // Reset combo if too much time has passed
        if (currentTime - lastHitTime > comboResetTime)
        {
            hitCount = 0;
        }
        
        hitCount++;
        lastHitTime = currentTime;
        
        // Provide feedback based on hit count
        if (hitCount % 10 == 0)
        {
            Debug.Log($"Great combo! {hitCount} hits!");
        }
        else if (hitCount % 5 == 0)
        {
            Debug.Log($"Nice combo! {hitCount} hits!");
        }
    }

    // Keep the collision detection as backup for non-punch interactions
    private void OnCollisionEnter(Collision collision)
    {
        // Only handle collisions that aren't from the punch system
        // (punch system will call OnPunch directly)
        if (collision.relativeVelocity.magnitude > 1f && !collision.gameObject.CompareTag("Player"))
        {
            ApplyCollisionForce(collision);
        }
    }

    private void ApplyCollisionForce(Collision collision)
    {
        // Calculate collision direction and force
        Vector3 collisionDirection = (transform.position - collision.contacts[0].point).normalized;
        Vector3 torque = Vector3.Cross(collisionDirection, Vector3.up) * swingForce;
        
        // Apply the force
        rb.AddTorque(torque, ForceMode.Impulse);
        isSwinging = true;
    }

    private void Update()
    {
        if (isSwinging)
        {
            // Check if the bag has mostly stopped swinging
            if (rb.angularVelocity.magnitude < 0.1f)
            {
                isSwinging = false;
                // Gradually return to initial position
                transform.rotation = Quaternion.Lerp(transform.rotation, 
                    Quaternion.Euler(initialRotation), 
                    Time.deltaTime * returnSpeed);
            }
        }
    }

    // Public method to get training stats
    public int GetHitCount()
    {
        return hitCount;
    }

    // Public method to reset training stats
    public void ResetTrainingStats()
    {
        hitCount = 0;
        lastHitTime = 0f;
        Debug.Log("Training stats reset!");
    }
}
