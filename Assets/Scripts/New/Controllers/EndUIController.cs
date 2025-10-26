using New.Managers;
using TMPro;
using UnityEngine;

namespace New.Controllers
{
    public class EndUIController : BasePanel
    {
        [SerializeField]
        private TMP_Text _winnerNameText;

        [SerializeField]
        private TMP_Text _pointsAddedText;

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

        public void SetWinnerName(string winnerName, int pointsAdded = 0)
        {
            _winnerNameText.text = $"{winnerName} wins!";
            if (pointsAdded > 0)
            {
                _pointsAddedText.text = $"{pointsAdded} upgrade points added!";
                _pointsAddedText.gameObject.SetActive(true);
            }
            else
            {
                _pointsAddedText.gameObject.SetActive(false);
            }
        }

        public void OnBackToPrefightButtonClicked()
        {
            Get.UIManager.ShowSingle(PanelType.PREFIGHT);
            Get.GameManager.ResetEntities();
        }
    }
}