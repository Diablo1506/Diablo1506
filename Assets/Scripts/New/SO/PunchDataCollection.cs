using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace New.SO
{
    public enum PunchID
    {
        LEFTJAB,
        RIGHTJAB,
        LEFTUPPERCUT,
        RIGHTUPPERCUT,
        LEFTHOOK,
        RIGHTHOOK
    }
    
    [Serializable]
    public class PunchData
    {
        public int Damage;
        public int EnergyRequired;
        public string PunchParameterName;
    }
    
    [CreateAssetMenu(fileName = "PunchDataCollection", menuName = "PunchDataCollection")]
    public class PunchDataCollection : ScriptableObject
    {
        [SerializeField]
        private SerializedDictionary<PunchID, PunchData> _punchDataDict;

        public PunchData GetPunchData(PunchID punchID)
        {
            return _punchDataDict.GetValueOrDefault(punchID);
        }
    }
}