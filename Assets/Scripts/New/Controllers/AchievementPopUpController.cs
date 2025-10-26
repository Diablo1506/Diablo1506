using System;
using System.Collections;
using New.SO;
using TMPro;
using UnityEngine;
namespace New.Controllers
{
    public class AchievementPopUpController : MonoBehaviour
    {
        [SerializeField]
        private GameObject _achievementPopUpObject;
        
        [SerializeField]
        private TMP_Text _achievementDescriptionText;
        
        [SerializeField]
        private TMP_Text _achievementTitleText;

        private void Start()
        {
            _achievementPopUpObject.SetActive(false);
        }

        public void ShowAchievementPopUp(AchievementID achievementID)
        {
            var achievementData = Get.AchievementDatabase.GetAchievement(achievementID);
            _achievementPopUpObject.SetActive(true);
            _achievementTitleText.text = achievementData.Title;
            _achievementDescriptionText.text = achievementData.Description;

            StartCoroutine(IEPopUp());
        }

        private IEnumerator IEPopUp()
        {
            yield return new WaitForSeconds(5);
            _achievementPopUpObject.SetActive(false);
        }
    }
}
