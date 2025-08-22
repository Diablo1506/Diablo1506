using System;
using System.Collections;
using System.Collections.Generic;
using New.Managers;
using New.SO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace New.Gameplay
{
    public class Entity : MonoBehaviour
    {
        [FormerlySerializedAs("_entityMovement")]
        [SerializeField]
        private EntityInput _entityInput;
        
        [SerializeField]
        private Animator _animator;

        [SerializeField]
        private int _maxEnergy;

        [SerializeField]
        private int _energyRestore;

        [SerializeField]
        private int _energyRestoreTime;

        [SerializeField]
        private bool _isPerformingAction;

        [SerializeField, Min(0.1f)]
        private float _comboTimeDuration;

        [SerializeField]
        private List<PunchID> _currentCombo = new List<PunchID>();

        private Coroutine _comboTimerCoroutine;

        private Coroutine _restoreEnergyCoroutine;
        [field: SerializeField] public int EntityHealth { get; set; }
        [field: SerializeField] public int EntityEnergy { get; set; }
        [field: SerializeField] public bool IsAI { get; set; }
        [field: SerializeField] public Rigidbody EntityRigidbody { get; set; }
        [field: SerializeField] public PunchCollider PunchCollider { get; set; }

        public virtual void Initialize()
        {
            _entityInput.Initialize(this);
        }
        
        public void OnRoundStart()
        {
            EntityEnergy = _maxEnergy;
            RestoreEnergy();
        }

        private void RestoreEnergy()
        {
            if (_restoreEnergyCoroutine != null)
                return;

            _restoreEnergyCoroutine = StartCoroutine(RestoreEnergyCoroutine());
            return;

            IEnumerator RestoreEnergyCoroutine()
            {
                while (true)
                {
                    EntityEnergy = Mathf.Min(EntityEnergy + _energyRestore, _maxEnergy);
                    Get.UIManager.GameUIController.PlayerStaminaSliderBar.ChangeValue(EntityEnergy);
                    yield return new WaitForSeconds(_energyRestoreTime);
                }
            }
        }

        private void StopRestoreEnergy()
        {
            StopCoroutine(_restoreEnergyCoroutine);
            _restoreEnergyCoroutine = null;
        }

        private void AddToCombo(PunchID punchID)
        {
            _currentCombo.Add(punchID);

            if (_comboTimerCoroutine != null)
                StopCoroutine(_comboTimerCoroutine);

            _comboTimerCoroutine = StartCoroutine(ComboTimerCoroutine());

            if (_currentCombo.Count >= 3)
            {
                TriggerCombo();
                ResetCombo();
            }
        }

        private IEnumerator ComboTimerCoroutine()
        {
            yield return new WaitForSeconds(_comboTimeDuration);
            ResetCombo();
        }

        private void ResetCombo()
        {
            _currentCombo.Clear();
            _comboTimerCoroutine = null;
        }

        protected virtual void TriggerCombo()
        {
            Debug.Log($"{gameObject.name} triggered a 3-hit combo!");
            // Add reward here (bonus damage, stamina regen, VFX, etc.)
        }

        private IEnumerator PerformingActionCoroutine(int damage)
        {
            _isPerformingAction = true;
            // add here punch collider active 
            PunchCollider.SetPunchColliderStatus(true, damage);
            var animState = _animator.GetCurrentAnimatorStateInfo(0);
            float animLength = animState.length; // increase multiplier in animation to make it go faster, for faster combos upgrade
            Debug.Log($"#{GetType()}: Punch Anim Length: {animLength}");
            yield return new WaitForSeconds(animLength);
            PunchCollider.SetPunchColliderStatus(false, damage);
            // disable punch collider here
            _isPerformingAction = false;
        }


        protected bool CanPunch(PunchID punchID)
        {
            var punchData = Get.PunchDataCollection.GetPunchData(punchID);
            return EntityEnergy >= punchData.EnergyRequired && !_isPerformingAction;
        }
        private void PerformPunch(string punchParameterName, int damage)
        {
            Debug.Log($"#{GetType()}: PERFORMING {punchParameterName}");
            if (_isPerformingAction)
                return;
            
            _animator.SetTrigger(punchParameterName);
            StartCoroutine(PerformingActionCoroutine(damage));
        }

        public virtual void TakeDamage(int damage)
        {
            EntityHealth -= damage;
        }

        public virtual void OnPunch(PunchID punchID)
        {
            if (_animator.IsInTransition(0))
                return;
            
            var punchData = Get.PunchDataCollection.GetPunchData(punchID);
            EntityEnergy -= punchData.EnergyRequired;
            PerformPunch(punchData.PunchParameterName, punchData.Damage);
            AddToCombo(punchID);
        }

        public void Walk(int direction)
        {
            _animator.SetInteger("Walk", direction);
        }
    }
}