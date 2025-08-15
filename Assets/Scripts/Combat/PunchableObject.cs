using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PunchableObject : MonoBehaviour, IPunchable
{
    [Header("Punch Reaction Settings")]
    [SerializeField] private float massMultiplier = 1f;
    [SerializeField] private float upwardForceMultiplier = 0.5f;
    [SerializeField] private bool useRandomTorque = true;
    [SerializeField] private float torqueMultiplier = 1f;

    [Header("Damage Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float damagePerPunch = 20f;
    [SerializeField] private bool destroyOnZeroHealth = true;

    [Header("Audio Settings")]
    [SerializeField] private AudioClip punchImpactSound;
    [SerializeField] private AudioClip breakingSound;
    [SerializeField] private float minPitchVariation = 0.9f;
    [SerializeField] private float maxPitchVariation = 1.1f;

    private float currentHealth;
    private Rigidbody rb;
    private AudioSource audioSource;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        // Add AudioSource component if not present
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.spatialBlend = 1f; // Make the sound 3D
            audioSource.maxDistance = 20f;
            audioSource.rolloffMode = AudioRolloffMode.Linear;
        }

        currentHealth = maxHealth;
    }

    public void OnPunch(Vector3 punchDirection, float force)
    {
        // Calculate final force considering mass
        float finalForce = force * massMultiplier;

        // Add upward component to make it more dramatic
        Vector3 finalDirection = punchDirection + Vector3.up * upwardForceMultiplier;
        finalDirection.Normalize();

        // Apply the force
        rb.AddForce(finalDirection * finalForce, ForceMode.Impulse);

        // Add random torque for more dynamic reaction
        if (useRandomTorque)
        {
            Vector3 randomTorque = new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            );
            rb.AddTorque(randomTorque * torqueMultiplier, ForceMode.Impulse);
        }

        // Play punch impact sound with random pitch
        if (punchImpactSound != null && audioSource != null)
        {
            audioSource.pitch = Random.Range(minPitchVariation, maxPitchVariation);
            audioSource.PlayOneShot(punchImpactSound);
        }

        // Handle damage
        TakeDamage(damagePerPunch);
    }

    private void TakeDamage(float damage)
    {
        currentHealth -= damage;

        // Optional: You can add visual feedback here like changing material color
        // For example, you could lerp the material color based on health percentage

        if (currentHealth <= 0 && destroyOnZeroHealth)
        {
            // Play breaking sound before destroying
            if (breakingSound != null && audioSource != null)
            {
                AudioSource.PlayClipAtPoint(breakingSound, transform.position);
            }

            // You could add particle effects here before destroying
            Destroy(gameObject);
        }
    }

    // Optional: Add a public method to get current health percentage
    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
}
