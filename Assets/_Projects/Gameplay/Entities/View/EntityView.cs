using Asce.Core;
using Asce.Core.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Entities
{
    public class EntityView : GameComponent
    {
        [SerializeField] protected Transform _rootTransform;
        [SerializeField] protected List<Renderer> _renderers = new();

        protected Vector2 _baseScale = Vector2.one;

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

        public virtual void Initialize()
        {
            _baseScale = RootTransform.localScale;
        }

        public virtual void ResetStatus()
        {

        }

        public virtual void SetAlpha(float alpha)
        {
            alpha = Mathf.Clamp01(alpha);
            foreach (Renderer renderer in _renderers)
            {
                if (renderer is SpriteRenderer spriteRenderer)
                {
                    spriteRenderer.color = spriteRenderer.color.WithAlpha(alpha);
                }
            }
        }

        public virtual void SetSize(float size)
        {
            RootTransform.localScale = _baseScale * size;
        }
    }
}
