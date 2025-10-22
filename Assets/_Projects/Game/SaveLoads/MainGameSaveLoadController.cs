using Asce.Game.Abilities;
using Asce.Game.Entities.Characters;
using Asce.Game.Entities.Enemies;
using Asce.Game.Guns;
using Asce.Game.Managers;
using Asce.Game.Players;
using Asce.Game.SaveLoads;
using Asce.Game.Supports;
using Asce.Managers;
using Asce.Managers.Attributes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game
{
    public class MainGameSaveLoadController : MonoBehaviourSingleton<MainGameSaveLoadController>
    {
        private readonly Dictionary<string, bool> _isLoadeds = new();

        public void SaveCurrentGame()
        {
            this.SaveEnemies();
            this.SaveCharacter();
            this.SaveAbilities();
            this.SaveSupports();
            this.SaveSupportCaller();
        }

        public void LoadCurrentGame()
        {
            this.LoadEnemies();
            this.LoadCharacter();
            this.LoadAbilities();
            this.LoadSupports();
            this.LoadSupportCaller();
        }

        public bool IsLoaded(string key)
        {
            return _isLoadeds.ContainsKey(key) && _isLoadeds[key];
        }


        public void SaveEnemies()
        {
            AllEntitiesSaveData<EnemySaveData> allData = new();
            List<Enemy> enemies = EnemyController.Instance.GetAllEnemies();
            foreach (Enemy enemy in enemies)
            {
                if (enemy is ISaveable<EnemySaveData> saveable)
                    allData.entities.Add(saveable.Save());
            }

            SaveLoadManager.Instance.Save("CurrentGameEnemies", allData);
        }

        private void SaveCharacter()
        {
            CharacterSaveData characterData = (Player.Instance.Character as ISaveable<CharacterSaveData>).Save();
            SaveLoadManager.Instance.Save("CurrentGameCharacter", characterData);
        }

        private void SaveAbilities()
        {
            AllAbilitiesSaveData<AbilitySaveData> allData = new();
            List<Ability> abilities = AbilityController.Instance.GetAllAbilities();
            foreach (Ability ability in abilities)
            {
                if (ability is ISaveable<AbilitySaveData> saveable)
                    allData.abilities.Add(saveable.Save());
            }

            SaveLoadManager.Instance.Save("CurrentGameAbilities", allData);
        }

        private void SaveSupports()
        {
            AllSupportsSaveData allData = new();
            List<Support> supports = SupportController.Instance.GetAllSupports();
            foreach (Support support in supports)
            {
                if (support is ISaveable<SupportSaveData> saveable)
                    allData.supports.Add(saveable.Save());
            }

            SaveLoadManager.Instance.Save("CurrentGameSupports", allData);

        }

        private void SaveSupportCaller()
        {
            SupportCallerSaveData supportCallerData = (Player.Instance.SupportCaller as ISaveable<SupportCallerSaveData>).Save();
            SaveLoadManager.Instance.Save("CurrentGameSupportCaller", supportCallerData);
        }



        private void LoadEnemies()
        {
            AllEntitiesSaveData<EnemySaveData> allData = SaveLoadManager.Instance.Load<AllEntitiesSaveData<EnemySaveData>>("CurrentGameEnemies");
            if (allData == null) return;
            foreach (EnemySaveData data in allData.entities)
            {
                Enemy enemy = EnemyController.Instance.Spawn(data.name, data.position);
                if (enemy is ILoadable<EnemySaveData> loadable)
                {
                    loadable.Load(data);
                }
            }
            _isLoadeds["Enemies"] = true;
        }

        private void LoadCharacter()
        {
            CharacterSaveData characterData = SaveLoadManager.Instance.Load<CharacterSaveData>("CurrentGameCharacter");
            if (characterData == null) return;
            Gun gun = CreateGunFromData(characterData.gun);

            Character characterPrefab = GameManager.Instance.AllCharacters.Get(characterData.name);
            Character characterInstance = Instantiate(characterPrefab);
            if (characterInstance != null)
            {
                characterInstance.Gun = gun;
            }

            Player.Instance.Character = characterInstance;
            Player.Instance.InitializeCharacter();

            (characterInstance as ILoadable<CharacterSaveData>).Load(characterData);
            _isLoadeds["Character"] = true;
        }

        private void LoadAbilities()
        {
            AllAbilitiesSaveData<AbilitySaveData> allData = SaveLoadManager.Instance.Load<AllAbilitiesSaveData<AbilitySaveData>>("CurrentGameAbilities");
            if (allData == null) return;
            foreach (AbilitySaveData data in allData.abilities)
            {
                Ability ability = AbilityController.Instance.Spawn(data.name, null);
                if (ability == null) continue;
                if (ability is ILoadable<AbilitySaveData> loadable)
                {
                    loadable.Load(data);
                }
                ability.gameObject.SetActive(true);
                ability.OnActive();
            }
            _isLoadeds["Abilities"] = true;
        }

        private void LoadSupports()
        {
            AllSupportsSaveData allData = SaveLoadManager.Instance.Load<AllSupportsSaveData>("CurrentGameSupports");
            if (allData == null) return;
            foreach (SupportSaveData data in allData.supports)
            {
                Support support = SupportController.Instance.Spawn(data.nameId);
                if (support == null) continue;
                if (support is ILoadable<SupportSaveData> loadable)
                {
                    loadable.Load(data);
                }

                support.gameObject.SetActive(true);
                support.OnLoad();
            }
            _isLoadeds["Supports"] = true;
        }

        private void LoadSupportCaller()
        {
            SupportCallerSaveData supportCallerData = SaveLoadManager.Instance.Load<SupportCallerSaveData>("CurrentGameSupportCaller");
            if (supportCallerData == null) return;

            Player.Instance.InitializeSupportCaller();
            (Player.Instance.SupportCaller as ILoadable<SupportCallerSaveData>).Load(supportCallerData);

            _isLoadeds["SupportCaller"] = true;
        }


        private Gun CreateGunFromData(GunSaveData gunData)
        {
            if (gunData == null)
            {
                Debug.LogError("Gun data is null");
                return null;
            }

            Gun gunPrefab = GameManager.Instance.AllGuns.Get(gunData.name);
            Gun gunInstance = Instantiate(gunPrefab);
            gunInstance.name = gunPrefab.name;
            return gunInstance;
        }
    }
}
