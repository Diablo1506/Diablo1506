using New.SO;
using UnityEngine;

namespace New.Managers
{
    public class PersistentManager : MonoBehaviour
    {
        [field: SerializeField] public UIManager UIManager { get; set; }
        [field: SerializeField] public GameManager GameManager { get; set; }
        [field: SerializeField] public PunchDataCollection PunchDataCollection { get; private set; }
        [field: SerializeField] public GlobalDataCollection GlobalDataCollection { get; private set; }
        public static PersistentManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                return;
            }
            
            Destroy(gameObject);
        }
    }
}