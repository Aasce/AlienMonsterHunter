using Asce.Core;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Interactions
{
    [CreateAssetMenu(menuName = "Asce/Interactions/All Interactions", fileName = "All Interactions")]
    public class SO_AllInteractiveObjects : ScriptableObject
    {
        [SerializeField] private ListObjects<string, InteractiveObject> _interactiveObjects = new((interactive) =>
        {
            if (interactive == null) return null;
            return interactive.Information.Name;
        });


        public ReadOnlyCollection<InteractiveObject> InteractiveObjects => _interactiveObjects.List;
        public InteractiveObject Get(string name) => _interactiveObjects.Get(name);
    }
}
