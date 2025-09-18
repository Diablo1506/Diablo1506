using New.Managers;
using UnityEngine;

namespace New.Controllers
{
    public class BasePanel : MonoBehaviour
    {
        [field: SerializeField] public PanelType PanelType { get; set; }

        public virtual void OnInitialize()
        {
            
        }
        
        public virtual void OnShow()
        {
            gameObject.SetActive(true);
            Get.CameraManager.SetCamera(PanelType);
        }

        public virtual void OnHide()
        {
            gameObject.SetActive(false);
        }
    }
}