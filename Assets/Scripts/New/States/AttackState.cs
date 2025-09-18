using New.Controllers;
using UnityEngine;

namespace New.States
{
    public class AttackState : IEnemyState
    {
        public void OnEnter(EnemyController enemyController)
        {
            Debug.Log($"#{GetType()}: Entering Attack State");
        }
        public void OnUpdate(EnemyController enemyController)
        {
            enemyController.CheckAttacks();
        }
        public void OnExit(EnemyController enemyController)
        {
            Debug.Log($"#{GetType()}: Exiting Attack State");
        }
    }
}