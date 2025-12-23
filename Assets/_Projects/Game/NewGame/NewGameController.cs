using Asce.Core;
using Asce.Game.Entities.Characters;
using Asce.Game.Enviroments;
using Asce.Game.Guns;
using Asce.Game.Managers;
using Asce.Game.Maps;
using Asce.Game.Players;
using Asce.Game.Progress;
using Asce.Game.Spawners;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.MainGame.Managers
{
    public class NewGameController : ControllerComponent
    {
        public override string ControllerName => "New Game";


        public override void Initialize()
        {
            base.Initialize();

        }

        public void CreateNewGame()
        {
            this.CreateMapLevel();
            this.CreateLoadout();
        }

        private void CreateLoadout()
        {
            if (!GameManager.Instance.Shared.TryGet("Loadout", out PickLoadoutShareData loadoutData))
            {
                Debug.LogError("[NewGameController] Loadout Share Data is null", this);
            }

            this.CreateCharacterForPlayer(loadoutData.CharacterName, loadoutData.GunName);
            this.CreateSupportForPlayer(loadoutData.SupportNames);
        }

        private void CreateMapLevel()
        {
            if (!GameManager.Instance.Shared.TryGet("MapLevel", out PickMapLevelShareData mapLevelData))
            {
                Debug.LogError("[NewGameController] Map Level Share Data is null", this);
            }

            Map mapPrefab = GameManager.Instance.AllMaps.Get(mapLevelData.MapName);
            Map map = Instantiate(mapPrefab);
            EnviromentController.Instance.SetMap(map);

            SO_MapLevelInformation levelInformation = map.Information.GetLevel(mapLevelData.Level);
            this.CreateWinLoseCondition(levelInformation);
            this.CreateSpawners(levelInformation);
        }

        private void CreateCharacterForPlayer(string characterName, string gunName)
        {
            Gun gunPrefab = GameManager.Instance.AllGuns.Get(gunName);
            Gun gunInstance = Instantiate(gunPrefab);
            gunInstance.name = gunPrefab.name;

            Character characterPrefab = GameManager.Instance.AllCharacters.Get(characterName);
            Character characterInstance = Instantiate(characterPrefab);
            if (characterInstance != null)
            {
                characterInstance.Gun = gunInstance;
            }

            MainGameManager.Instance.Player.Character = characterInstance;
            MainGameManager.Instance.Player.Character.transform.position = EnviromentController.Instance.SpawnPoints.CharacterSpawnPoint;
            MainGameManager.Instance.Player.Character.Initialize();
            PlayerManager.Instance.Progress.CharactersProgress.ApplyTo(MainGameManager.Instance.Player.Character);
            PlayerManager.Instance.Progress.GunsProgress.ApplyTo(MainGameManager.Instance.Player.Character.Gun);
        }

        private void CreateSupportForPlayer(List<string> supportNames)
        {
            if (supportNames == null) return;
            MainGameManager.Instance.Player.SupportCaller.Initialize(supportNames);
        }

        private void CreateWinLoseCondition(SO_MapLevelInformation levelInformation)
        {
            foreach (MapLevelGameStateCondition winCondition in levelInformation.WinConditions)
            {
                GameStateCondition conditionPrefab = GameManager.Instance.AllWinLoseConditions.Get(winCondition.ConditionName);
                if (conditionPrefab == null) continue;

                GameStateCondition condition = Instantiate(conditionPrefab);
                condition.transform.SetParent(MainGameManager.Instance.GameStateController.transform);

                condition.SetData(winCondition);
                MainGameManager.Instance.GameStateController.WinConditions.Add(condition);
            }

            foreach (MapLevelGameStateCondition loseCondition in levelInformation.LoseConditions)
            {
                GameStateCondition conditionPrefab = GameManager.Instance.AllWinLoseConditions.Get(loseCondition.ConditionName);
                if (conditionPrefab == null) continue;

                GameStateCondition condition = Instantiate(conditionPrefab);
                condition.transform.SetParent(MainGameManager.Instance.GameStateController.transform);

                condition.SetData(loseCondition);
                MainGameManager.Instance.GameStateController.LoseConditions.Add(condition);
            }
        }

        private void CreateSpawners(SO_MapLevelInformation levelInformation)
        {
            InitialEnemySpawner initialSpawnerPrefab = GameManager.Instance.AllSpawners.Get<InitialEnemySpawner>("Initial");
            if (initialSpawnerPrefab == null) return;

            InitialEnemySpawner initialSpawner = Instantiate(initialSpawnerPrefab);
            initialSpawner.transform.SetParent(MainGameManager.Instance.SpawnerController.transform);
            MainGameManager.Instance.SpawnerController.Spawners.Add(initialSpawner);

            foreach (MapLevelEnemy enemy in levelInformation.Enemies)
            {
                EnemySpawnContainer container = new (enemy.Name, enemy.Quantity, enemy.Level);
                initialSpawner.Enemies.Add(container);
            }

            MainGameManager.Instance.SpawnerController.OnCreate();
            MainGameManager.Instance.SpawnerController.StartSpawn();
        }

    }
}
