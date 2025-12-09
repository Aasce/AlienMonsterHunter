using UnityEngine;

namespace Asce.Game.Spawners
{
    [System.Serializable]
    public class EnemySpawnContainer
    {
        [SerializeField] private string _enemyName;
        [SerializeField, Min(0)] private int _quantity;
        [SerializeField] private Vector2Int _levelRange = Vector2Int.zero;

        public string EnemyName => _enemyName;
        public int Quantity => _quantity;
        public Vector2Int LevelRange => _levelRange;
        public int Level => LevelRange.y;


        public EnemySpawnContainer() : this (string.Empty, 0, 0) { }
        public EnemySpawnContainer(string enemyName, int quantity, int level)
            : this (enemyName, quantity, new Vector2Int(level, level)) { }
        public EnemySpawnContainer(string enemyName, int quantity, Vector2Int levelRange)
        {
            _enemyName = enemyName;
            _quantity = quantity;
            _levelRange = levelRange;
        }
    }
}