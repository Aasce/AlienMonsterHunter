using Asce.Managers;
using UnityEngine;

namespace Asce.Game.Progress
{
    public abstract class SO_UnlockCondition : ScriptableObject
    {
        [SerializeField] protected string _name;
        [SerializeField, TextArea(2, 3)] protected string _description;

        public string Name => _name;
        public string Description => _description;

        public abstract bool IsMet();
        public abstract void Met();
    }
}