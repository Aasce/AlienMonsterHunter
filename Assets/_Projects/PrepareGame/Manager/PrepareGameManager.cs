using Asce.Game.Managers;
using Asce.Managers;
using Asce.PrepareGame.Picks;
using UnityEngine;

namespace Asce.PrepareGame
{
    public class PrepareGameManager : MonoBehaviourSingleton<PrepareGameManager>
    {
        [SerializeField] private string _mainGameSceneName;


        public void PlayGame()
        {
            if (PickController.Instance.CharacterPrefab == null) return;
            if (PickController.Instance.GunPrefab  == null) return;

            Shared.SetOrAdd("character", $"{PickController.Instance.CharacterPrefab.Information.Name}");
            Shared.SetOrAdd("gun", $"{PickController.Instance.GunPrefab.Information.Name}");
            SceneLoader.Instance.Load(_mainGameSceneName);
        }

    }
}
