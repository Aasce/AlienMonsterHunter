using Asce.Core.Attributes;
using Asce.Core.UIs;
using Asce.Core.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.Entities.Machines
{
    public class MachineUI : EntityUI
    {
        public new Machine Owner
        {
            get => base.Owner as Machine;
            set => base.Owner = value;
        }
    }
}
