using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace New.SO
{
    public enum AchievementID
    {
        NONE,
        PERFECTHEALTH,
        DEFEATEDLASTOPPONENT,
        COMPLETETUTORIAL
    }

    [Serializable]
    public class AchievementData
    {
        public bool HasAchieved;
        public string Title;
        public string Description;
    }

    [CreateAssetMenu(fileName = "AchievementDatabase", menuName = "AchievementDatabase")]
    public class AchievementDatabase : ScriptableObject
    {
        [field: SerializeField] public SerializedDictionary<AchievementID, AchievementData> AchievementDataDict { get; set; } // todo: add to save file

        public bool SetAchievement(AchievementID id, bool hasAchieved)
        {
            if (!AchievementDataDict.TryGetValue(id, out AchievementData achievementData))
                return false;
            if (achievementData.HasAchieved)
                return false;

            achievementData.HasAchieved = hasAchieved;
            return true;
        }

        public AchievementData GetAchievement(AchievementID id)
        {
            return AchievementDataDict.GetValueOrDefault(id);
        }
    }
}