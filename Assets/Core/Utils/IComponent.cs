using UnityEngine;

namespace Asce.Managers
{
    public interface IComponent
    {
		public Transform transform { get; }
        public GameObject gameObject { get; }
    }
}