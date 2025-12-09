using Asce.Core;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Managers
{
    [CreateAssetMenu(menuName = "Asce/Managers/All Win Lose Conditions", fileName = "All Win Lose Conditions")]
    public class SO_AllWinLoseConditions : ScriptableObject
    {
        [SerializeField]
        private ListObjects<string, GameStateCondition> _winLoseCondtions = new(condition =>
        {
            if (condition == null) return null;
            return condition.ConditionName;
        });
    
        public ReadOnlyCollection<GameStateCondition> Conditions => _winLoseCondtions.List;
        public GameStateCondition Get(string conditionName) => _winLoseCondtions.Get(conditionName);
    }
}
