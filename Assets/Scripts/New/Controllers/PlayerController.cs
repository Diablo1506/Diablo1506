using New.Gameplay;
using New.Managers;
using New.SO;
using Sirenix.OdinInspector;
using UnityEngine;

namespace New.Controllers
{
    public class PlayerController : Entity
    {
        [Title("Player Controller Class")]
        public override void Initialize(DifficultyData difficultyData = null)
        {
            base.Initialize(difficultyData);

            IsAI = false;
        }
        
        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);

            // Get.UIManager.GameUIController.PlayerHealthSliderBar.ChangeValue(EntityHealth);
            Get.UIManager.GetPanel<GameUIController>(PanelType.GAMEUI).PlayerHealthSliderBar.ChangeValue(EntityHealth);
            // ChangeState(HurtState);
        }
        
        public override void OnPunch(PunchID punchID)
        {
            if (IsDead)
                return;
            
            if (!CanPunch(punchID))
                return;
            
            base.OnPunch(punchID);
            
            // Get.UIManager.GameUIController.PlayerStaminaSliderBar.ChangeValue(EntityEnergy);
            Get.UIManager.GetPanel<GameUIController>(PanelType.GAMEUI).PlayerStaminaSliderBar.ChangeValue(EntityEnergy);
            Debug.Log($"#{GetType()}: Getting punch shits here");
        }

        public override void OnDeath()
        {
            base.OnDeath();
            
            
        }
    }
}