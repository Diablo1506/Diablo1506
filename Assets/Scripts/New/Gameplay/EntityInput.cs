using System;
using CitrioN.SettingsMenuCreator;
using New.SO;
using UnityEngine;
using UnityEngine.InputSystem;

namespace New.Gameplay
{
    public class EntityInput : MonoBehaviour
    {
        [SerializeField]
        private float _speed;
        
        private Vector2 _movementDirection;
        private bool _isWalking;
        private Entity _entity;

        public void Initialize(Entity entity)
        {
            _entity = entity;
        }

        private void Update()
        {
            Move();
        }

        #region INPUT

        public void OnMove(InputValue value)
        {
            if (_entity.IsAI)
                return;
            
            _movementDirection = value.Get<Vector2>();
        }

        public void OnJabLeft()
        {
            OnAttack(PunchID.LEFTJAB);
        }

        public void OnJabRight()
        {
            OnAttack(PunchID.RIGHTJAB);
        }

        public void OnUppercutLeft()
        {
            OnAttack(PunchID.LEFTUPPERCUT);
        }

        public void OnUppercutRight()
        {
            OnAttack(PunchID.RIGHTUPPERCUT);
        }

        public void OnHookLeft()
        {
            OnAttack(PunchID.LEFTHOOK);
        }

        public void OnHookRight()
        {
            OnAttack(PunchID.RIGHTHOOK);
        }

        #endregion
        
        #region ACTIONS

        private void Move()
        {
            int intDirection = Mathf.RoundToInt(_movementDirection.x);
            _isWalking = intDirection != 0;
            
            var finalMovementDirection = new Vector3(_movementDirection.x, 0, 0);
            Debug.Log($"#{GetType()}: MOVE: {finalMovementDirection}");
            var targetPos = transform.position + finalMovementDirection * (_speed * Time.deltaTime);
            _entity.EntityRigidbody.MovePosition(targetPos);
            
            _entity.Walk(intDirection);
        }

        private void OnAttack(PunchID punchID)
        {
            if (_isWalking)
                return;
            
            _entity.OnPunch(punchID);
        }
        #endregion
    }
}