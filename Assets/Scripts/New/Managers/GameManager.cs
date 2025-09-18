using System;
using System.Collections;
using New.Controllers;
using New.Gameplay;
using New.SO;
using UnityEngine;
using UnityEngine.Rendering;

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

        [SerializeField]
        private SerializedDictionary<Entity, int> _winDataDict = new SerializedDictionary<Entity, int>();

        [SerializeField]
        private PlayerController _playerControllerPrefab;

        [SerializeField]
        private EnemyController _enemyControllerPrefab;

        [SerializeField]
        private Transform _playerControllerSpawnPosition;

        [SerializeField]
        private Transform _enemyControllerSpawnPosition;

        [field: SerializeField] public RoundData CurrentRoundData { get; set; }
        [field: SerializeField] public PlayerController PlayerController { get; set; }
        [field: SerializeField] public EnemyController EnemyController { get; set; }
        [field: SerializeField] public bool IsRoundOver { get; set; }
        [field: SerializeField] public DifficultyID DifficultyID { get; set; }

        public void StartRound()
        {
            IsRoundOver = false;

            if (_winDataDict.Count == 0)
            {
                _winDataDict.TryAdd(PlayerController, 0);
                _winDataDict.TryAdd(EnemyController, 0);
            }

            _roundTimeController.Initialize(this);
            PlayerController.Initialize();

            EnemyController.Initialize(Get.DifficultyDatabase.GetDifficultyData(DifficultyID));

            PlayerController.OnRoundStart();
            EnemyController.OnRoundStart();
            
            Get.UIManager.GetPanel<GameUIController>(PanelType.GAMEUI).PlayerStaminaSliderBar.ChangeValue(PlayerController.EntityEnergy);
            Get.UIManager.GetPanel<GameUIController>(PanelType.GAMEUI).EnemyStaminaSliderBar.ChangeValue(EnemyController.EntityEnergy);
            
            _roundTimeController.StartRound();
        }

        public void EndRound(Entity entity)
        {
            // entity is the loser
            IsRoundOver = true;
            _roundTimeController.StopRound();

            var gameUIController = Get.UIManager.GetPanel<GameUIController>(PanelType.GAMEUI);

            if (entity is PlayerController)
            {
                _winDataDict[EnemyController] += 1;
                gameUIController.WinRoundCount.AddPlayerWin(_winDataDict[EnemyController]);
            }
            else
            {
                _winDataDict[PlayerController] += 1;
                gameUIController.WinRoundCount.AddPlayerWin(_winDataDict[PlayerController]);
            }

            if (_winDataDict[EnemyController] == 2 || _winDataDict[PlayerController] == 2)
            {
                EndGame();
                return;
            }

            StartCoroutine(IENextRound());
        }

        private IEnumerator IENextRound()
        {
            // temporary sa ani kay mag beta testing paman
            int playerWinCount = _winDataDict[PlayerController];
            int enemyWinCount = _winDataDict[EnemyController];
            yield return new WaitForSeconds(5f);
            Destroy(PlayerController.gameObject);
            Destroy(EnemyController.gameObject);
            yield return new WaitForSeconds(1f);
            PlayerController = Instantiate(_playerControllerPrefab, _playerControllerSpawnPosition);
            EnemyController = Instantiate(_enemyControllerPrefab, _enemyControllerSpawnPosition);
            
            _winDataDict.Clear();
            _winDataDict.TryAdd(PlayerController, playerWinCount);
            _winDataDict.TryAdd(EnemyController, enemyWinCount);
            
            yield return new WaitForSeconds(1f);

            StartRound();
        }

        public void EndRoundByTime()
        {
            if (PlayerController.EntityHealth > EnemyController.EntityHealth)
            {
                EndRound(PlayerController);
            }
            else
            {
                EndRound(EnemyController);
            }
        }

        private void EndGame()
        {
            if (_winDataDict[PlayerController] > _winDataDict[EnemyController])
            {
                // win playercontroller
            }
            else
            {
                // win enemycontroller
            }
            
            //show end game ui
        }
    }
}