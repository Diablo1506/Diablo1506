using UnityEngine;

public class PunchSystemV2 : MonoBehaviour
{
    [Header("Punch Settings")]
    [SerializeField] private float punchForce = 10f;
    [SerializeField] private float punchRange = 2f;
    [SerializeField] private float punchRadius = 0.5f;
    [SerializeField] private LayerMask punchableLayer;

    [Header("Visual Feedback")]
    [SerializeField] private ParticleSystem punchVFX;

    private Camera mainCamera;
    private StarterAssets.StarterAssetsInputs inputs;

    private void Start()
    {
        mainCamera = Camera.main;
        inputs = GetComponent<StarterAssets.StarterAssetsInputs>();
    }

    private void Update()
    {
        // Check for punch inputs
        if (inputs.JabLeft || inputs.JabRight)
        {
            PerformPunch();
        }
    }

    private void PerformPunch()
    {
        // Get punch direction from camera forward
        Vector3 punchDirection = mainCamera.transform.forward;
        Vector3 punchStart = transform.position + Vector3.up * 1.5f; // Adjust height based on character

        // Perform spherecast to detect punchable objects
        RaycastHit[] hits = Physics.SphereCastAll(punchStart, punchRadius, punchDirection, punchRange, punchableLayer);

        foreach (RaycastHit hit in hits)
        {
            // Check if hit object implements IPunchable
            IPunchable punchable = hit.collider.GetComponent<IPunchable>();
            if (punchable != null)
            {
                // Apply punch effect
                punchable.OnPunch(punchDirection, punchForce);

                // Spawn visual effect if assigned
                if (punchVFX != null)
                {
                    Instantiate(punchVFX, hit.point, Quaternion.LookRotation(hit.normal));
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Debug visualization
        Gizmos.color = Color.red;
        Vector3 punchStart = transform.position + Vector3.up * 1.5f;
        Gizmos.DrawWireSphere(punchStart + transform.forward * punchRange, punchRadius);
    }
}
