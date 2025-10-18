using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace New.SO
{
    [Serializable]
    public class EnemyData
    {
        public string EnemyName;
        public Sprite EnemySprite;
        public int Wins;
        public int Losses;
        public int Height;
        public int Weight;
        //todo: maybe add the player skins here?
    }
    
    [CreateAssetMenu(fileName = "EnemyDatabase", menuName = "EnemyDatabase")]
    public class EnemyDatabase : ScriptableObject
    {
        [SerializeField]
        private SerializedDictionary<DifficultyID, EnemyData> _enemyDataDict;

        public EnemyData GetEnemyData(DifficultyID difficultyID)
        {
            return _enemyDataDict.GetValueOrDefault(difficultyID);
        }
    }
}