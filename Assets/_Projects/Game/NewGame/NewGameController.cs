using Asce.Game.Entities.Characters;
using Asce.Game.Enviroments;
using Asce.Game.Guns;
using Asce.Game.Managers;
using Asce.Game.Players;
using Asce.Managers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.MainGame.Managers
{
    public class NewGameController : GameComponent
    {

        public virtual void Initialize()
        {

        }

        public void CreateNewGame()
        {
            this.CreateCharacterForPlayer();
            this.CreateSupportForPlayer();
            this.CreateSpawners();
        }

        private void CreateCharacterForPlayer()
        {
            string characterName = Shared.Get<string>("character");
            string gunName = Shared.Get<string>("gun");

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
            MainGameManager.Instance.Player.Character.transform.position = EnviromentController.Instance.CharacterSpawnPoint;
            MainGameManager.Instance.Player.Character.Initialize();
            PlayerManager.Instance.Progress.CharactersProgress.ApplyTo(MainGameManager.Instance.Player.Character);
        }

        private void CreateSupportForPlayer()
        {
            List<string> supportNames = Shared.Get<List<string>>("supports");
            if (supportNames == null) return;
            MainGameManager.Instance.Player.SupportCaller.Initialize(supportNames);
        }

        private void CreateSpawners()
        {
            MainGameManager.Instance.SpawnerController.OnCreate();
            MainGameManager.Instance.SpawnerController.StartSpawn();
        }

    }
}
