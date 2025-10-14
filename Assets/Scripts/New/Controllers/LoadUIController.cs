using New.Managers;
using UnityEngine;

namespace New.Controllers
{
    public class LoadUIController : BasePanel
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

        private void LoadSavaData(SaveData saveData)
        {
            Get.DifficultyDatabase.HighestDifficultyIDUnlocked = saveData.HighestDifficultyIDUnlocked;
            Get.UpgradeDatabase.UpgradeData = saveData.UpgradeData;
            
            Get.UIManager.ShowSingle(PanelType.PREFIGHT);
        }

        public void OnSlotOneClicked()
        {
            if (!Get.PlayerPrefManager.CheckIfSlotExists(Constants.SAVE_SLOT_ONE_KEY))
            {
                return;
            }

            var saveData = Get.PlayerPrefManager.GetSaveData(Constants.SAVE_SLOT_ONE_KEY);
            LoadSavaData(saveData);
        }

        public void OnSlotTwoClicked()
        {
            if (!Get.PlayerPrefManager.CheckIfSlotExists(Constants.SAVE_SLOT_TWO_KEY))
            {
                return;
            }
            
            var saveData = Get.PlayerPrefManager.GetSaveData(Constants.SAVE_SLOT_TWO_KEY);
            LoadSavaData(saveData);
        }

        public void OnSlotThreeClicked()
        {
            if (!Get.PlayerPrefManager.CheckIfSlotExists(Constants.SAVE_SLOT_THREE_KEY))
            {
                return;
            }
            
            var saveData = Get.PlayerPrefManager.GetSaveData(Constants.SAVE_SLOT_THREE_KEY);
            LoadSavaData(saveData);
        }
    }
}