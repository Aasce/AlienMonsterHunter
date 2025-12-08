using Asce.Core.Attributes;
using Asce.Core.UIs;
using Asce.Game.Entities.Enemies;
using Asce.Game.Managers;
using Asce.Game.Maps;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.MainMenu.UIs.Panels
{
    public class UIMapLevelEnemyInformationItem : UIComponent
    {
        [Header("References")]
        [SerializeField] private RectTransform _newFlag;
        [SerializeField] private Image _icon;
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;

        [Space]
        [SerializeField] private TextMeshProUGUI _quantityText;

        [Header("Runtime")]
        [SerializeField, Readonly] private MapLevelEnemy _levelEnemy;

        public void Set(MapLevelEnemy enemy)
        {
            this.Unregister();
            _levelEnemy = enemy;
            this.Register();
        }

        private void Register()
        {
            if (_levelEnemy == null) return;
            Enemy enemyPrefab = GameManager.Instance.AllEnemies.Get(_levelEnemy.Name);
            if (enemyPrefab == null) return;

            _icon.sprite = enemyPrefab.Information.Icon;
            _nameText.text = enemyPrefab.Information.Name;
            _levelText.text = $"Lv.{_levelEnemy.Level}";

            _quantityText.text = _levelEnemy.Quantity.ToString();
        }

        private void Unregister()
        {
            if (_levelEnemy == null) return;

        }
    }
}
