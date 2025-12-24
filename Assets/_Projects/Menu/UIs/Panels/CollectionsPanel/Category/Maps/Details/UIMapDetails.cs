using Asce.Game;
using Asce.Game.Maps;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.MainMenu.UIs.Panels
{
    public class UIMapDetails : UICollectionDetails<Map>
    {
        [Header("Information")]
        [SerializeField] private Image _fullMap;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _mapSizeText;

        [Header("Locked")]
        [SerializeField] private RectTransform _lockContent;
        [SerializeField] private RectTransform _unlockContent;

        public override void Set(Map map)
        {
            if (Item == map) return;
            this.Unregister();
            Item = map;
            this.Register();
        }

        private void Register()
        {
            if (Item == null) return;
            if (Item.Information == null) return;

            this.SetLockedState();
            _fullMap.sprite = Item.Information.FullMap;
            _nameText.text = Item.Information.Name;
            _mapSizeText.text = $"{Item.Information.MapSize.x * 10f}m x {Item.Information.MapSize.y * 10f}m";
        }

        private void Unregister()
        {
            if (Item == null) return;
            if (Item.Information == null) return;

        }

        private void SetLockedState()
        {
            bool isUnlocked = true;
            _unlockContent.gameObject.SetActive(isUnlocked);
            _lockContent.gameObject.SetActive(!isUnlocked);

            if (isUnlocked)
            {

            }
            else
            {

            }
        }
    }
}
