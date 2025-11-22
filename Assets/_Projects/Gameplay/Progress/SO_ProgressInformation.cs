using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Progress
{
    [CreateAssetMenu(menuName = "Asce/Progress/Progress Information", fileName = "Progress Information")]
    public class SO_ProgressInformation : ScriptableObject
    {
        [SerializeField] private List<SO_UnlockCondition> _unlockConditions = new ();
        private ReadOnlyCollection<SO_UnlockCondition> _unlockConditionsReadOnly;

        public ReadOnlyCollection<SO_UnlockCondition> UnlockConditions => _unlockConditionsReadOnly ??= _unlockConditions.AsReadOnly();

        public bool AllUnlockConditionsMet()
        {
            foreach (SO_UnlockCondition unlockCondition in _unlockConditions)
            {
                if (!unlockCondition.IsMet()) return false;
            }
            return true;
        }

        public bool AnyUnlockConditionMet()
        {
            foreach (SO_UnlockCondition unlockCondition in _unlockConditions)
            {
                if (unlockCondition.IsMet()) return true;
            }
            return false;
        }
    }
}
