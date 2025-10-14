using System;
using New.SO;
using Newtonsoft.Json;
using UnityEngine;

namespace New.Managers
{
    [Serializable]
    public class SaveData
    {
        public DifficultyID HighestDifficultyIDUnlocked;
        public UpgradeData UpgradeData;
    }

    public class PlayerPrefManager : MonoBehaviour
    {
        public void LoadGame()
        {
            LoadUpgradeData();
            LoadDifficultyData();
        }

        public void SaveGame()
        {
            var saveData = new SaveData()
            {
                HighestDifficultyIDUnlocked = Get.DifficultyDatabase.HighestDifficultyIDUnlocked,
                UpgradeData = Get.UpgradeDatabase.UpgradeData
            };
            
            var jsonSaveData = JsonConvert.SerializeObject(saveData);
            // SaveUpgradeData();
            // SaveDifficultyData();
        }

        public bool CheckIfSlotExists(string slotKey)
        {
            return PlayerPrefs.HasKey(slotKey);
        }

        public SaveData GetSaveData(string slotKey)
        {
            var jsonSaveData = PlayerPrefs.GetString(slotKey);
            var saveData = JsonConvert.DeserializeObject<SaveData>(jsonSaveData);
            return saveData;
        }

        public void SaveDifficultyData()
        {
            var difficultyID = Get.DifficultyDatabase.HighestDifficultyIDUnlocked;
            PlayerPrefs.SetString(Constants.DIFFICULTY_KEY, difficultyID.ToString());
        }

        private void LoadDifficultyData()
        {
            if (!PlayerPrefs.HasKey(Constants.DIFFICULTY_KEY))
                return;

            var difficultyString = PlayerPrefs.GetString(Constants.DIFFICULTY_KEY);
            var difficultyID = (DifficultyID)Enum.Parse(typeof(DifficultyID), difficultyString);
            Get.DifficultyDatabase.HighestDifficultyIDUnlocked = difficultyID;
        }

        public void SaveUpgradeData()
        {
            var jsonData = JsonConvert.SerializeObject(Get.UpgradeDatabase.UpgradeData);
            PlayerPrefs.SetString(Constants.UPGRADE_KEY, jsonData);
        }

        private void LoadUpgradeData()
        {
            if (!PlayerPrefs.HasKey(Constants.UPGRADE_KEY))
            {
                Get.UpgradeDatabase.UpgradeData = new UpgradeData()
                {
                    Health = 100,
                    EnergyRestored = 5,
                    EnergyRestoreTime = 3
                };

                return;
            }

            var jsonData = PlayerPrefs.GetString(Constants.UPGRADE_KEY);
            var upgradeData = JsonConvert.DeserializeObject<UpgradeData>(jsonData);

            Get.UpgradeDatabase.UpgradeData = upgradeData;
        }
    }
}