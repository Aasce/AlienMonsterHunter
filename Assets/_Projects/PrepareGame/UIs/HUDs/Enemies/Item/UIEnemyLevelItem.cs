using Asce.Core.Attributes;
using Asce.Core.UIs;
using Asce.Game.Entities.Enemies;
using Asce.Game.Managers;
using Asce.Game.Maps;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs.HUDs
{
    public class UIEnemyLevelItem : UIComponent, IPointerClickHandler
    {
        [SerializeField] private RectTransform _content;
        [SerializeField] private RectTransform _nothingContent;

        [Space]
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _quantityText;
        [SerializeField] private TextMeshProUGUI _levelText;

        [SerializeField, Readonly] private MapLevelEnemy _mapLevelEnemy;

        public void Set(MapLevelEnemy mapLevelEnemy)
        {
            if (_mapLevelEnemy == mapLevelEnemy) return;
            this.Unregister();
            _mapLevelEnemy = mapLevelEnemy;
            this.Register();
        }

        private void Register() 
        {
            if (_mapLevelEnemy == null) return;
            Enemy enemyPrefab = GameManager.Instance.AllEnemies.Get(_mapLevelEnemy.Name);
            if (enemyPrefab == null)
            {
                this.ShowContent(false);
                return;
            }

            this.ShowContent(true);
            _icon.sprite = enemyPrefab.Information.Icon;
            _nameText.text = enemyPrefab.Information.Name;
            _quantityText.text = $"x{_mapLevelEnemy.Quantity}";
            _levelText.text = $"{_mapLevelEnemy.Level}";
        }

        private void Unregister() 
        {
            if (_mapLevelEnemy == null) return;

        }

        public void ShowContent(bool isShowContent)
        {
            _content.gameObject.SetActive(isShowContent);
            _nothingContent.gameObject.SetActive(!isShowContent);
        }

        public void OnPointerClick(PointerEventData eventData)
        {

        }
    }
}
