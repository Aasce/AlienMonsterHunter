using Asce.Game.Abilities;
using Asce.Game.Entities.Characters;
using Asce.Game.Entities.Enemies;
using Asce.Game.Guns;
using Asce.Game.Interactions;
using Asce.Game.Managers;
using Asce.Game.SaveLoads;
using Asce.Game.Supports;
using Asce.MainGame.Managers;
using Asce.Core;
using Asce.SaveLoads;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.MainGame
{
    public class MainGameSaveLoadController : ControllerComponent
    {
        private readonly Dictionary<string, bool> _isLoadeds = new();

        public override string ControllerName => "Save Load";


        public override void Initialize()
        {
            base.Initialize();
        }

        public void SaveCurrentGame()
        {
            this.SaveGameConfig();
            this.SaveWinLoseCondition();
            this.SaveEnemies();
            this.SaveSpawners();
            this.SaveCharacter();
            this.SaveAbilities();
            this.SaveSupports();
            this.SaveSupportCaller();
            this.SaveInteractiveObjects();
        }

        public void LoadCurrentGame()
        {
            this.LoadGameConfig();
            this.LoadWinLoseCondition();
            this.LoadEnemies();
            this.LoadSpawners();
            this.LoadCharacter();
            this.LoadAbilities();
            this.LoadSupports();
            this.LoadSupportCaller();
            this.LoadInteractiveObjects();
        }

        public bool IsLoaded(string key)
        {
            return _isLoadeds.ContainsKey(key) && _isLoadeds[key];
        }

        public void SaveGameConfig()
        {
            CurrentGameConfigData configData = new CurrentGameConfigData();
            configData.hasSave = MainGameManager.Instance.GameStateController.IsPlaying;
            configData.playTime = MainGameManager.Instance.PlayTimeController.ElapsedTime;

            SaveLoadManager.Instance.Save("CurrentGameConfig", configData);
        }

        public void SaveWinLoseCondition()
        {
            AllGameStateConditionsSaveData allData = new();

            foreach (GameStateCondition winCondition in MainGameManager.Instance.GameStateController.WinConditions)
            {
                GameStateConditionSaveData saveData = (winCondition as ISaveable<GameStateConditionSaveData>).Save();
                allData.winConditions.Add(saveData);
            }

            foreach (GameStateCondition loseCondition in MainGameManager.Instance.GameStateController.LoseConditions)
            {
                GameStateConditionSaveData saveData = (loseCondition as ISaveable<GameStateConditionSaveData>).Save();
                allData.loseConditions.Add(saveData);
            }

            SaveLoadManager.Instance.Save("CurrentGameWinLoseCondition", allData);
        }

        public void SaveEnemies()
        {
            AllEntitiesSaveData<EnemySaveData> allData = new();
            List<Enemy> enemies = EnemyController.Instance.GetAllEnemies();
            foreach (Enemy enemy in enemies)
            {
                if (enemy is ISaveable<EnemySaveData> saveable)
                {
                    if (!saveable.IsNeedSave) continue;
                    allData.entities.Add(saveable.Save());
                }
            }

            SaveLoadManager.Instance.Save("CurrentGameEnemies", allData);
        }

        private void SaveSpawners()
        {

        }

        private void SaveCharacter()
        {
            CharacterSaveData characterData = (MainGameManager.Instance.Player.Character as ISaveable<CharacterSaveData>).Save();
            SaveLoadManager.Instance.Save("CurrentGameCharacter", characterData);
        }

        private void SaveAbilities()
        {
            AllAbilitiesSaveData<AbilitySaveData> allData = new();
            List<Ability> abilities = AbilityController.Instance.GetAllAbilities();
            foreach (Ability ability in abilities)
            {
                if (ability is ISaveable<AbilitySaveData> saveable)
                {
                    if (!saveable.IsNeedSave) continue;
                    allData.abilities.Add(saveable.Save());
                }
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
                {
                    if (!saveable.IsNeedSave) continue;
                    allData.supports.Add(saveable.Save());
                }
            }

            SaveLoadManager.Instance.Save("CurrentGameSupports", allData);

        }

        private void SaveSupportCaller()
        {
            SupportCallerSaveData supportCallerData = (MainGameManager.Instance.Player.SupportCaller as ISaveable<SupportCallerSaveData>).Save();
            SaveLoadManager.Instance.Save("CurrentGameSupportCaller", supportCallerData);
        }

        private void SaveInteractiveObjects()
        {
            AllInteractiveObjectsSaveData allData = new();
            List<InteractiveObject> interactiveObjects = InteractionController.Instance.GetAllInteractiveObjects();
            foreach (InteractiveObject interactiveObject in interactiveObjects)
            {
                if (interactiveObject is ISaveable<InteractiveObjectSaveData> saveable)
                {
                    if (!saveable.IsNeedSave) continue;
                    allData.interactiveObjects.Add(saveable.Save());
                }
            }

            SaveLoadManager.Instance.Save("CurrentInteractiveObjects", allData);

        }

        private void LoadGameConfig()
        {
            CurrentGameConfigData configData = SaveLoadManager.Instance.Load<CurrentGameConfigData>("CurrentGameConfig");
            if (configData == null) return;
			MainGameManager.Instance.PlayTimeController.SetElapsedTime(configData.playTime);

            _isLoadeds["GameConfig"] = true;
        }

        private void LoadWinLoseCondition()
        {
            AllGameStateConditionsSaveData allData = SaveLoadManager.Instance.Load<AllGameStateConditionsSaveData>("CurrentGameWinLoseCondition");
            if (allData == null) return;

            foreach (GameStateConditionSaveData winConditionData in allData.winConditions)
            {
                GameStateCondition winCondition = MainGameManager.Instance.GameStateController.WinConditions.Find((condition) => condition.ConditionName == winConditionData.name);
                if (winCondition is not ILoadable<GameStateConditionSaveData> loadable) continue;
                loadable.Load(winConditionData);
            }

            foreach (GameStateConditionSaveData loseConditionData in allData.loseConditions)
            {
                GameStateCondition loseCondition = MainGameManager.Instance.GameStateController.LoseConditions.Find((condition) => condition.ConditionName == loseConditionData.name);
                if (loseCondition is not ILoadable<GameStateConditionSaveData> loadable) continue;
                loadable.Load(loseConditionData);
            }

            _isLoadeds["WinLoseCondition"] = true;
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

        private void LoadSpawners()
        {

            MainGameManager.Instance.SpawnerController.StartSpawn();
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

            MainGameManager.Instance.Player.Character = characterInstance;
            MainGameManager.Instance.Player.Character.Initialize();

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

            (MainGameManager.Instance.Player.SupportCaller as ILoadable<SupportCallerSaveData>).Load(supportCallerData);
            MainGameManager.Instance.Player.SupportCaller.OnLoad();

            _isLoadeds["SupportCaller"] = true;
        }

        private void LoadInteractiveObjects()
        {
            AllInteractiveObjectsSaveData allData = SaveLoadManager.Instance.Load<AllInteractiveObjectsSaveData>("CurrentInteractiveObjects");
            if (allData == null) return;
            foreach (InteractiveObjectSaveData data in allData.interactiveObjects)
            {
                InteractiveObject interactiveObject = InteractionController.Instance.Spawn(data.name);
                if (interactiveObject == null) continue;
                if (interactiveObject is ILoadable<InteractiveObjectSaveData> loadable)
                {
                    loadable.Load(data);
                }

                interactiveObject.gameObject.SetActive(true);
                interactiveObject.OnLoad();
            }

            _isLoadeds["InteractiveObjects"] = true;
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
