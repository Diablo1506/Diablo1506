using System;
using New.Managers;
using UnityEngine;

namespace New.Controllers
{
    public class PauseMenuUIController : BasePanel
    {
        public override void OnInitialize()
        {
            base.OnInitialize();
        }
        
        public override void OnShow()
        {
            base.OnShow();
        }

        public override void OnHide()
        {
            base.OnHide();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Get.GameManager.ResumeGame();
            }
        }

        public void OnResumeButtonClicked()
        {
            Get.GameManager.ResumeGame();
        }

        public void OnSettingsButtonClicked()
        {
            
        }

        public void OnBackToMenuButtonClicked()
        {
            Get.GameManager.EndGameByQuit();
        }
    }
}