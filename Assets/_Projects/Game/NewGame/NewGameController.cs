using Asce.Core;
using Asce.Game.Entities.Characters;
using Asce.Game.Enviroments;
using Asce.Game.Guns;
using Asce.Game.Managers;
using Asce.Game.Players;
using Asce.Game.Progress;
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
            PickLoadoutShareData loadoutData  = GameManager.Instance.Shared.Get<PickLoadoutShareData>("Loadout");
            this.CreateCharacterForPlayer(loadoutData.CharacterName, loadoutData.GunName);
            this.CreateSupportForPlayer(loadoutData.SupportNames);
            this.CreateSpawners();
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

        private void CreateSpawners()
        {
            MainGameManager.Instance.SpawnerController.OnCreate();
            MainGameManager.Instance.SpawnerController.StartSpawn();
        }

    }
}
