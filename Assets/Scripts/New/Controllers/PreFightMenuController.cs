using New.Managers;

namespace New.Controllers
{
    public class PreFightMenuController : BasePanel
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

        public void OnFightButtonClicked()
        {
            Get.UIManager.ShowSingle(PanelType.GAMEUI);
        }

        public void OnTrainButtonClicked()
        {
            
        }
    }
}