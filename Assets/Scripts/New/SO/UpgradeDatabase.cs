using System;
using Unity.VisualScripting.FullSerializer;
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
        [field: SerializeField] public int UpgradeTokens { get; set; }
        
        [field: SerializeField] public int HealthUpgradePrice { get; private set; }
        [field: SerializeField] public int EnergyRestoreTimeUpgradePrice { get; private set; }
        [field: SerializeField] public int EnergyRestoredUpgradePrice { get; private set; }
        
        [field: SerializeField] public UpgradeData UpgradeData { get; set; }

        public bool CanUpgradeHealth()
        {
            return UpgradeTokens >= HealthUpgradePrice;
        }

        public bool CanUpgradeEnergyRestoreTime()
        {
            if (Get.UpgradeDatabase.UpgradeData.EnergyRestoreTime <= 1)
            {
                return false;
            }
            
            return UpgradeTokens >= EnergyRestoreTimeUpgradePrice;
        }

        public bool CanUpgradeEnergyRestored()
        {
            return UpgradeTokens >= EnergyRestoredUpgradePrice;
        }
    }
}