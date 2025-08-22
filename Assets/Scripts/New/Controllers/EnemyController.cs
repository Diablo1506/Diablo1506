using New.Gameplay;
using UnityEngine;

namespace New.Controllers
{
    public class EnemyController : Entity
    {
        public override void Initialize()
        {
            base.Initialize();

            IsAI = true;
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);
            
            Get.UIManager.GameUIController.EnemyHealthSliderBar.ChangeValue(EntityHealth);
        }
    }
}