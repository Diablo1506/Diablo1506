using New.Controllers;
using UnityEngine;

namespace New.States
{
    public class IdleState : IEnemyState
    {
        public void OnEnter(EnemyController enemyController)
        {
            Debug.Log($"#{GetType()}: Entering Idle/Walk State");
        }
        public void OnUpdate(EnemyController enemyController)
        {
            enemyController.Walk();
        }
        public void OnExit(EnemyController enemyController)
        {
            Debug.Log($"#{GetType()}: Exiting Idle/Walk State");
        }
    }
}