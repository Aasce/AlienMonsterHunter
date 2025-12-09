using Asce.Core;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Maps
{
    [System.Serializable]
    public class MapLevelGameStateCondition
    {
        [SerializeField] private string _conditionName;

        [SerializeField]
        private ListObjects<string, CustomValue> _customValues = new((custom) =>
        {
            return custom.Name;
        });

        public ReadOnlyCollection<CustomValue> Customs => _customValues.List;
        public float Get(string name)
        {
            if (_customValues.TryGet(name, out CustomValue customValue))
            {
                return customValue.Value;
            }

            Debug.LogWarning($"Custom Value \"{name}\" not found.");
            return 0f;
        }

        public string ConditionName => _conditionName;
    }
}
