using System;
using UnityEngine;

namespace New.SO
{
    [Serializable]
    public class UpgradeData
    {
        public int Health;
        public int EnergyRestoreTime;
        public int EnergyRestored;
    }
    
    [CreateAssetMenu(fileName = "UpgradeDatabase", menuName = "UpgradeDatabase")]
    public class UpgradeDatabase : ScriptableObject
    {
        [field: SerializeField] public UpgradeData UpgradeData { get; set; }
    }
}