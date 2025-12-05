using Asce.Core;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Entities
{
    [CreateAssetMenu(menuName = "Asce/Stats/Entity Stats", fileName = "Entity Stats")]
    public class SO_EntityStats : ScriptableObject
    {
        [SerializeField, Min(0f)] private float _maxHealth = 100f;
        [SerializeField, Min(0f)] private float _armor = 0f;
        [SerializeField, Min(0f)] private float _speed = 5f;

        [Space]
        [SerializeField]
        private ListObjects<string, CustomValue> _customStats = new((stat) =>
        {
            return stat.Name;
        });


        public float MaxHealth => _maxHealth;
        public float Armor => _armor;
        public float Speed => _speed;

        public ReadOnlyCollection<CustomValue> CustomStats => _customStats.List;
        public float GetCustomStat(string name) => _customStats.Get(name).Value;

    }
}