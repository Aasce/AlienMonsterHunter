using Asce.Game.Combats;
using Asce.Game.Entities.Characters;
using Asce.Game.Entities.Enemies;
using Asce.Game.Entities.Machines;
using UnityEngine;

namespace Asce.Game.VFXs
{
    public static class PopupTextExtension
    {
        public static Color GetDamageColor(this ITakeDamageable receiver)
        {
            SO_PopupTextColor textColor = PopupTextController.Instance.PopupTextColor;
            if (textColor == null) return Color.white;
            return receiver switch
            {
                Character => textColor.CharacterTakeDamageColor,
                Machine => textColor.MachineTakeDamageColor,
                Enemy => textColor.EnemyTakeDamageColor,
                _ => Color.white,
            };
        }
    }
}