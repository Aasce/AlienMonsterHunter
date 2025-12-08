using Asce.Core.Attributes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace Asce.Game.Maps
{
    [CreateAssetMenu(menuName = "Asce/Maps/Map Level Information", fileName = "Map Level Information")]
    public class SO_MapLevelInformation : ScriptableObject
    {
        [SerializeField] private int _level;
        [SerializeField] private string _description;
        [SerializeField, SpritePreview] private Sprite _icon;

        [Space]
        [SerializeField] private List<MapLevelEnemy> _enemies = new();
        private ReadOnlyCollection<MapLevelEnemy> _enemiesReadonly;

        [Space]
        [SerializeField] private List<MapLevelWinCondition> _winConditions = new();
        [SerializeField] private List<MapLevelLoseCondition> _loseConditions = new();
        private ReadOnlyCollection<MapLevelWinCondition> _winConditionsReadonly;
        private ReadOnlyCollection<MapLevelLoseCondition> _loseConditionsReadonly;

        public int Level => _level;
        public string Description => _description;
        public Sprite Icon => _icon;

        public ReadOnlyCollection<MapLevelEnemy> Enemies => _enemiesReadonly ??= _enemies.AsReadOnly();
        public ReadOnlyCollection<MapLevelWinCondition> WinConditions => _winConditionsReadonly ??= _winConditions.AsReadOnly();
        public ReadOnlyCollection<MapLevelLoseCondition> LoseConditions => _loseConditionsReadonly ??= _loseConditions.AsReadOnly();

        public int TotalEnemiesQuantity => _enemies.Sum((enemy) => enemy?.Quantity ?? 0);
    }
}
