using Asce.Game.Managers;
using Asce.Managers;
using Asce.Menu.UIs;
using Asce.PrepareGame.Picks;
using Asce.PrepareGame.UIs;
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
            this.InitialzeControllers();
            Shared.SetOrAdd("NewGame", true);
        }

        private void InitialzeControllers()
        {
            UIPrepareGameController.Instance.Initialize();
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

            PrepareGameSaveLoadController.Instance.SaveLastPick();
            SceneLoader.Instance.Load(_mainGameSceneName, delay: 0.5f);
        }

        public void BackToMainMenu()
        {
            PrepareGameSaveLoadController.Instance.SaveLastPick();
            SceneLoader.Instance.Load(_mainMenuSceneName, delay: 0.5f);
        }
    }
}
