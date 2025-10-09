using Asce.Managers;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Effects
{
    [CreateAssetMenu(menuName = "Asce/Effects/All Effects", fileName = "All Effects")]
    public class SO_AllEffects : ScriptableObject
    {
        [SerializeField] private ListObjects<string, Effect> _effects = new((effect) =>
        {
            if (effect == null) return null;
            if (effect.Information == null) return null;
            return effect.Information.Name;
        });

        public ReadOnlyCollection<Effect> Effects => _effects.List;
        public Effect Get(string name) => _effects.Get(name);
    }
}
