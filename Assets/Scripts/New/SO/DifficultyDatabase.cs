using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace New.SO
{
    public enum DifficultyID
    {
        LEVEL_ONE,
        LEVEL_TWO,
        LEVEL_THREE,
        LEVEL_FOUR,
        LEVEL_FIVE,
        LEVEL_SIX,
        LEVEL_SEVEN,
        LEVEL_EIGHT
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

        public DifficultyData GetDifficultyData(DifficultyID difficultyID)
        {
            return _difficultyDataDict.GetValueOrDefault(difficultyID);
        }
    }
}