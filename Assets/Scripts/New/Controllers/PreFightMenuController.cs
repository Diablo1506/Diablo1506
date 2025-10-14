using System;
using System.Collections.Generic;
using New.Managers;
using New.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace New.Controllers
{
    public class PreFightMenuController : BasePanel
    {
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
            Get.UIManager.ShowSingle(PanelType.CUTSCENE);
        }

        public void OnTrainButtonClicked()
        {
            Get.UIManager.ShowSingle(PanelType.GAMEUI);
            Get.GameManager.StartTraining();
        }

        private void OnUpgradeHealthButtonClicked()
        {
            if (!Get.UpgradeDatabase.CanUpgradeHealth())
                return;

            Get.UpgradeDatabase.UpgradeTokens -= 10;
            Get.UpgradeDatabase.UpgradeData.Health += 1;
            Get.PlayerPrefManager.SaveUpgradeData();

            UpdateUI();
        }

        private void OnUpgradeEnergyRestoreTimeButtonClicked()
        {
            if (!Get.UpgradeDatabase.CanUpgradeEnergyRestoreTime())
                return;

            Get.UpgradeDatabase.UpgradeTokens -= 100;
            Get.UpgradeDatabase.UpgradeData.EnergyRestoreTime -= 1;
            
            Get.PlayerPrefManager.SaveUpgradeData();

            UpdateUI();
        }

        private void OnUpgradeEnergyRestoredButtonClicked()
        {
            if (!Get.UpgradeDatabase.CanUpgradeEnergyRestored())
                return;

            Get.UpgradeDatabase.UpgradeTokens -= 10;
            Get.UpgradeDatabase.UpgradeData.EnergyRestored += 1;
            Get.PlayerPrefManager.SaveUpgradeData();

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
        }

        private void UpdateDropdown()
        {
            _difficultyDropdown.ClearOptions();

            DifficultyID[] values = (DifficultyID[])Enum.GetValues(typeof(DifficultyID));

            foreach (DifficultyID id in values)
            {
                _difficultyDropdown.options.Add(new TMP_Dropdown.OptionData(id.ToString()));

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
        }

        public void OnBackButtonClicked()
        {
            Get.UIManager.ShowSingle(PanelType.MAINMENU);
        }
    }
}