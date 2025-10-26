using System;
using System.Collections;
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
            Get.TrophyDatabase.ResetDefeated();

            if (_currentSaveData != null)
            {
                Get.PlayerPrefManager.CurrentUserName = _currentSaveData.UserName;
                Get.DifficultyDatabase.HighestDifficultyIDUnlocked = _currentSaveData.HighestDifficultyIDUnlocked;
                Get.UpgradeDatabase.UpgradeTokens = _currentSaveData.UpgradeTokens;
                Get.UpgradeDatabase.UpgradeData = _currentSaveData.UpgradeData;

                foreach (var kvp in _currentSaveData.AchievementStatusDict)
                {
                    if (Get.AchievementDatabase.AchievementDataDict.TryGetValue(kvp.Key, out var achievementData))
                    {
                        achievementData.HasAchieved = kvp.Value;
                    }
                }


                foreach (DifficultyID value in Enum.GetValues(typeof(DifficultyID)))
                {
                    if (value == Get.DifficultyDatabase.HighestDifficultyIDUnlocked)
                        break;

                    Get.TrophyDatabase.SetDefeated(value, true);
                }
            }
            else
            {
                if (string.IsNullOrEmpty(Get.PlayerPrefManager.CurrentUserName))
                {
                    Get.UIManager.GetPanel<LoadUIController>(PanelType.LOAD).ShowFailPanel();
                    return;
                }

                Get.DifficultyDatabase.HighestDifficultyIDUnlocked = DifficultyID.DIVISION_ONE;
                Get.UpgradeDatabase.UpgradeTokens = 0;
                Get.UpgradeDatabase.UpgradeData = new UpgradeData()
                {
                    Health = 100,
                    EnergyRestoreTime = 5,
                    EnergyRestored = 5
                };

                foreach (var achievementData in Get.AchievementDatabase.AchievementDataDict.Values)
                {
                    achievementData.HasAchieved = false;
                }

            }

            Get.PlayerPrefManager.CurrentSaveSlotID = _saveSlotID;
            Get.UIManager.ShowSingle(PanelType.PREFIGHT);
        }

        public void OnClearButtonClicked()
        {
            switch (_saveSlotID)
            {
                case SaveSlotID.SLOTONE:
                    PlayerPrefs.DeleteKey(Constants.SAVE_SLOT_ONE_KEY);
                    break;
                case SaveSlotID.SLOTTWO:
                    PlayerPrefs.DeleteKey(Constants.SAVE_SLOT_TWO_KEY);
                    break;
                case SaveSlotID.SLOTTHREE:
                    PlayerPrefs.DeleteKey(Constants.SAVE_SLOT_THREE_KEY);
                    break;
            }

            StartCoroutine(IEClearButton());
        }

        private IEnumerator IEClearButton()
        {
            _userNameText.text = "CLEARED";
            _currentSaveData = null;
            yield return new WaitForSeconds(2);
            UpdateTexts();
        }
    }
}