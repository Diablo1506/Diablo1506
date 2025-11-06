using System;
using System.Collections;
using System.Collections.Generic;
using New.Controllers;
using New.Managers;
using New.SO;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace New.Gameplay
{
    public class Entity : MonoBehaviour
    {
        [Title("Entity Base Class")]
        [SerializeField]
        private EntityInput _entityInput;

        [SerializeField]
        protected Animator _animator;

        [SerializeField]
        private int _maxEnergy;

        [SerializeField]
        protected int _energyRestore;

        [SerializeField]
        protected int _energyRestoreTime;

        [SerializeField]
        protected bool _isPerformingAction;

        [SerializeField, Min(0.1f)]
        private float _comboTimeDuration;

        [SerializeField]
        private List<PunchID> _currentCombo = new List<PunchID>();

        private bool _isGuarding;

        private Coroutine _comboTimerCoroutine;

        private Coroutine _restoreEnergyCoroutine;
        private bool _isFacingRight;
        [field: SerializeField] public int EntityHealth { get; set; }
        [field: SerializeField] public int EntityEnergy { get; set; }
        [field: SerializeField] public bool IsAI { get; set; }
        [field: SerializeField] public Rigidbody EntityRigidbody { get; set; }
        [field: SerializeField] public PunchCollider PunchCollider { get; set; }
        [field: SerializeField] public float RotateSpeed { get; set; } = 50f; // min should be 50
        [field: SerializeField] public bool IsDead { get; set; }
        [field: SerializeField] public Transform PunchVFXSpawnPoint { get; set; }

        public virtual void Initialize(DifficultyData difficultyData = null)
        {
            if (_entityInput != null)
            {
                _entityInput.Initialize(this);
            }

            if (PunchCollider != null)
            {
                PunchCollider.Initialize(this);
            }
        }

        public void Uninitialize()
        {
            _entityInput.Initialize(null);
            PunchCollider.Initialize(null);
        }

        public virtual void Update()
        {
            if (Get.GameManager.IsRoundOver)
                return;

            if (IsDead)
                return;

            FaceTarget();
        }

        public void OnRoundStart()
        {
            EntityEnergy = _maxEnergy;
            RestoreEnergy();
        }

        private void FaceTarget()
        {
            Transform target = this == Get.GameManager.PlayerController
                ? Get.GameManager.EnemyController.transform
                : Get.GameManager.PlayerController.transform;

            if (target == null) return;

            _isFacingRight = transform.position.x < target.position.x;

            Quaternion targetRotation = new Quaternion();

            if (this == Get.GameManager.PlayerController)
            {
                targetRotation = _isFacingRight
                    ? Quaternion.Euler(0, 90, 0) // Player faces right
                    : Quaternion.Euler(0, -90, 0); // Player faces left
            }
            else // Enemy
            {
                targetRotation = _isFacingRight
                    ? Quaternion.Euler(0, 90, 0) // Enemy faces left (mirror logic)
                    : Quaternion.Euler(0, -90, 0); // Enemy faces right
            }


            // Smoothly rotate towards target
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * RotateSpeed
            );
        }

        private void RestoreEnergy()
        {
            if (_restoreEnergyCoroutine != null)
                return;

            _restoreEnergyCoroutine = StartCoroutine(RestoreEnergyCoroutine());

            IEnumerator RestoreEnergyCoroutine()
            {
                while (true)
                {
                    if (EntityEnergy < _maxEnergy)
                    {
                        EntityEnergy = Mathf.Min(EntityEnergy + _energyRestore, _maxEnergy);

                        if (!IsAI)
                            Get.UIManager.GetPanel<GameUIController>(PanelType.GAMEUI).PlayerStaminaSliderBar.ChangeValue(EntityEnergy);
                        else
                            Get.UIManager.GetPanel<GameUIController>(PanelType.GAMEUI).EnemyStaminaSliderBar.ChangeValue(EntityEnergy);
                    }

                    yield return new WaitForSeconds(_energyRestoreTime);
                }
            }
        }

        private void StopRestoreEnergy()
        {
            StopCoroutine(_restoreEnergyCoroutine);
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
            // float animLength = animState.length; // increase multiplier in animation to make it go faster, for faster combos upgrade
            // Debug.Log($"#{GetType()}: Punch Anim Length: {animLength}");
            yield return new WaitForSeconds(.1f);
            PunchCollider.SetPunchColliderStatus(false, damage);
            // disable punch collider here
            _isPerformingAction = false;
        }


        protected bool CanPunch(PunchID punchID)
        {
            if (Get.GameManager.IsRoundOver)
            {
                return false;
            }

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
            Get.AudioManager.PlayPunch();

            if (_isGuarding)
            {
                damage = Mathf.RoundToInt((float)damage / 4f);
            }
            else
            {
                _animator.SetTrigger("GetHit");
                Get.ParticleManager.PlayPunchVFX(PunchVFXSpawnPoint.position);
            }

            EntityHealth -= damage;

            if (EntityHealth <= 0)
            {
                OnDeath();
            }
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
            if (direction == 0)
            {
                _animator.SetInteger("Walk", 0);
                return;
            }

            // If facing right, direction stays the same.
            // If facing left, invert it.
            int adjustedDirection = _isFacingRight ? direction : -direction;

            _animator.SetInteger("Walk", adjustedDirection);
        }


        public virtual void OnDeath()
        {
            StartCoroutine(IEDeathSound());

            IsDead = true;

            StopRestoreEnergy();
            Get.GameManager.EndRound(this);

            return;

            IEnumerator IEDeathSound()
            {
                yield return new WaitForSeconds(1f);
                Get.AudioManager.PlaySFX(Get.AudioManager.DeathClip);
            }
        }

        public void SetGuard(bool isGuard)
        {
            _animator.SetBool("Guard", isGuard);
            _isGuarding = isGuard;
        }
    }
}