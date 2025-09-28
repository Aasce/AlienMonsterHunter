using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    [CreateAssetMenu(menuName = "Asce/Entities/Enemies Data", fileName = "Enemies Data")]
    public class SO_Enemies : ScriptableObject
    {
        [SerializeField] private List<Enemy> _enemies = new();
        private ReadOnlyCollection<Enemy> _enemiesReadonly;
        private Dictionary<string, Enemy> _enemiesDictionary;

        public ReadOnlyCollection<Enemy> Enemies => _enemiesReadonly ??= _enemies.AsReadOnly();
        public Enemy Get(string name)
        {
            if (string.IsNullOrEmpty(name)) return null;
            if (_enemiesDictionary == null) this.InitializeDictionary();
            _enemiesDictionary.TryGetValue(name, out Enemy enemy);
            return enemy;
        }

        private void InitializeDictionary()
        {
            _enemiesDictionary = new Dictionary<string, Enemy>();
            foreach (Enemy enemy in _enemies)
            {
                if (enemy == null) continue;
                if (enemy.Information == null) continue;
                if (_enemiesDictionary.ContainsKey(enemy.Information.Name)) continue;
                _enemiesDictionary[enemy.Information.Name] = enemy;
            }
        }
    }
}
