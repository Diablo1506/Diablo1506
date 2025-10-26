using System.Collections.Generic;
using New.Gameplay;
using New.Managers;
using UnityEngine;

namespace New.Controllers
{
    public class AchievementUIController : BasePanel
    {
        [Header("UI References")]
        [SerializeField] private Transform _achievementContentParent;
        [SerializeField] private Achievement _achievementPrefab;

        private readonly List<Achievement> _spawnedAchievements = new List<Achievement>();

        public override void OnInitialize()
        {
            base.OnInitialize();
        }
        
        public override void OnShow()
        {
            base.OnShow();
            PopulateAchievements();
        }

        public override void OnHide()
        {
            base.OnHide();
        }

        private void PopulateAchievements()
        {
            ClearAchievements();

            foreach (var kvp in Get.AchievementDatabase.AchievementDataDict)
            {
                var id = kvp.Key;
                var data = kvp.Value;

                if (!data.HasAchieved)
                    continue;

                var achievementInstance = Instantiate(_achievementPrefab, _achievementContentParent);
                achievementInstance.SetTexts(data.Description, data.Title);
                _spawnedAchievements.Add(achievementInstance);
            }
        }

        private void ClearAchievements()
        {
            foreach (var achievement in _spawnedAchievements)
            {
                if (achievement != null)
                    Destroy(achievement.gameObject);
            }
            _spawnedAchievements.Clear();
        }

        public void OnBackButtonClicked()
        {
            ClearAchievements();
            Get.UIManager.ShowSingle(PanelType.PREFIGHT);
        }
    }
}
