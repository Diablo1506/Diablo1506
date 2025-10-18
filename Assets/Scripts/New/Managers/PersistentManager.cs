using New.SO;
using UnityEngine;

namespace New.Managers
{
    public class PersistentManager : MonoBehaviour
    {
        [field: SerializeField] public CameraManager CameraManager { get; private set; }
        [field: SerializeField] public UIManager UIManager { get; set; }
        [field: SerializeField] public GameManager GameManager { get; set; }
        [field: SerializeField] public PunchDataCollection PunchDataCollection { get; private set; }
        [field: SerializeField] public GlobalDataCollection GlobalDataCollection { get; private set; }
        [field: SerializeField] public DifficultyDatabase DifficultyDatabase { get; private set; }
        [field: SerializeField] public UpgradeDatabase UpgradeDatabase { get; private set; }
        [field: SerializeField] public PlayerPrefManager PlayerPrefManager { get; private set; }
        [field: SerializeField] public EnemyDatabase EnemyDatabase { get; private set; }
        [field: SerializeField] public CutSceneDatabase CutSceneDatabase { get; private set; }
        [field: SerializeField] public AudioManager AudioManager { get; private set; }
        [field: SerializeField] public EnvironmentManager EnvironmentManager { get; private set; }
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