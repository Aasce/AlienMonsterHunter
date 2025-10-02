using Asce.Game.Entities.Characters;
using Asce.Game.Entities.Enemies;
using Asce.Game.Guns;
using Asce.Managers;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Managers
{
    public class GameManager : DontDestroyOnLoadSingleton<GameManager>
    {
        [SerializeField] private SO_AllCharacters _allCharacters;
        [SerializeField] private SO_AllEnemies _allEnemies;
        [SerializeField] private SO_AllGuns _allGuns;

        public SO_AllCharacters AllCharacters => _allCharacters;
        public SO_AllEnemies AllEnemies => _allEnemies;
        public SO_AllGuns AllGuns => _allGuns;

        private void OnValidate()
        {
            if (AllCharacters == null) 
                Debug.LogError($"[{typeof(GameManager).ToString().ColorWrap(Color.red)}]] All Characters is not assigned", this);

            if (AllEnemies == null) 
                Debug.LogError($"[{typeof(GameManager).ToString().ColorWrap(Color.red)}]] All Enemies is not assigned", this);

            if (AllGuns == null) 
                Debug.LogError($"[{typeof(GameManager).ToString().ColorWrap(Color.red)}]] All Guns is not assigned", this);
        }
    }
}
