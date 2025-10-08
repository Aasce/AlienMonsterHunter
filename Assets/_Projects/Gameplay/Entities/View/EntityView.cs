using Asce.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Entities
{
    public class EntityView : GameComponent
    {
        [SerializeField] protected Transform _rootTransform;
        [SerializeField] protected List<Renderer> _renderers = new();

        public Transform RootTransform => _rootTransform != null ? _rootTransform : transform;
        public List<Renderer> Renderers => _renderers;


        protected override void RefReset()
        {
            base.RefReset();
            this.LoadRenderers();
        }

        public virtual void LoadRenderers()
        {
            _renderers.Clear();
            Renderer[] renderers = GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers)
            {
                if (renderer != null) _renderers.Add(renderer);
            }
        }

        public virtual void ResetStatus()
        {

        }
    }
}
