using Asce.Game.Abilities;
using Asce.Game.Effects;
using Asce.Game.Entities.Characters;
using Asce.Game.Entities.Enemies;
using Asce.Game.Guns;
using Asce.Game.Interactions;
using Asce.Game.Supports;
using Asce.Managers;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Managers
{
    public class GameManager : DontDestroyOnLoadSingleton<GameManager>
    {
        [SerializeField] private GameServices _gameServices;

        [Header("Scriptable Objects")]
        [SerializeField] private SO_AllCharacters _allCharacters;
        [SerializeField] private SO_AllEnemies _allEnemies;
        [SerializeField] private SO_AllGuns _allGuns;
        [SerializeField] private SO_AllAbilities _allAbilities;
        [SerializeField] private SO_AllSupports _allSupports;
        [SerializeField] private SO_AllEffects _allEffects;
        [SerializeField] private SO_AllInteractiveObjects _allInteractiveObjects;

        public GameServices GameServices => _gameServices;

        public SO_AllCharacters AllCharacters => _allCharacters;
        public SO_AllEnemies AllEnemies => _allEnemies;
        public SO_AllGuns AllGuns => _allGuns;
        public SO_AllAbilities AllAbilities => _allAbilities;
        public SO_AllSupports AllSupports => _allSupports;
        public SO_AllEffects AllEffects => _allEffects;
        public SO_AllInteractiveObjects AllInteractiveObjects => _allInteractiveObjects;

        private void OnValidate()
        {
            if (GameServices == null) 
                Debug.LogError($"[{typeof(GameManager).ToString().ColorWrap(Color.red)}]] Game Services is not assigned", this);


            if (AllCharacters == null) 
                Debug.LogError($"[{typeof(GameManager).ToString().ColorWrap(Color.red)}]] All Characters is not assigned", this);

            if (AllEnemies == null) 
                Debug.LogError($"[{typeof(GameManager).ToString().ColorWrap(Color.red)}]] All Enemies is not assigned", this);

            if (AllGuns == null) 
                Debug.LogError($"[{typeof(GameManager).ToString().ColorWrap(Color.red)}]] All Guns is not assigned", this);

            if (AllAbilities == null) 
                Debug.LogError($"[{typeof(GameManager).ToString().ColorWrap(Color.red)}]] All Abilities is not assigned", this);

            if (_allSupports == null) 
                Debug.LogError($"[{typeof(GameManager).ToString().ColorWrap(Color.red)}]] All Supports is not assigned", this);

            if (AllEffects == null) 
                Debug.LogError($"[{typeof(GameManager).ToString().ColorWrap(Color.red)}]] All Effects is not assigned", this);

            if (AllInteractiveObjects == null)
                Debug.LogError($"[{typeof(GameManager).ToString().ColorWrap(Color.red)}]] All Interactive Objects is not assigned", this);

        }

        public void QuitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
