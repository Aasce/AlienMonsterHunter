using Asce.Managers;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    [CreateAssetMenu(menuName = "Asce/Entities/All Enemies", fileName = "All Enemies")]
    public class SO_AllEnemies : ScriptableObject
    {
        [SerializeField] private ListObjects<string, Enemy> _enemies = new((enemy) => {
            if (enemy == null) return null;
            if (enemy.Information == null) return null;
            return enemy.Information.Name;
        });

        public ReadOnlyCollection<Enemy> Enemies => _enemies.List;
        public Enemy Get(string name) => _enemies.Get(name);
    }
}
