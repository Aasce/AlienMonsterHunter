using UnityEngine;

namespace Asce.Game.Entities
{
    [CreateAssetMenu(menuName = "Asce/Stats/Enemy Stats", fileName = "Enemy Stats")]
    public class SO_EnemyStats : SO_EntityStats
    {
        [SerializeField, Min(0f)] private float _attackDamage = 100f;
        [SerializeField, Min(0f)] private float _attackSpeed = 0f;
        [SerializeField, Min(0f)] private float _attackRange = 5f;

        public float AttackDamage => _attackDamage;
        public float AttackSpeed => _attackSpeed;
        public float AttackRange => _attackRange;
    }
}