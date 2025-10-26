using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace New.SO
{
    public enum DifficultyID
    {
        DIVISION_ONE,
        DIVISION_TWO,
        DIVISION_THREE,
        DIVISION_FOUR,
        DIVISION_FIVE,
        DIVISION_SIX,
        DIVISION_SEVEN,
        DIVISION_EIGHT
    }

    [Serializable]
    public class DifficultyData
    {
        public int Health;
        public int ComboChance;
        public int AttackCooldown;
        public int EnergyRestoreTime;
        public int EnergyRestored;
    }

    [CreateAssetMenu(fileName = "DifficultyDatabase", menuName = "DifficultyDatabase")]
    public class DifficultyDatabase : ScriptableObject
    {
        [SerializeField]
        private SerializedDictionary<DifficultyID, DifficultyData> _difficultyDataDict;

        [SerializeField]
        private DifficultyID _highestDifficultyIDUnlocked;
        [field: SerializeField] public DifficultyID CurrentDifficultyID { get; set; } = DifficultyID.DIVISION_ONE;
        public DifficultyID HighestDifficultyIDUnlocked
        {
            get => _highestDifficultyIDUnlocked;
            set
            {
                _highestDifficultyIDUnlocked = value;
                // Get.PlayerPrefManager.SaveDifficultyData();
            }
        }

        public DifficultyData GetDifficultyData(DifficultyID difficultyID)
        {
            return _difficultyDataDict.GetValueOrDefault(difficultyID);
        }
    }
}