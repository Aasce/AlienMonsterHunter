using Asce.Game.Entities.Characters;
using Asce.Game.Interactions;
using Asce.Managers.Attributes;
using Asce.Managers.UIs;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.MainGame.UIs.ToolTips
{
    public class UIInteractionTip : UIObject
    {
        [SerializeField, Readonly] private CharacterInteraction _interaction;
        [SerializeField, Readonly] private UIInteractionTipBoard _board;

        public CharacterInteraction Interaction
        {
            get => _interaction;
            set
            {
                if (_interaction == value) return;
                this.Unregister();
                _interaction = value;
                this.Register();
            }
        }

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _board);
        }

        public void Initialize()
        {

        }

        private void Register()
        {
            if (Interaction == null) return;
            _board.Set(Interaction.FocusInteractiveObject);
            Interaction.OnFocusNewObject += Interaction_OnFocusNewObject;
        }

        private void Unregister()
        {
            if (Interaction == null) return;
            Interaction.OnFocusNewObject -= Interaction_OnFocusNewObject;
        }

        private void Interaction_OnFocusNewObject(InteractiveObject interactiveObject)
        {
            _board.Set(interactiveObject);
        }

    }
}
