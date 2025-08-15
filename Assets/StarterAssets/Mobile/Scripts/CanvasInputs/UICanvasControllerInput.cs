using UnityEngine;

namespace StarterAssets
{
    public class UICanvasControllerInput : MonoBehaviour
    {
        [Header("Output")]
        public StarterAssetsInputs starterAssetsInputs;

        public void VirtualMoveInput(Vector2 virtualMoveDirection)
        {
            if (starterAssetsInputs != null)
            {
                starterAssetsInputs.MoveInput(virtualMoveDirection);
            }
            else
            {
                Debug.LogError("StarterAssetsInputs is not assigned in UICanvasControllerInput.");
            }
        }

        public void VirtualLookInput(Vector2 virtualLookDirection)
        {
            if (starterAssetsInputs != null)
            {
                starterAssetsInputs.LookInput(virtualLookDirection);
            }
            else
            {
                Debug.LogError("StarterAssetsInputs is not assigned in UICanvasControllerInput.");
            }
        }
    }
}
