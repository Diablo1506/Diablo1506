using UnityEngine;
using UnityEngine.UI;

namespace New.Gameplay
{
    public class SliderBarUI : MonoBehaviour
    {
        [SerializeField]
        private Slider _healthSlider;

        public void SetHealthSliderMaxValue(int value)
        {
            _healthSlider.maxValue = value;
        }
        
        public void ChangeValue(int health)
        {
            _healthSlider.value = health;
        }
    }
}