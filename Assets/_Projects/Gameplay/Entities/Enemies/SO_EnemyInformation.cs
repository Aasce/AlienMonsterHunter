using Asce.Game.Levelings;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    [CreateAssetMenu(menuName = "Asce/Entities/Enemy Information", fileName = "Enemy Information")]
    public class SO_EnemyInformation : SO_EntityInformation
    {
        public new SO_EnemyLeveling Leveling => base.Leveling as SO_EnemyLeveling;
    }
}