using Asce.Managers.Attributes;
using System;
using UnityEngine;

namespace Asce.SaveLoads
{
    public abstract class SaveLoadController : MonoBehaviour
    {
        [SerializeField, Readonly] protected string _name = string.Empty;

        public string Name => _name;

        protected virtual void Reset()
        {
            this.LoadName();
        }

        protected abstract void LoadName();
    }
}
