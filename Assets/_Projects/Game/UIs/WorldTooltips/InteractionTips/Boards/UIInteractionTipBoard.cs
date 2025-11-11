using Asce.Game.Interactions;
using Asce.Game.Managers;
using Asce.MainGame.Managers;
using Asce.Managers.Attributes;
using Asce.Managers.UIs;
using Asce.Managers.Utils;
using TMPro;
using UnityEngine;

namespace Asce.MainGame.UIs.ToolTips
{
    public class UIInteractionTipBoard : UIObject
    {
        [Header("References")]
        [SerializeField] private TextMeshProUGUI _text;

        [Space]
        [SerializeField] private Vector2 _offset = new (0f, -1f); 
        
        [Header("Runtime")]
        [SerializeField, Readonly] private InteractiveObject _interactiveObject;

        protected Canvas Canvas => MainGameManager.Instance.UIController.WorldTooltipController.Canvas;
        protected string InteractKey => MainGameManager.Instance.Player.Settings.InteractKey.ToReadableString();


        public void Set(InteractiveObject interactiveObject)
        {
            _interactiveObject = interactiveObject;
            if (_interactiveObject == null)
            {
                this.Hide();
                return;
            }

            this.Show();
            string focusDescription = _interactiveObject.Information.FocusDescription;
            string description = string.IsNullOrEmpty(focusDescription) ? "interacting" : focusDescription;
            _text.text = $"Press [{InteractKey}] to {description}.";
        }

        private void LateUpdate()
        {
            if (_interactiveObject == null) return;
            Vector2 worldPosition = (Vector2)_interactiveObject.transform.position + _offset;
            Vector2 screenPosition = CameraController.Instance.MainCamera.WorldToScreenPoint(worldPosition);

            RectTransform.position = screenPosition;
        }
    }
}