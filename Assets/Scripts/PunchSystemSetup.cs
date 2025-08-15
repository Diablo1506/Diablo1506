using UnityEngine;

/// <summary>
/// Automatic setup script for the punch system to work with punchable objects
/// This script will configure layers, components, and settings automatically
/// </summary>
public class PunchSystemSetup : MonoBehaviour
{
    [Header("Setup Configuration")]
    [SerializeField] private string punchableLayerName = "Punchable";
    [SerializeField] private bool setupOnStart = true;
    [SerializeField] private bool debugMode = true;

    [Header("Manual Setup Tools")]
    [SerializeField] private GameObject[] objectsToMakePunchable;
    [SerializeField] private PunchSystem playerPunchSystem;

    private void Start()
    {
        if (setupOnStart)
        {
            AutoSetupPunchSystem();
        }
    }

    [ContextMenu("Auto Setup Punch System")]
    public void AutoSetupPunchSystem()
    {
        Debug.Log("=== STARTING PUNCH SYSTEM SETUP ===");

        // Step 1: Find or create the punchable layer
        int punchableLayerIndex = SetupPunchableLayer();
        
        // Step 2: Find the punch system
        PunchSystem punchSystem = FindPunchSystem();
        if (punchSystem == null)
        {
            Debug.LogError("❌ No PunchSystem found! Make sure your player has a PunchSystem component.");
            return;
        }

        // Step 3: Configure the punch system layer mask
        ConfigurePunchSystemLayerMask(punchSystem, punchableLayerIndex);

        // Step 4: Setup all boxing bags and punchable objects
        SetupPunchableObjects(punchableLayerIndex);

        // Step 5: Verify setup
        VerifySetup(punchSystem);

        Debug.Log("=== PUNCH SYSTEM SETUP COMPLETE ===");
    }

    private int SetupPunchableLayer()
    {
        int layerIndex = LayerMask.NameToLayer(punchableLayerName);
        
        if (layerIndex == -1)
        {
            Debug.LogWarning($"⚠️ Layer '{punchableLayerName}' not found!");
            Debug.Log($"Please create a layer named '{punchableLayerName}' in Edit > Project Settings > Tags and Layers");
            Debug.Log("For now, using Default layer (0)");
            return 0;
        }
        else
        {
            Debug.Log($"✅ Found layer '{punchableLayerName}' at index {layerIndex}");
            return layerIndex;
        }
    }

    private PunchSystem FindPunchSystem()
    {
        if (playerPunchSystem != null)
        {
            Debug.Log($"✅ Using assigned PunchSystem on {playerPunchSystem.gameObject.name}");
            return playerPunchSystem;
        }

        PunchSystem foundSystem = FindObjectOfType<PunchSystem>();
        if (foundSystem != null)
        {
            Debug.Log($"✅ Found PunchSystem on {foundSystem.gameObject.name}");
            return foundSystem;
        }

        Debug.LogError("❌ No PunchSystem found in scene!");
        return null;
    }

    private void ConfigurePunchSystemLayerMask(PunchSystem punchSystem, int layerIndex)
    {
        // Use reflection to set the private punchableLayer field
        var field = typeof(PunchSystem).GetField("punchableLayer", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (field != null)
        {
            LayerMask layerMask = 1 << layerIndex;
            field.SetValue(punchSystem, layerMask);
            Debug.Log($"✅ Set PunchSystem punchableLayer to layer {layerIndex} ({punchableLayerName})");
        }
        else
        {
            Debug.LogWarning("⚠️ Could not automatically set punchableLayer. Please set it manually in the inspector.");
            Debug.Log($"Set the 'Punchable Layer' field in PunchSystem to include layer '{punchableLayerName}'");
        }
    }

    private void SetupPunchableObjects(int layerIndex)
    {
        // Find all BoxingBag objects
        BoxingBag[] boxingBags = FindObjectsOfType<BoxingBag>();
        
        foreach (BoxingBag bag in boxingBags)
        {
            SetupSinglePunchableObject(bag.gameObject, layerIndex);
        }

        // Setup manually assigned objects
        if (objectsToMakePunchable != null)
        {
            foreach (GameObject obj in objectsToMakePunchable)
            {
                if (obj != null)
                {
                    SetupSinglePunchableObject(obj, layerIndex);
                }
            }
        }
    }

    private void SetupSinglePunchableObject(GameObject obj, int layerIndex)
    {
        Debug.Log($"Setting up punchable object: {obj.name}");

        // Set layer
        obj.layer = layerIndex;
        Debug.Log($"  ✅ Set layer to {punchableLayerName}");

        // Ensure Rigidbody
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = obj.AddComponent<Rigidbody>();
            Debug.Log("  ✅ Added Rigidbody");
        }

        // Ensure Collider
        Collider col = obj.GetComponent<Collider>();
        if (col == null)
        {
            col = obj.AddComponent<BoxCollider>();
            Debug.Log("  ✅ Added BoxCollider");
        }

        // Check for IPunchable implementation
        IPunchable punchable = obj.GetComponent<IPunchable>();
        if (punchable == null)
        {
            Debug.LogWarning($"  ⚠️ {obj.name} does not implement IPunchable interface!");
            Debug.Log("  Consider adding BoxingBag or PunchableObject component");
        }
        else
        {
            Debug.Log("  ✅ Implements IPunchable interface");
        }
    }

    private void VerifySetup(PunchSystem punchSystem)
    {
        Debug.Log("=== VERIFICATION ===");

        // Check punch system settings
        var field = typeof(PunchSystem).GetField("punchableLayer", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (field != null)
        {
            LayerMask currentMask = (LayerMask)field.GetValue(punchSystem);
            Debug.Log($"PunchSystem punchableLayer mask: {currentMask.value}");
        }

        // List all punchable objects
        BoxingBag[] bags = FindObjectsOfType<BoxingBag>();
        Debug.Log($"Found {bags.Length} BoxingBag objects:");
        
        foreach (BoxingBag bag in bags)
        {
            string layerName = LayerMask.LayerToName(bag.gameObject.layer);
            Debug.Log($"  - {bag.name} on layer: {layerName} ({bag.gameObject.layer})");
        }

        Debug.Log("=== SETUP INSTRUCTIONS ===");
        Debug.Log("1. Make sure your player has the PunchSystem component");
        Debug.Log("2. Check that the 'Punchable Layer' field in PunchSystem includes your punchable objects");
        Debug.Log("3. Test punching with mouse buttons (Left/Right click for jabs)");
        Debug.Log("4. Additional punch inputs: Q/E for hooks, Z/C for uppercuts");
    }

    [ContextMenu("Test Punch Detection")]
    public void TestPunchDetection()
    {
        PunchSystem punchSystem = FindPunchSystem();
        if (punchSystem == null) return;

        // Simulate a punch to test detection
        Vector3 punchStart = punchSystem.transform.position + Vector3.up * 1.5f;
        Vector3 punchDirection = punchSystem.transform.forward;
        
        // Get the punchable layer mask
        var field = typeof(PunchSystem).GetField("punchableLayer", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        
        if (field != null)
        {
            LayerMask punchableLayer = (LayerMask)field.GetValue(punchSystem);
            
            RaycastHit[] hits = Physics.SphereCastAll(punchStart, 0.5f, punchDirection, 2f, punchableLayer);
            
            Debug.Log($"=== PUNCH DETECTION TEST ===");
            Debug.Log($"Punch start: {punchStart}");
            Debug.Log($"Punch direction: {punchDirection}");
            Debug.Log($"Layer mask: {punchableLayer.value}");
            Debug.Log($"Objects detected: {hits.Length}");
            
            foreach (RaycastHit hit in hits)
            {
                Debug.Log($"  - Hit: {hit.collider.name} at distance {hit.distance}");
                IPunchable punchable = hit.collider.GetComponent<IPunchable>();
                if (punchable != null)
                {
                    Debug.Log($"    ✅ Has IPunchable interface");
                }
                else
                {
                    Debug.Log($"    ❌ Missing IPunchable interface");
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw punch detection visualization
        PunchSystem punchSystem = FindPunchSystem();
        if (punchSystem != null)
        {
            Gizmos.color = Color.green;
            Vector3 punchStart = punchSystem.transform.position + Vector3.up * 1.5f;
            Vector3 punchEnd = punchStart + punchSystem.transform.forward * 2f;
            
            Gizmos.DrawLine(punchStart, punchEnd);
            Gizmos.DrawWireSphere(punchEnd, 0.5f);
        }
    }
}
