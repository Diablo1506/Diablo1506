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
        private EnemyController _trainingEnemyControllerPrefab;

        [SerializeField]
        private Transform _playerControllerSpawnPosition;

        [SerializeField]
        private Transform _enemyControllerSpawnPosition;

        [field: SerializeField] public RoundData CurrentRoundData { get; set; }
        [field: SerializeField] public PlayerController PlayerController { get; set; }
        [field: SerializeField] public EnemyController EnemyController { get; set; }
        [field: SerializeField] public bool IsRoundOver { get; set; }
        [field: SerializeField] public DifficultyID DifficultyID { get; set; }
        [field: SerializeField] public Transform LeftBoundary { get; set; }
        [field: SerializeField] public Transform RightBoundary { get; set; }

        [SerializeField]
        private bool _isInGame;
        public bool IsInGame
        {
            get => _isInGame;
            set => _isInGame = value;
        }

        private void Start()
        {
            // Get.PlayerPrefManager.LoadGame();
        }

        public void StartRound()
        {
            IsRoundOver = false;
            IsInGame = true;

            Get.AudioManager.PlaySFX(Get.AudioManager.BellClip);
            
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
            
            Get.UIManager.GetPanel<GameUIController>(PanelType.GAMEUI).CheckTrainingUI();
        }

        public void EndRound(Entity entity)
        {
            // entity is the loser
            IsRoundOver = true;
            IsInGame = false;
            _roundTimeController.StopRound();

            var gameUIController = Get.UIManager.GetPanel<GameUIController>(PanelType.GAMEUI);

            if (entity is PlayerController)
            {
                Debug.Log($"#{GetType()}: PLAYER LOST");
                _winDataDict[EnemyController] += 1;
                gameUIController.WinRoundCount.AddEnemyWin(_winDataDict[EnemyController]);
            }
            else
            {
                Debug.Log($"#{GetType()}: ENEMY LOST");
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

        public void StartTraining()
        {
            Get.UIManager.GetPanel<GameUIController>(PanelType.GAMEUI).CheckTrainingUI();
            StartCoroutine(IEStartTraining());
            return;

            IEnumerator IEStartTraining()
            {
                ResetTrainingEntities();
                yield return new WaitForSeconds(1);
                StartRound();
            }
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
            var enemyControllerPrefab = Get.EnemyDatabase.GetEnemyData(Get.DifficultyDatabase.CurrentDifficultyID).EnemyController;
            EnemyController = Instantiate(enemyControllerPrefab, _enemyControllerSpawnPosition);

            _winDataDict.Clear();
            _winDataDict.TryAdd(PlayerController, playerWinCount);
            _winDataDict.TryAdd(EnemyController, enemyWinCount);

            yield return new WaitForSeconds(1f);

            StartRound();
        }

        public void ResetEntities()
        {
            Destroy(PlayerController.gameObject);
            Destroy(EnemyController.gameObject);
            PlayerController = Instantiate(_playerControllerPrefab, _playerControllerSpawnPosition);
            var enemyControllerPrefab = Get.EnemyDatabase.GetEnemyData(Get.DifficultyDatabase.CurrentDifficultyID).EnemyController;
            EnemyController = Instantiate(enemyControllerPrefab, _enemyControllerSpawnPosition);
            _winDataDict.Clear();
            _winDataDict.TryAdd(PlayerController, 0);
            _winDataDict.TryAdd(EnemyController, 0);
        }

        public void ResetTrainingEntities()
        {
            Destroy(PlayerController.gameObject);
            Destroy(EnemyController.gameObject);
            PlayerController = Instantiate(_playerControllerPrefab, _playerControllerSpawnPosition);
            EnemyController = Instantiate(_trainingEnemyControllerPrefab, _enemyControllerSpawnPosition);
            EnemyController.IsTrainingDummy = true;
            _winDataDict.Clear();
            _winDataDict.TryAdd(PlayerController, 0);
            _winDataDict.TryAdd(EnemyController, 0);
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
            var endUIController = Get.UIManager.GetPanel<EndUIController>(PanelType.ENDUI);
            Get.UIManager.ShowSingle(PanelType.ENDUI);
            if (_winDataDict[PlayerController] > _winDataDict[EnemyController])
            {
                // win playercontroller

                if (PlayerController.EntityHealth >= Get.UpgradeDatabase.UpgradeData.Health)
                {
                    if (!Get.AchievementDatabase.SetAchievement(AchievementID.PERFECTHEALTH, true))
                        return;
                    Get.UIManager.AchievementPopUpController.ShowAchievementPopUp(AchievementID.PERFECTHEALTH);
                }

                if (Get.DifficultyDatabase.CurrentDifficultyID == DifficultyID.DIVISION_EIGHT)
                {
                    Get.AchievementDatabase.SetAchievement(AchievementID.DEFEATEDLASTOPPONENT, true);
                    Get.UIManager.AchievementPopUpController.ShowAchievementPopUp(AchievementID.DEFEATEDLASTOPPONENT);
                }
                
                if (Get.DifficultyDatabase.HighestDifficultyIDUnlocked != DifficultyID.DIVISION_EIGHT &&
                Get.DifficultyDatabase.HighestDifficultyIDUnlocked == Get.DifficultyDatabase.CurrentDifficultyID)
                {
                    Get.DifficultyDatabase.HighestDifficultyIDUnlocked++;
                    var enemyData = Get.EnemyDatabase.GetEnemyData(Get.DifficultyDatabase.CurrentDifficultyID);
                    Get.TrophyDatabase.SetDefeated(Get.DifficultyDatabase.CurrentDifficultyID, true);
                    Get.UpgradeDatabase.UpgradeTokens += 100; // add visuals saying you earned 100 points
                    endUIController.SetWinnerName("Pacquiao", 100);
                }
                else
                {
                    Get.UpgradeDatabase.UpgradeTokens += 10; // add visuals saying you earned 10 points
                    endUIController.SetWinnerName("Pacquiao", 10);
                }
            }
            else
            {
                // win enemycontroller
                DifficultyID currentDifficultyID = Get.DifficultyDatabase.CurrentDifficultyID;
                EnemyData enemyData = Get.EnemyDatabase.GetEnemyData(currentDifficultyID);
                endUIController.SetWinnerName(enemyData.EnemyName);
            }

            var gameUIController = Get.UIManager.GetPanel<GameUIController>(PanelType.GAMEUI);
            gameUIController.WinRoundCount.Reset();

            //show end game ui
            Get.PlayerPrefManager.SaveGame();
        }

        public void EndGameByQuit()
        {
            Time.timeScale = 1;
            IsInGame = false;
            _roundTimeController.StopRound();
            ResetEntities();
            
            var gameUIController = Get.UIManager.GetPanel<GameUIController>(PanelType.GAMEUI);
            gameUIController.WinRoundCount.Reset();

            Get.UIManager.ShowSingle(PanelType.PREFIGHT);
        }

        public void PauseGame()
        {
            Get.UIManager.ShowSingle(PanelType.PAUSE);
            Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            Get.UIManager.ShowSingle(PanelType.GAMEUI);
            Time.timeScale = 1;
        }
    }
}