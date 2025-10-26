using TMPro;
using UnityEngine;
namespace New.Gameplay
{
    public class Achievement : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text _achievementDescriptionText;
        
        [SerializeField]
        private TMP_Text _achievementTitleText;

        public void SetTexts(string achievementDescription, string achievementTitle)
        {
            _achievementTitleText.text = achievementTitle;
            _achievementDescriptionText.text = achievementDescription;
        }
    }
}
