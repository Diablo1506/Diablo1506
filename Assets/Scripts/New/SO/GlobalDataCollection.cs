using UnityEngine;

namespace New.SO
{
    [CreateAssetMenu(menuName = "GlobalDataCollection", fileName = "GlobalDataCollection")]
    public class GlobalDataCollection : ScriptableObject
    {
        [field: SerializeField] public int EnergyAmount = 100;
        [field: SerializeField] public int EnergyRestored = 5;
        [field: SerializeField] public int MaxHealth = 100;
        [field: SerializeField] public int PunchSpeed = 1;
    }
}