using UnityEngine;

namespace Asce.Game.VFXs
{
    [CreateAssetMenu(menuName = "Asce/VFXs/Popup Color", fileName = "Popup Text Color")]
    public class SO_PopupTextColor : ScriptableObject
    {
        [SerializeField, ColorUsage(showAlpha: true)] private Color _characterTakeDamageColor = Color.red;
        [SerializeField, ColorUsage(showAlpha: true)] private Color _machineTakeDamageColor = Color.yellow;
        [SerializeField, ColorUsage(showAlpha: true)] private Color _enemyTakeDamageColor = Color.white;

        [Space]
        [SerializeField, ColorUsage(showAlpha: true)] private Color _healColor = Color.green;


        public Color CharacterTakeDamageColor => _characterTakeDamageColor;
        public Color MachineTakeDamageColor => _machineTakeDamageColor;
        public Color EnemyTakeDamageColor => _enemyTakeDamageColor;

        public Color HealColor => _healColor;
    }
}