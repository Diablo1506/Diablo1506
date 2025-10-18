using System;
using System.Collections.Generic;
using New.Controllers;
using Sirenix.OdinInspector;
using UnityEngine;

namespace New.Managers
{
    public enum PanelType
    {
        MAINMENU,
        PREFIGHT,
        GAMEUI,
        TRAINUI,
        ENDUI,
        PAUSE,
        CUTSCENE,
        SETTINGS,
        INTRO,
        INSTRUCTIONS,
        LOAD,
        NONE
    }
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private List<BasePanel> _panelList;

        [SerializeField]
        private PanelType _initialPanel = PanelType.INTRO;

        private PanelType _currentPanel = PanelType.INTRO;
        private PanelType _lastPanel = PanelType.INTRO;
        
        private Stack<BasePanel> _panelStack = new Stack<BasePanel>();
        private Dictionary<PanelType, BasePanel> _panels = new Dictionary<PanelType, BasePanel>();

        private void Awake()
        {
            foreach (var panel in _panelList)
            {
                _panels[panel.PanelType] = panel;
                panel.OnInitialize();
                panel.OnHide(); // start hidden
            }
        }

        private void Start()
        {
            ShowSingle(_initialPanel);
        }

        public void ShowSingle(PanelType type)
        {
            _currentPanel = type;
            
            if (_currentPanel is PanelType.MAINMENU or PanelType.PREFIGHT or PanelType.SETTINGS)
            {
                if (_lastPanel is not (PanelType.MAINMENU or PanelType.PREFIGHT or PanelType.SETTINGS))
                {
                    Get.AudioManager.PlayBGM(Get.AudioManager.MenuBGMClip);
                }
            }
            
            _lastPanel = _currentPanel;
            
            foreach (var p in _panels.Values)
                p.OnHide();

            var panel = _panels[type];
            panel.OnShow();

            _panelStack.Clear();
            _panelStack.Push(panel);
        }

        /// <summary>Push a panel on top of the current one (like modal/popup)</summary>
        [Button]
        public void Push(PanelType type)
        {
            if (_panelStack.Count > 0)
                _panelStack.Peek().OnHide();

            var panel = _panels[type];
            panel.OnShow();
            _panelStack.Push(panel);
        }

        /// <summary>Pop the current panel and return to the previous one</summary>
        public void Pop()
        {
            if (_panelStack.Count > 0)
            {
                var panel = _panelStack.Pop();
                panel.OnHide();
            }

            if (_panelStack.Count > 0)
            {
                _panelStack.Peek().OnShow();
            }
        }

        public T GetPanel<T>(PanelType type) where T : BasePanel
        {
            if (_panels.TryGetValue(type, out var panel))
                return panel as T;

            Debug.LogError($"Panel of type {type} not found or not of {typeof(T)}");
            return null;
        }
    }
}