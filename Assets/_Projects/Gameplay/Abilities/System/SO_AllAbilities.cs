using Asce.Managers;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Abilities
{
    [CreateAssetMenu(menuName = "Asce/Abilities/All Abilities", fileName = "All Abilities")]
    public class SO_AllAbilities : ScriptableObject
    {
        [SerializeField] private ListObjects<string, Ability> _abilities = new((ability) =>
        {
            if (ability == null || ability.Information == null) return null;
            return ability.Information.Name;
        });

        public ReadOnlyCollection<Ability> Abilities => _abilities.List;
        public Ability Get(string name) => _abilities.Get(name);
    }
}