using System;
using System.Collections;
using New.Gameplay;
using New.Managers;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace New.Controllers
{
    public class GameUIController : BasePanel
    {
        [SerializeField]
        private TMP_Text _timeText;

        [SerializeField]
        private TMP_Text _countDownText;

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

        public void StartCountDown()
        {
            // todo: cut scene sa dri.a
            StartCoroutine(IECountDown());
        }

        private IEnumerator IECountDown()
        {
            _countDownText.gameObject.SetActive(true);
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
        
        public void SetTime(int time)
        {
            _timeText.text = time.ToString();
        }
    }
}