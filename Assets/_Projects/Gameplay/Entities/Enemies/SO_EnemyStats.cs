using UnityEngine;

namespace Asce.Game.Entities
{
    [CreateAssetMenu(menuName = "Asce/Stats/Enemy Stats", fileName = "Enemy Stats")]
    public class SO_EnemyStats : SO_EntityStats
    {
        public float AttackDamage => this.GetCustomStat("AttackDamage");
        public float AttackSpeed => this.GetCustomStat("AttackSpeed");
        public float AttackRange => this.GetCustomStat("AttackRange");

        public float ViewRange => this.GetCustomStat("ViewRange");
    }
}