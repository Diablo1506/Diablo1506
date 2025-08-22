using New.Gameplay;
using New.SO;
using UnityEngine;

namespace New.Controllers
{
    public class PlayerController : Entity
    {
        public override void Initialize()
        {
            base.Initialize();

            IsAI = false;
        }
        public override void OnPunch(PunchID punchID)
        {
            if (!CanPunch(punchID))
                return;
            
            base.OnPunch(punchID);
            
            Get.UIManager.GameUIController.PlayerStaminaSliderBar.ChangeValue(EntityEnergy);
            Debug.Log($"#{GetType()}: Getting punch shits here");
        }
    }
}