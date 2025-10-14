using System.Collections;
using New.Managers;
using UnityEngine;

namespace New.Controllers
{
    public class IntroUIController : BasePanel
    {
        [SerializeField]
        private bool _forceSkip;
        public override void OnInitialize()
        {
            base.OnInitialize();
        }

        public override void OnShow()
        {
            base.OnShow();

            if (_forceSkip)
            {
                Get.UIManager.ShowSingle(PanelType.MAINMENU);
                return;
            }

            Get.AudioManager.PlayBGM(Get.AudioManager.IntroBGMClip, false);
            StartCoroutine(IEStopIntro());
            return;

            IEnumerator IEStopIntro()
            {
                yield return new WaitForSeconds(Get.AudioManager.IntroBGMClip.length);
                Get.UIManager.ShowSingle(PanelType.MAINMENU);
            }
        }

        public override void OnHide()
        {
            base.OnHide();
        }
    }
}