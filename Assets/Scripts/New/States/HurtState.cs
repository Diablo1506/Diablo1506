using New.Controllers;
using UnityEngine;

namespace New.States
{
    public class HurtState : IEnemyState
    {
        public void OnEnter(EnemyController enemyController)
        {
            Debug.Log($"#{GetType()}: Entering Hurt State");
        }
        public void OnUpdate(EnemyController enemyController)
        {
        }
        public void OnExit(EnemyController enemyController)
        {
            Debug.Log($"#{GetType()}: Exiting Hurt State");
            // enemyController.ChangeState(enemyController.IdleState);
        }
    }
}