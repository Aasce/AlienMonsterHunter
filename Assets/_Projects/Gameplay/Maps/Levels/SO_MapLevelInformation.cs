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
        [SerializeField, TextArea(2, 6)] private string _description;
        [SerializeField, SpritePreview] private Sprite _icon;

        [Header("Spawners")]
        [SerializeField] private List<MapLevelEnemy> _enemies = new();
        private ReadOnlyCollection<MapLevelEnemy> _enemiesReadonly;

        [Space]
        [SerializeField] private List<MapLevelGameStateCondition> _winConditions = new();
        [SerializeField] private List<MapLevelGameStateCondition> _loseConditions = new();
        private ReadOnlyCollection<MapLevelGameStateCondition> _winConditionsReadonly;
        private ReadOnlyCollection<MapLevelGameStateCondition> _loseConditionsReadonly;

        [Header("Awards")]
        [SerializeField] private List<MapLevelAward> _firstWinAwards = new();
        [SerializeField] private List<MapLevelAward> _winAwards = new();
        [SerializeField] private List<MapLevelAward> _playAwards = new();
        private ReadOnlyCollection<MapLevelAward> _firstWinAwardsReadonly;
        private ReadOnlyCollection<MapLevelAward> _winAwardsReadonly;
        private ReadOnlyCollection<MapLevelAward> _playAwardsReadonly;


        public int Level => _level;
        public string Description => _description;
        public Sprite Icon => _icon;

        public ReadOnlyCollection<MapLevelEnemy> Enemies => _enemiesReadonly ??= _enemies.AsReadOnly();
        public ReadOnlyCollection<MapLevelGameStateCondition> WinConditions => _winConditionsReadonly ??= _winConditions.AsReadOnly();
        public ReadOnlyCollection<MapLevelGameStateCondition> LoseConditions => _loseConditionsReadonly ??= _loseConditions.AsReadOnly();

        public ReadOnlyCollection<MapLevelAward> FirstWinAwards => _firstWinAwardsReadonly ??= _firstWinAwards.AsReadOnly();
        public ReadOnlyCollection<MapLevelAward> WinAwards => _winAwardsReadonly ??= _winAwards.AsReadOnly();
        public ReadOnlyCollection<MapLevelAward> PlayAwards => _playAwardsReadonly ??= _playAwards.AsReadOnly();

        public int TotalEnemiesQuantity => _enemies.Sum((enemy) => enemy?.Quantity ?? 0);
    }
}
