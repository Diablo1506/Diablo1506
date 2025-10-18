using New.Controllers;
using New.Managers;
using New.SO;
using TMPro;
using UnityEngine;

namespace New.Gameplay
{
    public class LoadButton : MonoBehaviour
    {
        [SerializeField]
        private SaveSlotID _saveSlotID;

        [SerializeField]
        private TMP_Text _userNameText;

        [SerializeField]
        private TMP_Text _upgradeTokensText;

        [SerializeField]
        private TMP_Text _healthText;

        [SerializeField]
        private TMP_Text _energyRestoreTimeText;

        [SerializeField]
        private TMP_Text _energyRestoredText;

        private SaveData _currentSaveData;

        public void Initialize(string slotKey)
        {
            _currentSaveData = Get.PlayerPrefManager.GetSaveData(slotKey);
            UpdateTexts();
        }

        private void UpdateTexts()
        {
            _userNameText.text = _currentSaveData == null ? "EMPTY" : _currentSaveData.UserName;
            _upgradeTokensText.text = _currentSaveData == null ? $"Tokens: 0" : $"Tokens: {_currentSaveData.UpgradeTokens}";
            _healthText.text = _currentSaveData == null ? $"Max Health: 0" : $"Max Health: {_currentSaveData.UpgradeData.Health}";
            _energyRestoreTimeText.text = _currentSaveData == null ? $"Energy Restore Time: 0" : $"Energy Restore Time: {_currentSaveData.UpgradeData.EnergyRestoreTime}";
            _energyRestoredText.text = _currentSaveData == null ? $"Energy Restored: 0" : $"Energy Restored: {_currentSaveData.UpgradeData.EnergyRestored}";
        }

        public void OnLoadButtonClicked()
        {
            if (_currentSaveData != null)
            {
                Get.PlayerPrefManager.CurrentUserName = _currentSaveData.UserName;
                Get.DifficultyDatabase.HighestDifficultyIDUnlocked = _currentSaveData.HighestDifficultyIDUnlocked;
                Get.UpgradeDatabase.UpgradeTokens = _currentSaveData.UpgradeTokens;
                Get.UpgradeDatabase.UpgradeData = _currentSaveData.UpgradeData;
            }
            else
            {
                if (string.IsNullOrEmpty(Get.PlayerPrefManager.CurrentUserName))
                {
                    Get.UIManager.GetPanel<LoadUIController>(PanelType.LOAD).ShowFailPanel();
                    return;
                }
                
                Get.DifficultyDatabase.HighestDifficultyIDUnlocked = DifficultyID.LEVEL_ONE;
                Get.UpgradeDatabase.UpgradeTokens = 0;
                Get.UpgradeDatabase.UpgradeData = new UpgradeData()
                {
                    Health = 100,
                    EnergyRestoreTime = 5,
                    EnergyRestored = 5
                };
            }

            Get.PlayerPrefManager.CurrentSaveSlotID = _saveSlotID;
            Get.UIManager.ShowSingle(PanelType.PREFIGHT);
        }
    }
}