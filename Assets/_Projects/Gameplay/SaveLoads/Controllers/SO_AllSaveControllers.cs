using Asce.Core;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.SaveLoads
{
    [CreateAssetMenu(menuName = "Asce/SaveLoads/All Save Controllers", fileName = "All Save Controllers")]
    public class SO_AllSaveControllers : ScriptableObject
    {
        [SerializeField]
        private ListObjects<string, SaveLoadController> _controllers = new((controller) =>
        {
            if (controller == null) return null;
            return controller.ControllerName;
        });

        public ReadOnlyCollection<SaveLoadController> Controllers => _controllers.List;
        public SaveLoadController Get(string name) => _controllers.Get(name);
    }
}
