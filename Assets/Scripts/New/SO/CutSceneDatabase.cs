using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace New.SO
{
    [Serializable]
    public class CutSceneData
    {
        // dpat equal ni sila
        public List<string> PlayerDialogueList;
        public List<string> EnemyDialogueList;
    }

    [CreateAssetMenu(fileName = "CutSceneDatabase", menuName = "CutSceneDatabase")]
    public class CutSceneDatabase : ScriptableObject
    {
        [SerializeField]
        private SerializedDictionary<DifficultyID, CutSceneData> _cutSceneDataDict;

        public CutSceneData GetCutSceneData(DifficultyID difficultyID)
        {
            return _cutSceneDataDict.GetValueOrDefault(difficultyID);
        }
    }
}