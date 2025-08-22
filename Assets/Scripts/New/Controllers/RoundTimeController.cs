using System.Collections;
using New.Managers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace New.Controllers
{
    public class RoundTimeController : MonoBehaviour
    {
        [SerializeField]
        private int _roundTime;

        private Coroutine _roundTimeCoroutine;

        private GameManager _gameManager;

        public void Initialize(GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        private void StartTime()
        {
            if (_roundTimeCoroutine != null)
                return;
            
            _roundTimeCoroutine = StartCoroutine(StartTimeCoroutine());
                return;

            IEnumerator StartTimeCoroutine()
            {
                while (_roundTime > 0)
                {
                    yield return new WaitForSeconds(1);
                    _roundTime--;
                    Get.UIManager.GameUIController.SetTime(_roundTime);
                }

                StopRound();
            }
        }

        [Button]
        public void StartRound()
        {
            _roundTime = _gameManager.CurrentRoundData.RoundTime;
            StartTime();
        }

        public void StopRound()
        {
            StopCoroutine(_roundTimeCoroutine);
            _roundTimeCoroutine = null;
        }
    }
}