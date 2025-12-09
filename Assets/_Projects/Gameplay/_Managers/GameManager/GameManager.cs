using Asce.Core;
using Asce.Core.Utils;
using Asce.Game.Abilities;
using Asce.Game.Effects;
using Asce.Game.Entities.Characters;
using Asce.Game.Entities.Enemies;
using Asce.Game.Guns;
using Asce.Game.Interactions;
using Asce.Game.Items;
using Asce.Game.Maps;
using Asce.Game.Spawners;
using Asce.Game.Supports;
using System;
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
        [SerializeField] private SO_AllSupports _allSupports;
        [SerializeField] private SO_AllMaps _allMaps;
        [SerializeField] private SO_AllSpawners _allSpawners;
        [SerializeField] private SO_AllAbilities _allAbilities;
        [SerializeField] private SO_AllEffects _allEffects;
        [SerializeField] private SO_AllInteractiveObjects _allInteractiveObjects;
        [SerializeField] private SO_AllItems _allItems;

        public Shared _shared = new();

        public event Action OnQuitGame;

        public GameServices GameServices => _gameServices;

        public SO_AllCharacters AllCharacters => _allCharacters;
        public SO_AllEnemies AllEnemies => _allEnemies;
        public SO_AllGuns AllGuns => _allGuns;
        public SO_AllSupports AllSupports => _allSupports;
        public SO_AllMaps AllMaps => _allMaps;
        public SO_AllSpawners AllSpawners => _allSpawners;
        public SO_AllAbilities AllAbilities => _allAbilities;
        public SO_AllEffects AllEffects => _allEffects;
        public SO_AllInteractiveObjects AllInteractiveObjects => _allInteractiveObjects;
        public SO_AllItems AllItems => _allItems;

        public Shared Shared => _shared;

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

            if (AllSupports == null) 
                Debug.LogError($"[{typeof(GameManager).ToString().ColorWrap(Color.red)}]] All Supports is not assigned", this);

            if (AllMaps == null) 
                Debug.LogError($"[{typeof(GameManager).ToString().ColorWrap(Color.red)}]] All Maps is not assigned", this);

            if (AllSpawners == null) 
                Debug.LogError($"[{typeof(GameManager).ToString().ColorWrap(Color.red)}]] All Spawners is not assigned", this);

            if (AllAbilities == null) 
                Debug.LogError($"[{typeof(GameManager).ToString().ColorWrap(Color.red)}]] All Abilities is not assigned", this);

            if (AllEffects == null) 
                Debug.LogError($"[{typeof(GameManager).ToString().ColorWrap(Color.red)}]] All Effects is not assigned", this);

            if (AllInteractiveObjects == null)
                Debug.LogError($"[{typeof(GameManager).ToString().ColorWrap(Color.red)}]] All Interactive Objects is not assigned", this);

            if (AllItems == null)
                Debug.LogError($"[{typeof(GameManager).ToString().ColorWrap(Color.red)}]] All Items is not assigned", this);
        }

        public void QuitGame()
        {
            OnQuitGame?.Invoke();
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }
}
