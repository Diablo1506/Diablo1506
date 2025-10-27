using System;
using System.Collections.Generic;
using New.Managers;
using New.SO;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

namespace New.Controllers
{
    public class TrophyUIRoomController : BasePanel
    {
        // todo: add texts when a belt is clicked about the story behind it
        // todo: initialize the buttons

        [SerializeField]
        private SerializedDictionary<DifficultyID, GameObject> _trophyObjectDict;

        [SerializeField]
        private GameObject _buttonContainer;
        
        [SerializeField]
        private GameObject _pagesObject;

        [SerializeField]
        private List<TMP_Text> _pageTextList;
        private TrophyData _currentTrophyData;
        private int _currentPageIndex;

        public override void OnInitialize()
        {
            base.OnInitialize();
        }

        public override void OnShow()
        {
            base.OnShow();

            Get.EnvironmentManager.ToggleGymEnvironment();
            UpdateTrophyRoom();
        }

        public override void OnHide()
        {
            base.OnHide();
        }

        private void UpdateTrophyRoom()
        {
            foreach (var trophyObject in _trophyObjectDict.Values)
                trophyObject.SetActive(false);

            foreach (DifficultyID value in Enum.GetValues(typeof(DifficultyID)))
            {
                var trophyData = Get.TrophyDatabase.GetTrophyData(value);

                if (trophyData.Defeated && _trophyObjectDict.TryGetValue(value, out GameObject trophyObject))
                    trophyObject.SetActive(true);
            }
        }

        private void OpenPage(DifficultyID difficultyID)
        {
            _pagesObject.SetActive(true);
            _currentTrophyData = Get.TrophyDatabase.GetTrophyData(difficultyID);
            _currentPageIndex = 0;
            UpdatePageUI();
            
            _buttonContainer.SetActive(false);
        }

        private void UpdatePageUI()
        {
            if (_currentTrophyData == null) return;

            // Loop through all page text objects
            for(int i = 0; i < _pageTextList.Count; i++)
            {
                // Only enable the current page index
                bool isActivePage = (i == _currentPageIndex);
                _pageTextList[i].gameObject.SetActive(isActivePage);

                // Set text only for the active page
                if (isActivePage && i < _currentTrophyData.DescriptionPageList.Count)
                {
                    _pageTextList[i].text = _currentTrophyData.DescriptionPageList[i];
                }
                else
                {
                    _pageTextList[i].text = string.Empty;
                }
            }
        }


        public void OnPreviousPageClicked()
        {
            if (_currentTrophyData == null) return;

            _currentPageIndex--;
            if (_currentPageIndex < 0)
                _currentPageIndex = 0;

            UpdatePageUI();
        }

        public void OnNextPageClicked()
        {
            if (_currentTrophyData == null) return;

            _currentPageIndex++;
            if (_currentPageIndex >= _currentTrophyData.DescriptionPageList.Count)
                _currentPageIndex = _currentTrophyData.DescriptionPageList.Count - 1;

            UpdatePageUI();
        }

        public void OnTrophyOneClicked() => OpenPage(DifficultyID.DIVISION_ONE);
        public void OnTrophyTwoClicked() => OpenPage(DifficultyID.DIVISION_TWO);
        public void OnTrophyThreeClicked() => OpenPage(DifficultyID.DIVISION_THREE);
        public void OnTrophyFourClicked() => OpenPage(DifficultyID.DIVISION_FOUR);
        public void OnTrophyFiveClicked() => OpenPage(DifficultyID.DIVISION_FIVE);
        public void OnTrophySixClicked() => OpenPage(DifficultyID.DIVISION_SIX);
        public void OnTrophySevenClicked() => OpenPage(DifficultyID.DIVISION_SEVEN);
        public void OnTrophyEightClicked() => OpenPage(DifficultyID.DIVISION_EIGHT);

        public void OnClosePagesButton()
        {
            foreach (var pageText in _pageTextList)
                pageText.text = string.Empty;

            _pagesObject.SetActive(false);
            _currentTrophyData = null;
            
            _buttonContainer.SetActive(true);
        }

        public void OnBackButtonClicked()
        {
            OnClosePagesButton();
            
            Get.EnvironmentManager.ToggleStadiumEnvironment();
            Get.UIManager.ShowSingle(PanelType.PREFIGHT);
        }
    }
}