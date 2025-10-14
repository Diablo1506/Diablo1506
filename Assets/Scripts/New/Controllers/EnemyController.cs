using System;
using System.Collections;
using New.Gameplay;
using New.Managers;
using New.SO;
using New.States;
using UnityEngine;

namespace New.Controllers
{
    public class EnemyController : Entity
    {
        [SerializeField]
        private float _speed = 2f;

        public IEnemyState CurrentState;
        public IdleState IdleState = new();
        public AttackState AttackState = new();
        public HurtState HurtState = new();

        [SerializeField]
        private float _nextAttackTime;
        [SerializeField]
        private float _attackCooldown;
        [SerializeField]
        private int _comboChance;

        [field: SerializeField] public bool IsTrainingDummy { get; set; }



        public override void Initialize(DifficultyData difficultyData = null)
        {
            base.Initialize(difficultyData);

            if (IsTrainingDummy)
            {
                EntityHealth = 999999;
                return;
            }

            IsAI = true;
            SetDifficultyData(difficultyData);
            ChangeState(IdleState);
        }

        public override void Update()
        {
            if (IsTrainingDummy)
                return;

            base.Update();

            if (IsDead)
                return;

            if (CurrentState != null)
                CurrentState.OnUpdate(this);
        }

        public override void OnPunch(PunchID punchID)
        {
            if (IsDead)
                return;

            if (!CanPunch(punchID))
                return;

            base.OnPunch(punchID);

            // Get.UIManager.GameUIController.EnemyStaminaSliderBar.ChangeValue(EntityEnergy);
            Get.UIManager.GetPanel<GameUIController>(PanelType.GAMEUI).EnemyStaminaSliderBar.ChangeValue(EntityEnergy);
            Debug.Log($"#{GetType()}: Getting punch shits here");
        }

        public override void OnDeath()
        {
            base.OnDeath();


        }

        private void SetDifficultyData(DifficultyData difficultyData)
        {
            // Get.UIManager.GameUIController.EnemyHealthSliderBar.SetHealthSliderMaxValue(difficultyData.Health);
            Get.UIManager.GetPanel<GameUIController>(PanelType.GAMEUI).EnemyHealthSliderBar.SetHealthSliderMaxValue(difficultyData.Health);
            EntityHealth = difficultyData.Health; // make sure health is dynamic in slider
            _attackCooldown = difficultyData.AttackCooldown;
            _comboChance = difficultyData.ComboChance;
            _energyRestore = difficultyData.EnergyRestored;
            _energyRestoreTime = difficultyData.EnergyRestoreTime;

        }

        public void ChangeState(IEnemyState newState)
        {
            if (CurrentState != null)
            {
                CurrentState.OnExit(this);
            }
            CurrentState = newState;
            CurrentState.OnEnter(this);
        }

        public override void TakeDamage(int damage)
        {
            base.TakeDamage(damage);

            Get.UIManager.GetPanel<GameUIController>(PanelType.GAMEUI).EnemyHealthSliderBar.ChangeValue(EntityHealth);

            Get.UpgradeDatabase.UpgradeTokens += 1;
            // Get.UIManager.GameUIController.EnemyHealthSliderBar.ChangeValue(EntityHealth);
            // ChangeState(HurtState);
        }

        public void Walk()
        {
            var enemyPos = transform.position;
            var playerPos = Get.GameManager.PlayerController.transform.position;

            // Decide walk direction
            int intDirection = (enemyPos.x > playerPos.x) ? -1 : 1;

            // Movement
            var moveDir = new Vector3(intDirection, 0, 0);
            var targetPos = enemyPos + moveDir * (_speed * Time.deltaTime);
            EntityRigidbody.MovePosition(targetPos);

            // Animation
            Walk(intDirection);

            // Transition check
            if (Vector3.Distance(enemyPos, playerPos) < 1.5f)
            {
                ChangeState(AttackState);
            }
        }

        public void CheckAttacks()
        {
            var enemyPos = transform.position;
            var playerPos = Get.GameManager.PlayerController.transform.position;

            // Stop moving when in range
            Walk(0);

            float distance = Vector3.Distance(enemyPos, playerPos);

            if (distance > 1.5f)
            {
                ChangeState(IdleState);
                return;
            }

            // Only attack if cooldown is over
            if (Time.time >= _nextAttackTime && !_isPerformingAction)
            {
                int randomChoice = UnityEngine.Random.Range(0, 100); // 0 - 100

                if (randomChoice < _comboChance)
                {
                    // 40% chance: perform a 3-hit combo
                    StartCoroutine(PerformCombo());
                }
                else
                {
                    // 60% chance: single punch
                    var punch = (PunchID)UnityEngine.Random.Range(0, Enum.GetValues(typeof(PunchID)).Length);
                    OnPunch(punch);
                }

                // Reset cooldown
                _nextAttackTime = Time.time + _attackCooldown;
            }
        }

        private IEnumerator PerformCombo()
        {
            Debug.Log($"#{GetType()}: PERFORMING COMBO");
            int punchesInCombo = 3;

            for(int i = 0; i < punchesInCombo; i++)
            {
                var punch = (PunchID)UnityEngine.Random.Range(0, Enum.GetValues(typeof(PunchID)).Length);
                OnPunch(punch);

                // Wait until current punch animation finishes
                yield return new WaitForSeconds(0.3f); // keep it at this
            }

        }
    }
}