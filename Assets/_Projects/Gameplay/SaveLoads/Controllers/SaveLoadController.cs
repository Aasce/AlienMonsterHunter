using Asce.Core;
using Asce.Core.Attributes;
using System;
using UnityEngine;

namespace Asce.SaveLoads
{
    public abstract class SaveLoadController : ControllerComponent
    {
        [SerializeField, Readonly] protected string _name = string.Empty;

        public override string ControllerName => _name;

        protected override void Reset()
        {
            base.Reset();
            this.LoadName();
        }

        protected abstract void LoadName();

        public virtual void Load() { }
    }
}
