using New.Managers;

namespace New.Controllers
{
    public class SettingsUIController : BasePanel
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

        public void OnAdjustMasterVolume(float volume)
        {
            Get.AudioManager.SetMasterVolume(volume);
        }

        public void OnAdjustBGMVolume(float volume)
        {
            Get.AudioManager.SetBGMVolume(volume);
        }

        public void OnAdjustSFXVolume(float volume)
        {
            Get.AudioManager.SetSFXVolume(volume);
        }

        public void OnBackButtonPressed()
        {
            Get.UIManager.ShowSingle(PanelType.MAINMENU);
        }
    }
}