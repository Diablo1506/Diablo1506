using UnityEngine;

/// <summary>
/// Debug helper for punch system - shows real-time punch detection info
/// Attach this to your player to see punch detection visualization and debug info
/// </summary>
public class PunchDebugger : MonoBehaviour
{
    [Header("Debug Settings")]
    [SerializeField] private bool showDebugInfo = true;
    [SerializeField] private bool showPunchRange = true;
    [SerializeField] private bool logPunchAttempts = true;
    [SerializeField] private KeyCode debugKey = KeyCode.F1;

    private PunchSystem punchSystem;
    private Camera playerCamera;

    private void Start()
    {
        punchSystem = GetComponent<PunchSystem>();
        playerCamera = Camera.main;
        
        if (punchSystem == null)
        {
            Debug.LogWarning("PunchDebugger: No PunchSystem found on this GameObject!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(debugKey))
        {
            showDebugInfo = !showDebugInfo;
            Debug.Log($"Punch Debug Info: {(showDebugInfo ? "ON" : "OFF")}");
        }

        if (showDebugInfo && logPunchAttempts)
        {
            // Check for punch inputs and log them
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                LogPunchAttempt();
            }
        }
    }

    private void LogPunchAttempt()
    {
        if (punchSystem == null) return;

        Debug.Log("=== PUNCH ATTEMPT DEBUG ===");
        
        // Get punch parameters using reflection
        var punchRangeField = typeof(PunchSystem).GetField("punchRange", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var punchRadiusField = typeof(PunchSystem).GetField("punchRadius", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var punchableLayerField = typeof(PunchSystem).GetField("punchableLayer", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (punchRangeField != null && punchRadiusField != null && punchableLayerField != null)
        {
            float punchRange = (float)punchRangeField.GetValue(punchSystem);
            float punchRadius = (float)punchRadiusField.GetValue(punchSystem);
            LayerMask punchableLayer = (LayerMask)punchableLayerField.GetValue(punchSystem);

            Vector3 punchStart = transform.position + Vector3.up * 1.5f;
            Vector3 punchDirection = playerCamera.transform.forward;

            Debug.Log($"Punch Start: {punchStart}");
            Debug.Log($"Punch Direction: {punchDirection}");
            Debug.Log($"Punch Range: {punchRange}");
            Debug.Log($"Punch Radius: {punchRadius}");
            Debug.Log($"Punchable Layer Mask: {punchableLayer.value}");

            // Perform the same detection as PunchSystem
            RaycastHit[] hits = Physics.SphereCastAll(punchStart, punchRadius, punchDirection, punchRange, punchableLayer);
            
            Debug.Log($"Objects in punch range: {hits.Length}");
            
            if (hits.Length == 0)
            {
                Debug.LogWarning("❌ No punchable objects detected!");
                
                // Check for objects on any layer
                RaycastHit[] allHits = Physics.SphereCastAll(punchStart, punchRadius, punchDirection, punchRange);
                Debug.Log($"Objects on any layer: {allHits.Length}");
                
                foreach (RaycastHit hit in allHits)
                {
                    string layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
                    Debug.Log($"  - {hit.collider.name} on layer: {layerName} ({hit.collider.gameObject.layer})");
                }
            }
            else
            {
                Debug.Log("✅ Punchable objects detected:");
                foreach (RaycastHit hit in hits)
                {
                    IPunchable punchable = hit.collider.GetComponent<IPunchable>();
                    string layerName = LayerMask.LayerToName(hit.collider.gameObject.layer);
                    Debug.Log($"  - {hit.collider.name} on layer: {layerName}, IPunchable: {punchable != null}");
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (!showPunchRange || punchSystem == null) return;

        // Draw punch detection area
        Gizmos.color = Color.red;
        Vector3 punchStart = transform.position + Vector3.up * 1.5f;
        Vector3 punchDirection = playerCamera != null ? playerCamera.transform.forward : transform.forward;
        
        // Get punch range using reflection
        var punchRangeField = typeof(PunchSystem).GetField("punchRange", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        var punchRadiusField = typeof(PunchSystem).GetField("punchRadius", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        if (punchRangeField != null && punchRadiusField != null)
        {
            float punchRange = (float)punchRangeField.GetValue(punchSystem);
            float punchRadius = (float)punchRadiusField.GetValue(punchSystem);

            Vector3 punchEnd = punchStart + punchDirection * punchRange;
            
            // Draw punch ray
            Gizmos.DrawLine(punchStart, punchEnd);
            
            // Draw punch sphere at end
            Gizmos.DrawWireSphere(punchEnd, punchRadius);
            
            // Draw punch start sphere
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(punchStart, 0.1f);
        }
    }

    private void OnGUI()
    {
        if (!showDebugInfo) return;

        GUILayout.BeginArea(new Rect(10, 10, 300, 200));
        GUILayout.BeginVertical("box");
        
        GUILayout.Label("PUNCH SYSTEM DEBUG");
        GUILayout.Label($"Press {debugKey} to toggle debug info");
        
        if (punchSystem != null)
        {
            GUILayout.Label("✅ PunchSystem found");
            
            // Show punch inputs
            GUILayout.Label("Punch Inputs:");
            GUILayout.Label($"  Left Mouse: {(Input.GetMouseButton(0) ? "PRESSED" : "Released")}");
            GUILayout.Label($"  Right Mouse: {(Input.GetMouseButton(1) ? "PRESSED" : "Released")}");
            GUILayout.Label($"  Q (Left Hook): {(Input.GetKey(KeyCode.Q) ? "PRESSED" : "Released")}");
            GUILayout.Label($"  E (Right Hook): {(Input.GetKey(KeyCode.E) ? "PRESSED" : "Released")}");
        }
        else
        {
            GUILayout.Label("❌ No PunchSystem found!");
        }

        if (GUILayout.Button("Test Punch Detection"))
        {
            LogPunchAttempt();
        }

        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
}
