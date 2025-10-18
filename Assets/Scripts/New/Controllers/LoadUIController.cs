using System.Collections;
using System.Collections.Generic;
using New.Gameplay;
using New.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace New.Controllers
{
    public class LoadUIController : BasePanel
    {
        [SerializeField]
        private List<LoadButton> _slotLoadButtonList;

        // todo: add reference then check if it works
        [SerializeField]
        private TMP_InputField _userNameInputField;

        [SerializeField]
        private GameObject _failPanel;

        private Coroutine _failCoroutine;

        public override void OnInitialize()
        {
            base.OnInitialize();
            
            _userNameInputField.onValueChanged.AddListener(OnInputFieldTextChanged);
        }

        public override void OnShow()
        {
            base.OnShow();
            
            InitializeLoadSlots();
        }

        public override void OnHide()
        {
            base.OnHide();

            if (_failCoroutine != null)
            {
                StopCoroutine(_failCoroutine);
                _failCoroutine = null;
                _failPanel.SetActive(false);
                _userNameInputField.gameObject.SetActive(true);
            }
        }

        private void InitializeLoadSlots()
        {
            _slotLoadButtonList[0].Initialize(Constants.SAVE_SLOT_ONE_KEY);
            _slotLoadButtonList[1].Initialize(Constants.SAVE_SLOT_TWO_KEY);
            _slotLoadButtonList[2].Initialize(Constants.SAVE_SLOT_THREE_KEY);
        }

        private void OnInputFieldTextChanged(string text)
        {
            Get.PlayerPrefManager.CurrentUserName = _userNameInputField.text;
        }

        public void ShowFailPanel()
        {
            if (_failCoroutine != null)
                return;
            
            _failCoroutine = StartCoroutine(IEFailPanel());
        }

        private IEnumerator IEFailPanel()
        {
            _failPanel.SetActive(true);
            _userNameInputField.gameObject.SetActive(false);
            yield return new WaitForSeconds(3);
            _failPanel.SetActive(false);
            _userNameInputField.gameObject.SetActive(true);
            _failCoroutine = null;
        }

        public void OnBackButtonClicked()
        {
            Get.UIManager.ShowSingle(PanelType.MAINMENU);
        }
    }
}