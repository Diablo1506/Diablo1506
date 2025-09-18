using New.Controllers;

namespace New.States
{
    public interface IEnemyState
    {
        void OnEnter(EnemyController enemyController);
        void OnUpdate(EnemyController enemyController);
        void OnExit(EnemyController enemyController);
    }
}