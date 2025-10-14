using New.Managers;
using TMPro;
using UnityEngine;

namespace New.Controllers
{
    public class EndUIController : BasePanel
    {
        [SerializeField]
        private TMP_Text _winnerNameText;
        
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

        public void SetWinnerName(string winnerName)
        {
            _winnerNameText.text = $"{winnerName} wins!";
        }

        public void OnBackToPrefightButtonClicked()
        {
            Get.UIManager.ShowSingle(PanelType.PREFIGHT);
            Get.GameManager.ResetEntities();
        }
    }
}