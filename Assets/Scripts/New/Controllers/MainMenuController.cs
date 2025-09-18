using New.Managers;
using UnityEngine;

namespace New.Controllers
{
    public class MainMenuController : BasePanel
    {
        public override void OnInitialize()
        {
            base.OnInitialize();
        }
        
        public override void OnShow()
        {
            base.OnShow();
        }

        public override void OnHide()
        {
            base.OnHide();
        }
        
        public void OnPlayButtonClicked()
        {
            // temp
            Get.UIManager.ShowSingle(PanelType.PREFIGHT);
            // gameObject.SetActive(false);
            // Get.UIManager.GameUIController.gameObject.SetActive(true);
        }

        public void OnSettingsButtonClicked()
        {
            
        }

        public void OnQuitButtonClicked()
        {
            
        }
    }
}