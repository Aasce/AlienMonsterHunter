using Asce.Game.Managers;
using Asce.Managers;
using Asce.PrepareGame.Picks;
using System.Linq;
using UnityEngine;

namespace Asce.PrepareGame
{
    public class PrepareGameManager : MonoBehaviourSingleton<PrepareGameManager>
    {
        [SerializeField] private string _mainGameSceneName;
        [SerializeField] private string _mainMenuSceneName;

        private void Start()
        {
            Shared.SetOrAdd("NewGame", true);
        }

        public void PlayGame()
        {
            if (PickController.Instance.CharacterPrefab == null) return;
            if (PickController.Instance.GunPrefab  == null) return;

            Shared.SetOrAdd("character", $"{PickController.Instance.CharacterPrefab.Information.Name}");
            Shared.SetOrAdd("gun", $"{PickController.Instance.GunPrefab.Information.Name}");
            Shared.SetOrAdd("supports", PickController.Instance.SupportPrefabs
                .Where(s => s != null && s.Information != null)
                .Select(s => s.Information.Name)
                .ToList()
            );

            SceneLoader.Instance.Load(_mainGameSceneName, delay: 0.5f);
        }

        public void BackToMainMenu()
        {
            SceneLoader.Instance.Load(_mainMenuSceneName, delay: 0.5f);
        }
    }
}
