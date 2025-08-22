using System;
using New.Controllers;
using UnityEngine;

namespace New.Gameplay
{
    public class PunchCollider : MonoBehaviour
    {
        [SerializeField]
        private Collider _collider;

        public int DamageToGive;

        public void SetPunchColliderStatus(bool isActive, int damageToGive)
        {
            _collider.enabled = isActive;
            DamageToGive = damageToGive;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out EnemyController enemyController))
            {
                enemyController.TakeDamage(DamageToGive);
            }
        }
    }
}