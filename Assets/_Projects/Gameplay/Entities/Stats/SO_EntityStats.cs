using UnityEngine;

namespace Asce.Game.Entities
{
    [CreateAssetMenu(menuName = "Asce/Stats/Entity Stats", fileName = "Entity Stats")]
    public class SO_EntityStats : ScriptableObject
    {
        [SerializeField, Min(0f)] private float _maxHealth = 100f;
        [SerializeField, Min(0f)] private float _armor = 0f;
        [SerializeField, Min(0f)] private float _speed = 5f;

        public float MaxHealth => _maxHealth;
        public float Armor => _armor;
        public float Speed => _speed;
    }
}