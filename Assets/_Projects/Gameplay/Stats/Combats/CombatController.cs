using Asce.Game.Entities;
using Asce.Managers;
using UnityEngine;

namespace Asce.Game.Stats
{
    public class CombatController : MonoBehaviourSingleton<CombatController>
    {
        [SerializeField] private float _armorReductionFactor = 100f;

        public float ArmorReductionFactor => _armorReductionFactor;


        public void DamageDealing(Entity receiver, float damage)
        {
            if (receiver == null || damage <= 0f) return;
            float finalDamage = this.CalculateDamage(damage, receiver.Stats.Armor.FinalValue);
            receiver.Stats.Health.CurrentValue -= finalDamage;
        }

        public float CalculateDamage(float damage, float armor)
        {
            if (damage <= 0f) return 0f;
            armor = Mathf.Max(0f, armor);
            return damage * (ArmorReductionFactor / (armor + ArmorReductionFactor));
        }
    }
}