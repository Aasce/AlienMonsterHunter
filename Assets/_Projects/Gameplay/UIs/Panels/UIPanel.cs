using Asce.Core.Attributes;
using Asce.Core.UIs;
using UnityEngine;

namespace Asce.Game.UIs.Panels
{
    public abstract class UIPanel : UIComponent
    {
        [SerializeField, Readonly] protected string _name = string.Empty;

        public string Name => _name;

        public virtual void Initialize() { }
    }
}