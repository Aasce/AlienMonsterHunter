using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs.Panels
{
    public class UIFullScreenPanel : UIPanel
    {
        [SerializeField] protected TextMeshProUGUI _titleText;
        [SerializeField] protected Button _closeButton;

        protected virtual void Start()
        {
            _closeButton.onClick.AddListener(CloseButton_OnClick);
        }

        protected virtual void CloseButton_OnClick()
        {
            this.Hide();
        }
    }
}
