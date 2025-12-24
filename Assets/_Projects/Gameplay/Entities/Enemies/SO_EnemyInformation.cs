using Asce.Game.Levelings;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    [CreateAssetMenu(menuName = "Asce/Entities/Enemy Information", fileName = "Enemy Information")]
    public class SO_EnemyInformation : SO_EntityInformation
    {
        [SerializeField] private SO_EnemyDroppedSpoils _droppedSpoils;

        [SerializeField] private SpeciesType _species = SpeciesType.Undefined;
        [SerializeField, Range(0, 10)] private int _dangerLevel = 0;

        public new SO_EnemyLeveling Leveling => base.Leveling as SO_EnemyLeveling;
        public SO_EnemyDroppedSpoils DroppedSpoils => _droppedSpoils;

        public SpeciesType Species => _species;
        public int DangerLevel => _dangerLevel;
    }
}