using System;
using New.Controllers;
using UnityEngine;

namespace New.Managers
{
    public class UIManager : MonoBehaviour
    {
        [field: SerializeField] public GameUIController GameUIController { get; private set; }
    }
}