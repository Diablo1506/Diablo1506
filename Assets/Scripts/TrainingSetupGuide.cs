using UnityEngine;

/// <summary>
/// Setup Guide for Making Objects Punchable in Training Ground
/// 
/// This script provides instructions and helper methods for setting up punchable objects
/// in your training environment. Attach this to any GameObject for easy setup.
/// </summary>
public class TrainingSetupGuide : MonoBehaviour
{
    [Header("Setup Instructions")]
    [TextArea(10, 20)]
    public string setupInstructions = @"
BOXING BAG SETUP GUIDE FOR TRAINING GROUND:

1. LAYER SETUP:
   - Create a layer called 'Punchable' (or use existing layer)
   - Set your BoxingBag GameObject to this layer
   - In PunchSystem component, set 'Punchable Layer' to match this layer

2. BOXING BAG REQUIREMENTS:
   - BoxingBag script (already implements IPunchable)
   - Rigidbody component (automatically added)
   - Collider component (for punch detection)

3. PUNCH SYSTEM SETUP:
   - Ensure your player has PunchSystem component
   - Configure punch settings (force, range, radius)
   - Set the punchableLayer mask to include your Punchable layer

4. OPTIONAL ENHANCEMENTS:
   - Add AudioClip to punchImpactSound for sound feedback
   - Add ParticleSystem to hitEffect for visual feedback
   - Adjust punchForceMultiplier for desired impact strength

5. TESTING:
   - Enter Play mode
   - Approach the boxing bag
   - Use punch inputs (default: Mouse buttons for jabs)
   - Check console for hit feedback messages

TROUBLESHOOTING:
- If punches don't register: Check layer settings
- If bag doesn't swing: Check Rigidbody and Joint setup
- If no sound: Assign AudioClip to punchImpactSound
- If no visual effects: Assign ParticleSystem to hitEffect
";

    [Header("Quick Setup Tools")]
    [SerializeField] private LayerMask punchableLayerMask = 1;
    [SerializeField] private AudioClip defaultPunchSound;
    [SerializeField] private ParticleSystem defaultHitEffect;

    [Header("Auto-Setup Options")]
    [SerializeField] private bool autoSetupOnStart = false;
    [SerializeField] private string punchableLayerName = "Punchable";

    private void Start()
    {
        if (autoSetupOnStart)
        {
            AutoSetupBoxingBag();
        }
    }

    [ContextMenu("Auto Setup Boxing Bag")]
    public void AutoSetupBoxingBag()
    {
        BoxingBag boxingBag = GetComponent<BoxingBag>();
        if (boxingBag == null)
        {
            Debug.LogWarning("No BoxingBag component found on this GameObject!");
            return;
        }

        // Set layer
        int layerIndex = LayerMask.NameToLayer(punchableLayerName);
        if (layerIndex == -1)
        {
            Debug.LogWarning($"Layer '{punchableLayerName}' not found. Please create it in the Layer settings.");
        }
        else
        {
            gameObject.layer = layerIndex;
            Debug.Log($"Set GameObject layer to '{punchableLayerName}'");
        }

        // Ensure Rigidbody exists
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
            Debug.Log("Added Rigidbody component");
        }

        // Ensure Collider exists
        Collider col = GetComponent<Collider>();
        if (col == null)
        {
            gameObject.AddComponent<BoxCollider>();
            Debug.Log("Added BoxCollider component");
        }

        // Setup audio if provided
        if (defaultPunchSound != null)
        {
            // Use reflection to set the private field (for demonstration)
            Debug.Log("Default punch sound available - assign manually in inspector");
        }

        Debug.Log("Boxing Bag auto-setup completed!");
    }

    [ContextMenu("Find Punch System")]
    public void FindAndConfigurePunchSystem()
    {
        PunchSystem punchSystem = FindObjectOfType<PunchSystem>();
        if (punchSystem == null)
        {
            Debug.LogWarning("No PunchSystem found in the scene!");
            return;
        }

        Debug.Log($"Found PunchSystem on: {punchSystem.gameObject.name}");
        Debug.Log("Make sure to set the 'Punchable Layer' in PunchSystem to match your boxing bag's layer.");
    }

    [ContextMenu("Test Boxing Bag Setup")]
    public void TestBoxingBagSetup()
    {
        BoxingBag boxingBag = GetComponent<BoxingBag>();
        if (boxingBag == null)
        {
            Debug.LogError("❌ No BoxingBag component found!");
            return;
        }

        // Check IPunchable implementation
        IPunchable punchable = boxingBag as IPunchable;
        if (punchable != null)
        {
            Debug.Log("✅ BoxingBag implements IPunchable interface");
        }

        // Check required components
        Rigidbody rb = GetComponent<Rigidbody>();
        Collider col = GetComponent<Collider>();

        Debug.Log($"✅ Rigidbody: {(rb != null ? "Present" : "❌ Missing")}");
        Debug.Log($"✅ Collider: {(col != null ? "Present" : "❌ Missing")}");
        Debug.Log($"✅ Layer: {LayerMask.LayerToName(gameObject.layer)}");

        // Test punch simulation
        if (Application.isPlaying)
        {
            Vector3 testDirection = Vector3.forward;
            float testForce = 10f;
            punchable.OnPunch(testDirection, testForce);
            Debug.Log("🥊 Test punch applied!");
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw punch detection area visualization
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2f); // Approximate punch range
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, transform.localScale);
    }
}
