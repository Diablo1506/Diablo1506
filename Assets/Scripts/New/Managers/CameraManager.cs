using System;
using System.Collections.Generic;
using Cinemachine;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace New.Managers
{
    [Serializable]
    public class CameraData
    {
        public PanelType PanelType;
        public CinemachineVirtualCamera VirtualCamera;
    }
    public class CameraManager : MonoBehaviour
    {
        [SerializeField]
        private List<CameraData> _cameraDataList;

        public void SetCamera(PanelType panelType)
        {
            foreach (var cameraData in _cameraDataList)
            {
                if (cameraData.PanelType == panelType)
                {
                    cameraData.VirtualCamera.Priority = 10;
                    continue;
                }

                cameraData.VirtualCamera.Priority = 0;
            }
        }
    }
}