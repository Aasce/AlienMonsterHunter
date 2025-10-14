using Asce.Game.Entities.Characters;
using Asce.Game.Entities.Machines;
using Asce.Game.VFXs;
using Asce.Managers;
using UnityEngine;

namespace Asce.Game.Stats
{
    public class CombatController : MonoBehaviourSingleton<CombatController>
    {
        [SerializeField] private float _armorReductionFactor = 100f;

        public float ArmorReductionFactor => _armorReductionFactor;


        public void DamageDealing(ITakeDamageable receiver, float damage)
        {
            if (receiver == null || damage <= 0f) return;
            float finalDamage = this.CalculateDamage(damage, receiver.Armor.FinalValue);
            receiver.Health.CurrentValue -= finalDamage;
            receiver.TakeDamageCallback(finalDamage);
            this.ShowDamageText(receiver, finalDamage);
        }

        public void Healing(ITakeDamageable receiver, float healAmount)
        {
            if (receiver == null || healAmount <= 0f) return;
            receiver.Health.CurrentValue += healAmount;
            this.ShowHealText(receiver, healAmount);
        }


        public float CalculateDamage(float damage, float armor)
        {
            if (damage <= 0f) return 0f;
            armor = Mathf.Max(0f, armor);
            return damage * (ArmorReductionFactor / (armor + ArmorReductionFactor));
        }

        private void ShowDamageText(ITakeDamageable receiver, float damage)
        {
            float size = Mathf.Lerp(100f, 200f, Mathf.InverseLerp(10f, 100f, damage));

            PopupTextData data = new()
            {
                Text = damage.ToString("0"),
                Color = receiver switch
                {
                    Character => Color.red,
                    Machine => Color.yellow,
                    MonoBehaviour => Color.white,
                    _ => Color.white,
                },
                Size = size
            };

            PopupTextController.Instance.EnqueuePopupText((receiver as MonoBehaviour).transform, data);
        }

        private void ShowHealText(ITakeDamageable receiver, float healAmount)
        {
            float size = Mathf.Lerp(100f, 200f, Mathf.InverseLerp(10f, 100f, healAmount));

            PopupTextData data = new()
            {
                Text = healAmount.ToString("0"),
                Color = Color.green,
                Size = size
            };

            PopupTextController.Instance.EnqueuePopupText((receiver as MonoBehaviour).transform, data);
        }

    }
}