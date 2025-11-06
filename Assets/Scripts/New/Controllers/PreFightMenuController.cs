using System;
using System.Collections.Generic;
using New.Managers;
using New.SO;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace New.Controllers
{
    public class PreFightMenuController : BasePanel
    {
        [Header("Pre-Fight Menu")]
        [SerializeField]
        private TMP_Text _tokensText;

        [SerializeField]
        private Button _upgradeHealthButton;

        [SerializeField]
        private TMP_Text _healthAmountText;

        [SerializeField]
        private Button _upgradeEnergyRestoreTimeButton;

        [SerializeField]
        private TMP_Text _energyRestoreTimeText;

        [SerializeField]
        private Button _upgradeEnergyRestoredButton;

        [SerializeField]
        private TMP_Text _energyRestoredText;

        [SerializeField]
        private TMP_Text _enemyName;

        [SerializeField]
        private Image _enemyImage;

        [SerializeField]
        private TMP_Dropdown _difficultyDropdown;

        [Header("Enemy Stats")]
        [SerializeField]
        private TMP_Text _winsText;

        [SerializeField]
        private TMP_Text _lossText;

        [SerializeField]
        private TMP_Text _weightClassText;

        [FormerlySerializedAs("_heightText")]
        [SerializeField]
        private TMP_Text _healthText;

        [FormerlySerializedAs("_weightText")]
        [SerializeField]
        private TMP_Text _staminaText;

        public override void OnInitialize()
        {
            base.OnInitialize();
        }

        public override void OnShow()
        {
            base.OnShow();

            _upgradeHealthButton.onClick.AddListener(OnUpgradeHealthButtonClicked);
            _upgradeEnergyRestoreTimeButton.onClick.AddListener(OnUpgradeEnergyRestoreTimeButtonClicked);
            _upgradeEnergyRestoredButton.onClick.AddListener(OnUpgradeEnergyRestoredButtonClicked);
            _difficultyDropdown.onValueChanged.AddListener(SelectDropdownOption);

            UpdateUI();
            
            bool hasCompletedTutorial = Get.AchievementDatabase.GetAchievement(AchievementID.COMPLETETUTORIAL).HasAchieved;

            if (!hasCompletedTutorial)
            {
                Get.UIManager.GetPanel<GameUIController>(PanelType.GAMEUI).StartTutorial();
            }
        }

        public override void OnHide()
        {
            base.OnHide();

            _upgradeHealthButton.onClick.RemoveAllListeners();
            _upgradeEnergyRestoreTimeButton.onClick.RemoveAllListeners();
            _upgradeEnergyRestoredButton.onClick.RemoveAllListeners();
            _difficultyDropdown.onValueChanged.RemoveAllListeners();
        }

        public void OnFightButtonClicked()
        {
            Get.GameManager.ResetEntities();
            Get.UIManager.ShowSingle(PanelType.CUTSCENE);
        }

        public void OnTrainButtonClicked()
        {
            Get.UIManager.ShowSingle(PanelType.GAMEUI);
            Get.GameManager.StartTraining();
            
            Get.EnvironmentManager.ToggleGymEnvironment();
        }

        private void OnUpgradeHealthButtonClicked()
        {
            if (!Get.UpgradeDatabase.CanUpgradeHealth())
                return;

            Get.UpgradeDatabase.UpgradeTokens -= 10;
            Get.UpgradeDatabase.UpgradeData.Health += 1;

            UpdateUI();
        }

        private void OnUpgradeEnergyRestoreTimeButtonClicked()
        {
            if (!Get.UpgradeDatabase.CanUpgradeEnergyRestoreTime())
                return;

            Get.UpgradeDatabase.UpgradeTokens -= 100;
            Get.UpgradeDatabase.UpgradeData.EnergyRestoreTime -= 1;
            
            UpdateUI();
        }

        private void OnUpgradeEnergyRestoredButtonClicked()
        {
            if (!Get.UpgradeDatabase.CanUpgradeEnergyRestored())
                return;

            Get.UpgradeDatabase.UpgradeTokens -= 10;
            Get.UpgradeDatabase.UpgradeData.EnergyRestored += 1;

            UpdateUI();
        }

        private void UpdateUI()
        {
            _tokensText.text = $"Upgrade Tokens: {Get.UpgradeDatabase.UpgradeTokens}";
            _healthAmountText.text = Get.UpgradeDatabase.UpgradeData.Health.ToString();
            _energyRestoreTimeText.text = Get.UpgradeDatabase.UpgradeData.EnergyRestoreTime.ToString();
            _energyRestoredText.text = Get.UpgradeDatabase.UpgradeData.EnergyRestored.ToString();

            DifficultyID currentDifficultyID = Get.DifficultyDatabase.CurrentDifficultyID;
            EnemyData enemyData = Get.EnemyDatabase.GetEnemyData(currentDifficultyID);
            _enemyName.text = enemyData.EnemyName;
            _enemyImage.sprite = enemyData.EnemySprite;
            
            
            
            UpdateDropdown();
            
            Get.PlayerPrefManager.SaveGame();
        }

        private void UpdateDropdown()
        {
            _difficultyDropdown.ClearOptions();

            DifficultyID[] values = (DifficultyID[])Enum.GetValues(typeof(DifficultyID));

            foreach (DifficultyID id in values)
            {
                string displayName = id.ToString().Replace("_", " ");
                _difficultyDropdown.options.Add(new TMP_Dropdown.OptionData(displayName));

                if ((int)id == (int)Get.DifficultyDatabase.HighestDifficultyIDUnlocked)
                {
                    break;
                }
            }
            
            SelectDropdownOption((int)Get.DifficultyDatabase.CurrentDifficultyID);
        }

        private void SelectDropdownOption(int index)
        {
            Get.DifficultyDatabase.CurrentDifficultyID = (DifficultyID)index;
            
            DifficultyID currentDifficultyID = Get.DifficultyDatabase.CurrentDifficultyID;
            EnemyData enemyData = Get.EnemyDatabase.GetEnemyData(currentDifficultyID);
            _enemyName.text = enemyData.EnemyName;
            _enemyImage.sprite = enemyData.EnemySprite;
            
            DifficultyData difficultyData = Get.DifficultyDatabase.GetDifficultyData(currentDifficultyID);

            _winsText.text = $"Wins {enemyData.Wins}";
            _lossText.text = $"{enemyData.Losses} Losses";
            _weightClassText.text = enemyData.WeightClass;
            _healthText.text = $"Health: {difficultyData.Health}"; // changing to health
            _staminaText.text = $"Stamina Restored Per {difficultyData.EnergyRestoreTime} seconds: {difficultyData.EnergyRestored}"; // changing to stamina
        }

        public void OnBackButtonClicked()
        {
            Get.UIManager.ShowSingle(PanelType.MAINMENU);
        }

        public void OnTrophyButtonClicked()
        {
            Get.UIManager.ShowSingle(PanelType.TROPHY);
        }

        public void OnAchievementButtonClicked()
        {
            Get.UIManager.ShowSingle(PanelType.ACHIEVEMENT);
        }
    }
}