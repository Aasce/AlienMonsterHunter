using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs.Panels
{
    /// <summary>
    ///     A confirmation panel with customizable title, description, and button actions.
    /// </summary>
    public class UIConfirmPanel : UIWindowPanel
    {
        [SerializeField] private TextMeshProUGUI _descriptionText;

        [Header("Yes Button")]
        [SerializeField] private Button _yesButton;
        [SerializeField] private TextMeshProUGUI _yesButtonText;

        [Header("No Button")]
        [SerializeField] private Button _noButton;
        [SerializeField] private TextMeshProUGUI _noButtonText;

        private Action _onYes;
        private Action _onNo;

        protected override void RefReset()
        {
            base.RefReset();
            _name = "Confirm";
        }

        public override void Hide()
        {
            base.Hide();
            this.ClearButtonEvents();
        }

        /// <summary>
        ///     Show the confirm panel with custom content and button actions.
        ///     <br/>
        ///     See <see cref="UIConfirmPanel.Set(string, string, string, string, Action, Action)"/>
        /// </summary>
        public void Show(
            string title,
            string description = "",
            string yesText = "Yes",
            string noText = "No",
            Action onYes = null,
            Action onNo = null)
        {
            this.Set(title, description, yesText, noText, onYes, onNo);
            base.Show();
        }

        /// <summary>
        ///     Set the confirm panel with custom content and button actions.
        /// </summary>
        /// <param name="title">Title text to display.</param>
        /// <param name="description">Description text to display.</param>
        /// <param name="yesText">Text for the Yes button.</param>
        /// <param name="noText">Text for the No button.</param>
        /// <param name="onYes">Callback invoked when Yes button is clicked.</param>
        /// <param name="onNo">Callback invoked when No button is clicked.</param>
        public void Set(
            string title,
            string description = "",
            string yesText = "Yes",
            string noText = "No",
            Action onYes = null,
            Action onNo = null)
        {
            // Assign texts
            if (_titleText != null) _titleText.text = title;
            if (_descriptionText != null) _descriptionText.text = description;

            if (_yesButtonText != null) _yesButtonText.text = yesText;
            if (_noButtonText != null) _noButtonText.text = noText;

            // Clear previous listeners
            ClearButtonEvents();

            // Register new listeners
            _onYes = onYes;
            _onNo = onNo;

            if (_yesButton != null)
            {
                _yesButton.gameObject.SetActive(_onYes != null);
                if (_onYes != null) _yesButton.onClick.AddListener(OnYesClicked);
            }

            if (_noButton != null)
            {
                _noButton.gameObject.SetActive(_onNo != null);
                if (_onNo != null) _noButton.onClick.AddListener(OnNoClicked);
            }
        }

        /// <summary> Invoked when the Yes button is clicked. </summary>
        private void OnYesClicked()
        {
            _onYes?.Invoke();
            this.Hide();
        }

        /// <summary>  Invoked when the No button is clicked. </summary>
        private void OnNoClicked()
        {
            _onNo?.Invoke();
            this.Hide();
        }

        /// <summary> Remove all registered listeners and callbacks. </summary>
        private void ClearButtonEvents()
        {
            if (_yesButton != null) _yesButton.onClick.RemoveAllListeners();
            if (_noButton != null) _noButton.onClick.RemoveAllListeners();

            _onYes = null;
            _onNo = null;
        }
    }
}
