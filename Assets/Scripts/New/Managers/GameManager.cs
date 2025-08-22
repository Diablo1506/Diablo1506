using System;
using System.Collections;
using New.Controllers;
using New.Gameplay;
using Sirenix.OdinInspector;
using UnityEngine;

namespace New.Managers
{
    [Serializable]
    public class RoundData
    {
        public EnemyController EnemyControllerPrefab;
        public int RoundTime;
    }
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        private RoundTimeController _roundTimeController;
        
        [field: SerializeField] public RoundData CurrentRoundData { get; set; }
        [field: SerializeField] public PlayerController PlayerController { get; set; }
        [field: SerializeField] public EnemyController EnemyController { get; set; }

        private void Awake()
        {
            _roundTimeController.Initialize(this);
            PlayerController.Initialize();
            // EnemyController.Initialize();
        }

        private void Start()
        {
            // temp
            StartRound();
        }

        private void StartRound()
        {
            PlayerController.OnRoundStart();
            _roundTimeController.StartRound();
        }
    }
}