using New.Managers;

namespace New.Controllers
{
    public class InstructionsUIController : BasePanel
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

        public void OnBackButtonClicked()
        {
            Get.UIManager.ShowSingle(PanelType.MAINMENU);
        }
    }
}