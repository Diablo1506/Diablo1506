using System.Collections;
using New.Managers;
using New.SO;
using TMPro;
using UnityEngine;

namespace New.Controllers
{
    public class CutSceneUIController : BasePanel
    {
        [SerializeField]
        private TMP_Text _cutSceneText;

        [SerializeField]
        private int _dialogueLength = 5;

        private CutSceneData _cutSceneData;

        public override void OnInitialize()
        {
            base.OnInitialize();
        }

        public override void OnShow()
        {
            base.OnShow();

            _cutSceneData = Get.CutSceneDatabase.GetCutSceneData(Get.DifficultyDatabase.CurrentDifficultyID);

            StartCutScene();
        }

        public override void OnHide()
        {
            base.OnHide();
        }

        private void StartCutScene()
        {
            StartCoroutine(IECutScene());
        }

        private IEnumerator IECutScene()
        {
            int i = 0;
            while (i < _cutSceneData.PlayerDialogueList.Count)
            {
                string playerLine = _cutSceneData.PlayerDialogueList[i];
                string enemyLine = _cutSceneData.EnemyDialogueList.Count > i
                    ? _cutSceneData.EnemyDialogueList[i]
                    : string.Empty;

                // Player speaks
                if (!string.IsNullOrWhiteSpace(playerLine))
                {
                    _cutSceneText.text = playerLine;
                    Get.CameraManager.SetCutSceneCamera(true);
                    yield return new WaitForSeconds(_dialogueLength);
                }

                // Enemy responds
                if (!string.IsNullOrWhiteSpace(enemyLine))
                {
                    _cutSceneText.text = enemyLine;
                    Get.CameraManager.SetCutSceneCamera(false);
                    yield return new WaitForSeconds(_dialogueLength);
                }

                i++;
                yield return null;
            }

            // Fade out text (optional polish)
            _cutSceneText.text = string.Empty;

            // Transition to game
            Get.UIManager.ShowSingle(PanelType.GAMEUI);
            Get.UIManager.GetPanel<GameUIController>(PanelType.GAMEUI).StartCountDown();
        }
    }
}