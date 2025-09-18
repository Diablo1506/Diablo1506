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
        private Entity _entity;

        public void Initialize(Entity entity)
        {
            _entity = entity;
            _collider.enabled = false;
        }

        public void SetPunchColliderStatus(bool isActive, int damageToGive)
        {
            _collider.enabled = isActive;
            DamageToGive = damageToGive;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Entity entity) && _entity != entity)
            {
                entity.TakeDamage(DamageToGive);
            }
        }
    }
}