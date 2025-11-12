using Asce.Managers.Attributes;
using Asce.Managers.UIs;
using Asce.Managers.Utils;
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
