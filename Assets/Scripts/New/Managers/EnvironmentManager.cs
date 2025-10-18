using UnityEngine;

namespace New.Managers
{
    public class EnvironmentManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject _stadiumEnvironment;

        [SerializeField]
        private GameObject _gymEnvironment;

        public void ToggleStadiumEnvironment()
        {
            if (_stadiumEnvironment.activeSelf || _stadiumEnvironment == null)
                return;
            
            _stadiumEnvironment.SetActive(true);
            _gymEnvironment.SetActive(false);
        }

        public void ToggleGymEnvironment()
        {
            if (_gymEnvironment.activeSelf || _gymEnvironment == null)
                return;
            
            _stadiumEnvironment.SetActive(false);
            _gymEnvironment.SetActive(true);
        }
    }
}