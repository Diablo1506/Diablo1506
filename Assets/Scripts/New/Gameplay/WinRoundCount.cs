using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace New.Gameplay
{
    public class WinRoundCount : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _playerWinCount;

        [SerializeField]
        private List<GameObject> _enemyWinCount;

        public void AddPlayerWin(int value)
        {
            // value is amount of rounds won. -1 ta pra sa index
            _playerWinCount[value-1].SetActive(true);
        }

        public void AddEnemyWin(int value)
        {
            _enemyWinCount[value-1].SetActive(true);
        }

        public void Reset()
        {
            foreach (var winObject in _playerWinCount)
            {
                winObject.SetActive(false);
            }

            foreach (var winObject in _enemyWinCount)
            {
                winObject.SetActive(false);
            }
        }
    }
}