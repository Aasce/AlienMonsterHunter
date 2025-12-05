using Asce.Game.VFXs;
using Asce.Core;
using UnityEngine;

namespace Asce.Game.Combats
{
    public class CombatController : MonoBehaviourSingleton<CombatController>
    {
        [SerializeField] private float _armorReductionFactor = 100f;

        public float ArmorReductionFactor => _armorReductionFactor;

        public void DamageDealing(DamageContainer container)
        {
            if (container == null) return;
            if (container.Receiver == null) return;

            if (container.Sender != null) container.Sender.BeforeSendDamageCallback(container);
            container.Receiver.BeforeTakeDamageCallback(container);

            float finalArmor = this.CalculateArmor(container.Receiver.Armor.FinalValue, container.Penetration);
            float finalDamage = this.CalculateDamage(container.Damage, finalArmor);
            container.FinalDamage = finalDamage;
            container.Receiver.Health.CurrentValue -= finalDamage;
            if (container.Receiver.Health.CurrentValue <= 0f)
            {
                container.IsKill = true;
            }
            else container.IsKill = false;

            container.Receiver.AfterTakeDamageCallback(container);
            if (container.Sender != null) container.Sender.AfterSendDamageCallback(container);
            if (container.IsKill) 
            {
                container.Receiver.DeadCallback(container);
                if (container.Sender != null) container.Sender.KillCallback(container);
            }
            this.ShowDamageText(container.Receiver, finalDamage);
        }

        public void Healing(IHealable receiver, float healAmount)
        {
            if (receiver == null || healAmount <= 0f) return;
            receiver.Health.CurrentValue += healAmount;
            receiver.HealingCallback(healAmount);
            this.ShowHealText(receiver, healAmount);
        }

        public void Killing(ISendDamageable killer, ITakeDamageable victim)
        {
            if (victim == null) return;
            DamageContainer container = new DamageContainer(killer, victim)
            {
                Damage = victim.Health.FinalValue,
                FinalDamage = victim.Health.FinalValue,
                DamageType = DamageType.TrueDamage,
                IsKill = true,
            };
            victim.DeadCallback(container);
            if (killer != null) killer.KillCallback(container);
        }

        public float CalculateArmor(float armor, float penetration)
        {
            if (penetration <= 0f) return armor;
            return Mathf.Max(0, armor - penetration);
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
                Color = receiver.GetDamageColor(),
                Size = size
            };

            PopupTextController.Instance.EnqueuePopupText((receiver as MonoBehaviour).transform, data);
        }

        private void ShowHealText(IHealable receiver, float healAmount)
        {
            float size = Mathf.Lerp(100f, 200f, Mathf.InverseLerp(10f, 100f, healAmount));

            PopupTextData data = new()
            {
                Text = healAmount.ToString("0"),
                Color = PopupTextController.Instance.PopupTextColor.HealColor,
                Size = size
            };

            PopupTextController.Instance.EnqueuePopupText((receiver as MonoBehaviour).transform, data);
        }

    }
}