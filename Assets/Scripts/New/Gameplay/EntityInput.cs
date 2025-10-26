using System;
using New.Controllers;
using New.Managers;
using New.SO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace New.Gameplay
{
    public class EntityInput : MonoBehaviour
    {
        [SerializeField]
        private float _speed;

        [SerializeField]
        private PlayerInput _playerInput;

        private InputAction _guardAction;

        private Vector2 _movementDirection;
        private bool _isWalking;
        private bool _isGuarding;
        private Entity _entity;

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _playerInput.enabled = true;

            _guardAction = _playerInput.actions.FindActionMap("Player").FindAction("Guard");

            _guardAction.started += OnGuardStarted;
            _guardAction.canceled += OnGuardEnded;
        }

        private void Update()
        {
            if (Get.GameManager.IsRoundOver)
                return;
            
            if (_entity == null)
                return;

            if (Get.GameManager.PlayerController == null)
                return;
            
            if (Get.GameManager.PlayerController.IsDead)
                return;

            Move();
        }

        #region INPUT

        public void OnMove(InputValue value)
        {
            if (_entity.IsAI || _isGuarding)
                return;

            _movementDirection = value.Get<Vector2>();
            Get.UIManager.GetPanel<GameUIController>(PanelType.GAMEUI).CompleteTutorialStep(TutorialID.MOVE);
        }

        public void OnJabLeft()
        {
            OnAttack(PunchID.LEFTJAB);
            Get.UIManager.GetPanel<GameUIController>(PanelType.GAMEUI).CompleteTutorialStep(TutorialID.JAB);
        }

        public void OnJabRight()
        {
            OnAttack(PunchID.RIGHTJAB);
            Get.UIManager.GetPanel<GameUIController>(PanelType.GAMEUI).CompleteTutorialStep(TutorialID.JAB);
        }

        public void OnUppercutLeft()
        {
            OnAttack(PunchID.LEFTUPPERCUT);
            Get.UIManager.GetPanel<GameUIController>(PanelType.GAMEUI).CompleteTutorialStep(TutorialID.UPPERCUT);
        }

        public void OnUppercutRight()
        {
            OnAttack(PunchID.RIGHTUPPERCUT);
            Get.UIManager.GetPanel<GameUIController>(PanelType.GAMEUI).CompleteTutorialStep(TutorialID.UPPERCUT);
        }

        public void OnHookLeft()
        {
            OnAttack(PunchID.LEFTHOOK);
            Get.UIManager.GetPanel<GameUIController>(PanelType.GAMEUI).CompleteTutorialStep(TutorialID.HOOK);
        }

        public void OnHookRight()
        {
            OnAttack(PunchID.RIGHTHOOK);
            Get.UIManager.GetPanel<GameUIController>(PanelType.GAMEUI).CompleteTutorialStep(TutorialID.HOOK);
        }

        private void OnGuardStarted(InputAction.CallbackContext callbackContext)
        {
            if (_isWalking)
                return;
            
            OnSetGuard(true);
            Get.UIManager.GetPanel<GameUIController>(PanelType.GAMEUI).CompleteTutorialStep(TutorialID.BLOCK);
        }

        private void OnGuardEnded(InputAction.CallbackContext callbackContext)
        {
            if (_isWalking)
                return;
            
            OnSetGuard(false);
        }

        #endregion

        #region ACTIONS

        private void Move()
        {
            int intDirection = Mathf.RoundToInt(_movementDirection.x);
            _isWalking = intDirection != 0;

            // Calculate target position
            Vector3 finalMovementDirection = new Vector3(_movementDirection.x, 0, 0);
            Vector3 targetPos = transform.position + finalMovementDirection * (_speed * Time.deltaTime);

            // Clamp to boundary limits
            if (Get.GameManager.LeftBoundary != null && Get.GameManager.RightBoundary != null)
            {
                float leftLimit = Get.GameManager.LeftBoundary.position.x;
                float rightLimit = Get.GameManager.RightBoundary.position.x;
                targetPos.x = Mathf.Clamp(targetPos.x, leftLimit, rightLimit);
            }

            _entity.EntityRigidbody.MovePosition(targetPos);
            _entity.Walk(intDirection);
        }

        private void OnAttack(PunchID punchID)
        {
            if (_isWalking)
                return;

            _entity.OnPunch(punchID);
        }

        private void OnSetGuard(bool isGuard)
        {
            _isGuarding = isGuard;
            _entity.SetGuard(isGuard);
        }

        #endregion
    }
}