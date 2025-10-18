using System;
using New.SO;
using Newtonsoft.Json;
using Sirenix.OdinInspector;
using UnityEngine;

namespace New.Managers
{
    [Serializable]
    public class SaveData
    {
        public string UserName;
        public DifficultyID HighestDifficultyIDUnlocked;
        public int UpgradeTokens;
        public UpgradeData UpgradeData;
    }

    public enum SaveSlotID
    {
        SLOTONE,
        SLOTTWO,
        SLOTTHREE
    }

    public class PlayerPrefManager : MonoBehaviour
    {
        [field: SerializeField] public string CurrentUserName { get; set; }
        [field: SerializeField] public SaveSlotID CurrentSaveSlotID { get; set; }
        
        public void SaveGame()
        {
            string slotKey = null;
            
            switch (CurrentSaveSlotID)
            {
                case SaveSlotID.SLOTONE:
                    slotKey = Constants.SAVE_SLOT_ONE_KEY;
                    break;
                case SaveSlotID.SLOTTWO:
                    slotKey = Constants.SAVE_SLOT_TWO_KEY;
                    break;
                case SaveSlotID.SLOTTHREE:
                    slotKey = Constants.SAVE_SLOT_THREE_KEY;
                    break;
            }
            
            var saveData = new SaveData()
            {
                UserName = CurrentUserName,
                HighestDifficultyIDUnlocked = Get.DifficultyDatabase.HighestDifficultyIDUnlocked,
                UpgradeTokens = Get.UpgradeDatabase.UpgradeTokens,
                UpgradeData = Get.UpgradeDatabase.UpgradeData
            };

            var jsonSaveData = JsonConvert.SerializeObject(saveData);
            PlayerPrefs.SetString(slotKey, jsonSaveData);
            // SaveUpgradeData();
            // SaveDifficultyData();
        }

        public SaveData GetSaveData(string slotKey)
        {
            var jsonSaveData = PlayerPrefs.GetString(slotKey);
            var saveData = JsonConvert.DeserializeObject<SaveData>(jsonSaveData);
            return saveData;
        }

        [Button]
        private void DebugClearAllData()
        {
            PlayerPrefs.DeleteKey(Constants.SAVE_SLOT_ONE_KEY);
            PlayerPrefs.DeleteKey(Constants.SAVE_SLOT_TWO_KEY);
            PlayerPrefs.DeleteKey(Constants.SAVE_SLOT_THREE_KEY);
        }
    }
}