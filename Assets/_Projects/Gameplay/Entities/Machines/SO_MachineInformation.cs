using Asce.Game.Levelings;
using UnityEngine;

namespace Asce.Game.Entities.Machines
{
    [CreateAssetMenu(menuName = "Asce/Entities/Machine Information", fileName = "Machine Information")]
    public class SO_MachineInformation : SO_EntityInformation
    {
        public new SO_MachineLeveling Leveling => base.Leveling as SO_MachineLeveling;
    }
}
