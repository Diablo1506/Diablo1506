using System;
using System.Collections;
using New.Gameplay;
using New.Managers;
using New.SO;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace New.Controllers
{
    public enum TutorialID
    {
        NONE,
        MOVE,
        JAB,
        HOOK,
        UPPERCUT,
        BLOCK,
        PUNCHINGBAG,
        EXIT
    }
    public class GameUIController : BasePanel
    {
        [SerializeField]
        private TMP_Text _timeText;

        [SerializeField]
        private TMP_Text _countDownText;

        [SerializeField]
        private GameObject _tutorialPanel;

        [SerializeField] private SerializedDictionary<TutorialID, GameObject> _tutorialStepObjects;
        private TutorialID _currentTutorialStep = TutorialID.MOVE;
        private bool _tutorialActive;


        [SerializeField]
        private TMP_Text _pointsAddedText;
        private int _pointsAdded;

        public int CurrentRound = 0;

        [field: SerializeField] public WinRoundCount WinRoundCount { get; set; }

        [field: Title("Player"), Space(10)]
        [field: SerializeField] public SliderBarUI PlayerHealthSliderBar { get; set; }
        [field: SerializeField] public SliderBarUI PlayerStaminaSliderBar { get; set; }

        [field: Title("Enemy"), Space(10)]
        [field: SerializeField] public SliderBarUI EnemyHealthSliderBar { get; set; }
        [field: SerializeField] public SliderBarUI EnemyStaminaSliderBar { get; set; }

        public override void OnInitialize()
        {
            base.OnInitialize();

            _countDownText.gameObject.SetActive(false);
        }

        public override void OnShow()
        {
            base.OnShow();

            Get.AudioManager.PlayBGM(Get.AudioManager.InGameBGMClip);
        }

        public override void OnHide()
        {
            base.OnHide();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && Get.GameManager.IsInGame)
            {
                Get.GameManager.PauseGame();
            }
        }

        public void CheckTrainingUI()
        {
            _pointsAdded = 0;
            if (Get.GameManager.EnemyController.IsTrainingDummy)
            {
                _pointsAddedText.gameObject.SetActive(true);
                _pointsAddedText.text = $"Points Added: {_pointsAdded}";
            }
            else
            {
                _pointsAddedText.gameObject.SetActive(false);
            }
        }

        public void DisableTrainingUI()
        {
            _pointsAddedText.gameObject.SetActive(false);
        }

        public void StartCountDown()
        {
            DisableTrainingUI();
            StartCoroutine(IECountDown());
        }

        private IEnumerator IECountDown()
        {
            _countDownText.gameObject.SetActive(true);

            // round number
            CurrentRound++;
            _countDownText.text = $"Round {CurrentRound}";
            yield return new WaitForSeconds(3f);
            
            int countDown = 3;
            _countDownText.text = countDown.ToString(); // show immediately

            while (countDown > 0)
            {
                yield return new WaitForSeconds(1f);
                countDown--;
                _countDownText.text = countDown > 0 ? countDown.ToString() : "FIGHT!";
            }

            yield return new WaitForSeconds(1f);

            _countDownText.gameObject.SetActive(false);
            Get.GameManager.StartRound();
        }

        public void SetKnockdownText(bool isKnockout)
        {
            _countDownText.gameObject.SetActive(true);
            _countDownText.text = isKnockout ? "K.O" : "Downed";
        }

        public void SetTime(int time)
        {
            _timeText.text = time.ToString();
        }

        public void StartTutorial()
        {
            _tutorialActive = true;
            _currentTutorialStep = TutorialID.MOVE;

            _tutorialPanel.SetActive(true);

            // Hide all
            foreach (var go in _tutorialStepObjects.Values)
                go.SetActive(false);

            ShowCurrentTutorialStep();
        }

        public void CompleteTutorialStep(TutorialID tutorialID)
        {
            if (!_tutorialActive)
                return;

            if (tutorialID != _currentTutorialStep)
                return;

            // Hide current step
            _tutorialStepObjects[tutorialID].SetActive(false);

            // Move to next step
            _currentTutorialStep++;

            if (_currentTutorialStep > TutorialID.EXIT)
            {
                _tutorialPanel.SetActive(false);
                _tutorialActive = false;
                Debug.Log("Tutorial complete!");
                Get.AchievementDatabase.GetAchievement(AchievementID.COMPLETETUTORIAL).HasAchieved = true;
                return;
            }

            ShowCurrentTutorialStep();
        }

        private void ShowCurrentTutorialStep()
        {
            if (_tutorialStepObjects.TryGetValue(_currentTutorialStep, out var go))
                go.SetActive(true);
        }

        public void AddPoints(int points)
        {
            _pointsAdded += points;
            _pointsAddedText.text = $"Points Added: {_pointsAdded}";
        }

    }
}