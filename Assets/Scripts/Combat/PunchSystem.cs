using UnityEngine;

public class PunchSystem : MonoBehaviour
{
    [Header("Punch Settings")]
    [SerializeField] private float punchForce = 10f;
    [SerializeField] private float punchRange = 2f;
    [SerializeField] private float punchRadius = 0.5f;
    [SerializeField] private LayerMask punchableLayer;

    [Header("Punch Type Forces")]
    [SerializeField] private float jabForce = 10f;
    [SerializeField] private float hookForce = 15f;
    [SerializeField] private float uppercutForce = 20f;

    [Header("Visual Feedback")]
    [SerializeField] private ParticleSystem punchVFX;
    [SerializeField] private ParticleSystem[] punchTypeVFX;

    private Camera mainCamera;
    private StarterAssets.StarterAssetsInputs inputs;
    private ComboSystem comboSystem;

    private void Start()
    {
        mainCamera = Camera.main;
        inputs = GetComponent<StarterAssets.StarterAssetsInputs>();
        comboSystem = GetComponent<ComboSystem>();
        
        if (comboSystem == null)
        {
            comboSystem = gameObject.AddComponent<ComboSystem>();
        }
    }

    private void Update()
    {
        // Check for punch inputs
        if (inputs.JabLeft)
        {
            PerformPunch(ComboSystem.PunchType.LeftJab, jabForce);
        }
        else if (inputs.JabRight)
        {
            PerformPunch(ComboSystem.PunchType.RightJab, jabForce);
        }
        
        // Hook inputs (you'll need to add these to StarterAssetsInputs)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            PerformPunch(ComboSystem.PunchType.LeftHook, hookForce);
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            PerformPunch(ComboSystem.PunchType.RightHook, hookForce);
        }
        
        // Uppercut inputs
        if (Input.GetKeyDown(KeyCode.Z))
        {
            PerformPunch(ComboSystem.PunchType.LeftUppercut, uppercutForce);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            PerformPunch(ComboSystem.PunchType.RightUppercut, uppercutForce);
        }
    }

    private void PerformPunch(ComboSystem.PunchType punchType, float baseForce)
    {
        // Register punch with combo system
        comboSystem.RegisterPunch(punchType);

        // Get punch direction based on type
        Vector3 punchDirection = GetPunchDirection(punchType);
        Vector3 punchStart = transform.position + Vector3.up * 1.5f;

        // Apply combo multiplier to force
        float finalForce = baseForce * comboSystem.GetCurrentComboMultiplier();

        // Perform spherecast to detect punchable objects
        RaycastHit[] hits = Physics.SphereCastAll(punchStart, punchRadius, punchDirection, punchRange, punchableLayer);

        foreach (RaycastHit hit in hits)
        {
            IPunchable punchable = hit.collider.GetComponent<IPunchable>();
            if (punchable != null)
            {
                // Apply punch effect with combo-modified force
                punchable.OnPunch(punchDirection, finalForce);

                // Spawn appropriate visual effect
                SpawnPunchVFX(punchType, hit.point, hit.normal);
            }
        }
    }

    private Vector3 GetPunchDirection(ComboSystem.PunchType punchType)
    {
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;
        Vector3 up = Vector3.up;

        switch (punchType)
        {
            case ComboSystem.PunchType.LeftHook:
            case ComboSystem.PunchType.RightHook:
                // Hooks have more horizontal component
                return (forward + (punchType == ComboSystem.PunchType.RightHook ? right : -right)).normalized;
            
            case ComboSystem.PunchType.LeftUppercut:
            case ComboSystem.PunchType.RightUppercut:
                // Uppercuts have more upward component
                return (forward + up * 1.5f).normalized;
            
            default: // Jabs
                return forward;
        }
    }

    private void SpawnPunchVFX(ComboSystem.PunchType punchType, Vector3 position, Vector3 normal)
    {
        // Spawn base punch VFX
        if (punchVFX != null)
        {
            Instantiate(punchVFX, position, Quaternion.LookRotation(normal));
        }

        // Spawn punch-type specific VFX if available
        int typeIndex = (int)punchType;
        if (punchTypeVFX != null && typeIndex < punchTypeVFX.Length && punchTypeVFX[typeIndex] != null)
        {
            Instantiate(punchTypeVFX[typeIndex], position, Quaternion.LookRotation(normal));
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
