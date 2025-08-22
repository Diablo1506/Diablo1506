using New.Gameplay;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

namespace New.Controllers
{
    public class GameUIController : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _timeText;
        
        [field: Title("Player"), Space(10)]
        [field: SerializeField] public SliderBarUI PlayerHealthSliderBar { get; set; }
        [field: SerializeField] public SliderBarUI PlayerStaminaSliderBar { get; set; }
        
        [field: Title("Enemy"), Space(10)]
        [field: SerializeField] public SliderBarUI EnemyHealthSliderBar { get; set; }
        [field: SerializeField] public SliderBarUI EnemyStaminaSliderBar { get; set; }

        public void SetTime(int time)
        {
            _timeText.text = time.ToString();
        }
    }
}