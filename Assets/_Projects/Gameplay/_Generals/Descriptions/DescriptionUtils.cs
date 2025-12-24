using Asce.Game.Abilities;
using Asce.Game.Entities;
using Asce.Game.Supports;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game
{
    [System.Serializable]
    public static class DescriptionUtils
    {
        public static Dictionary<string, string> GetEntityDescriptionKeys(Entity entity)
        {
            Dictionary<string, string> values = new()
            {
                { "Health", $"{entity.Information.Stats.MaxHealth:0.###}" },
                { "Armor", $"{entity.Information.Stats.Armor:0.###}" },
                { "Speed", $"{entity.Information.Stats.Speed:0.###}" }
            };

            foreach (var custom in entity.Information.Stats.CustomStats)
            {
                values.Add(custom.Name, $"{custom.Value:0.###}");
            }

            return values;
        }

        public static Dictionary<string, string> GetAbilityDescriptionKeys(Ability ability)
        {
            Dictionary<string, string> values = new()
            {
                { "DespawnTime", $"{ability.Information.DespawnTime:0.###}" },
                { "UseRangeRadius", $"{ability.Information.UseRangeRadius:0.###}" },
                { "Cooldown", $"{ability.Information.Cooldown:0.###}" },
                { "ReactiveCooldown", $"{ability.Information.ReactiveCooldown:0.###}" }
            };

            foreach (var custom in ability.Information.Customs)
            {
                values.Add(custom.Name, $"{custom.Value:0.###}");
            }

            if (ability is IControlMachineAbility controlMachineAbility)
            {
                values.Add("Health", $"{controlMachineAbility.Machine.Information.Stats.MaxHealth:0.###}");
                values.Add("Armor", $"{controlMachineAbility.Machine.Information.Stats.Armor:0.###}");
                values.Add("Speed", $"{controlMachineAbility.Machine.Information.Stats.Speed:0.###}");
                foreach (var custom in controlMachineAbility.Machine.Information.Stats.CustomStats)
                {
                    values.Add(custom.Name, $"{custom.Value:0.###}");
                }
            }

            return values;
        }

        public static Dictionary<string, string> GetSupportDescriptionKeys(Support support)
        {
            Dictionary<string, string> values = new()
            {
                { "Cooldown", $"{support.Information.Cooldown:0.###}" },
                { "CooldownOnRecall", $"{support.Information.CooldownOnRecall:0.###}" }
            };

            if (support is IControlMachineAbility controlMachineAbility)
            {
                values.Add("Health", $"{controlMachineAbility.Machine.Information.Stats.MaxHealth:0.###}");
                values.Add("Armor", $"{controlMachineAbility.Machine.Information.Stats.Armor:0.###}");
                values.Add("Speed", $"{controlMachineAbility.Machine.Information.Stats.Speed:0.###}");
                foreach (var custom in controlMachineAbility.Machine.Information.Stats.CustomStats)
                {
                    values.Add(custom.Name, $"{custom.Value:0.###}");
                }
            }

            return values;
        }
    }
}
