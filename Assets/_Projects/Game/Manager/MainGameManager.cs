using Asce.Game.Entities;
using Asce.Game.Guns;
using Asce.Game.Managers;
using Asce.Game.Players;
using Asce.Managers;
using UnityEngine;

namespace Asce.Game
{
    public class MainGameManager : MonoBehaviourSingleton<MainGameManager>
    {
        private void Start()
        {
            CreateCharacterForPlayer();

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
                gunInstance.Initialize();
                characterInstance.Gun = gunInstance;
            }

            Player.Instance.Character = characterInstance;
            Player.Instance.Initialize();
        }
    }
}