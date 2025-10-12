using Asce.Managers.UIs;
using UnityEngine;

namespace Asce.Game.UIs.Panels
{
    public class UIPanel : UIObject
    {
        [SerializeField] protected string _name = string.Empty;

        public string Name => _name;
    }
}