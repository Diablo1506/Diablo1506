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

        [SerializeField]
        private CinemachineVirtualCamera _playerCamera;

        [SerializeField]
        private CinemachineVirtualCamera _enemyCamera;

        public void SetCamera(PanelType panelType)
        {
            if (_playerCamera != null && _enemyCamera != null)
            {
                _playerCamera.Priority = 0;
                _enemyCamera.Priority = 0;
            }
            
            foreach (var cameraData in _cameraDataList)
            {
                if (cameraData.PanelType == panelType && cameraData.VirtualCamera != null)
                {
                    cameraData.VirtualCamera.Priority = 10;
                    continue;
                }

                cameraData.VirtualCamera.Priority = 0;
            }
        }

        public void SetCutSceneCamera(bool isPlayerScene)
        {
            foreach (var cameraData in _cameraDataList)
            {
                cameraData.VirtualCamera.Priority = 0;
            }

            if (isPlayerScene)
            {
                _playerCamera.Priority = 10;
                _enemyCamera.Priority = 0;
            }
            else
            {
                _playerCamera.Priority = 0;
                _enemyCamera.Priority = 10;
            }
        }
    }
}