using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace New.SO
{
    [Serializable]
    public class TrophyData
    {
        // todo: add video clip
        public List<string> DescriptionPageList;
        public bool Defeated; // todo: add this part to save data
    }
    
    [CreateAssetMenu(fileName = "TrophyDatabase", menuName = "TrophyDatabase")]
    public class TrophyDatabase : ScriptableObject
    {
        [field: SerializeField] public SerializedDictionary<DifficultyID, TrophyData> TrophyDataDict { get; private set; }
        
        public void SetDefeated(DifficultyID difficultyID, bool isDefeated)
        {
            if (TrophyDataDict.TryGetValue(difficultyID, out TrophyData trophyData))
            {
                trophyData.Defeated = isDefeated;
            }
        }

        public void ResetDefeated()
        {
            foreach (var trophyData in TrophyDataDict.Values)
            {
                trophyData.Defeated = false;
            }
        }

        public TrophyData GetTrophyData(DifficultyID difficultyID)
        {
            return TrophyDataDict.GetValueOrDefault(difficultyID);

        }
    }
}