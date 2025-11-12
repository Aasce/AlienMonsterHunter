using Asce.Game.Effects;
using Asce.Game.Entities;
using Asce.Managers.Attributes;
using Asce.Managers.UIs;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.MainGame.UIs.HUDs
{
    public class UIEffects : UIObject
    {
        [SerializeField] private Pool<UIEffect> _pool = new();

        [Header("Runtime")]
        [SerializeField, Readonly] private EntityEffects _entityEffects;

        public EntityEffects EntityEffects
        {
            get => _entityEffects;
            set
            {
                if (_entityEffects == value) return;
                this.Unregister();
                _entityEffects = value;
                this.Register();
            }
        }

        private void Register()
        {
            if (EntityEffects == null) return;
            this.HandleEffects();
            EntityEffects.OnEffectAdded += EntityEffects_OnEffectAdded;
            EntityEffects.OnEffectRemoved += EntityEffects_OnEffectRemoved;
        }

        private void Unregister()
        {
            if (EntityEffects == null) return;
            EntityEffects.OnEffectAdded -= EntityEffects_OnEffectAdded;
            EntityEffects.OnEffectRemoved -= EntityEffects_OnEffectRemoved;
        }

        private void HandleEffects()
        {
            foreach (Effect effect in EntityEffects.Effects)
            {
                UIEffect uiEffect = _pool.Activate();
                if (uiEffect == null) continue;

                uiEffect.Effect = effect;
                uiEffect.RectTransform.SetAsLastSibling();
                uiEffect.Show();
            }
        }

        private void EntityEffects_OnEffectAdded(Effect effect)
        {
            UIEffect uiEffect = _pool.Activate();
            if (uiEffect == null) return;

            uiEffect.Effect = effect;
            uiEffect.RectTransform.SetAsLastSibling();
            uiEffect.Show();
        }

        private void EntityEffects_OnEffectRemoved(Effect effect)
        {
            UIEffect uiEffect = _pool.Activities.Find((ui) => ui.Effect == effect);
            if (uiEffect == null) return;

            uiEffect.Effect = null;
            uiEffect.Hide();
            _pool.Deactivate(uiEffect);
        }

    }
}
