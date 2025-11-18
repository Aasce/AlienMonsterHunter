using Asce.Game.Managers;
using Asce.Game.Players;
using Asce.Managers;
using Asce.PrepareGame.Picks;
using Asce.PrepareGame.Players;
using Asce.PrepareGame.UIs;
using System.Linq;
using UnityEngine;

namespace Asce.PrepareGame.Manager
{
    public class PrepareGameManager : MonoBehaviourSingleton<PrepareGameManager>
    {
        [SerializeField] private PrepareGamePlayer _player;
        [SerializeField] private UIPrepareGameController _uiController;

        [Space]
        [SerializeField] private string _mainGameSceneName;
        [SerializeField] private string _mainMenuSceneName;

        public PrepareGamePlayer Player => _player;
        public UIPrepareGameController UIController => _uiController;

        private void Start()
        {
            this.Initialze();
            Shared.SetOrAdd("NewGame", true);
        }

        private void Initialze()
        {
            Player.Initialize();
            UIController.Initialize();
            PlayerManager.Instance.RegisterPlayer(Player);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (PlayerManager.Instance != null) PlayerManager.Instance.UnregisterPlayer(Player);
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

            this.BeforeChangeScene();
            SceneLoader.Instance.Load(_mainGameSceneName, delay: 0.5f);
        }

        public void BackToMainMenu()
        {
            this.BeforeChangeScene();
            SceneLoader.Instance.Load(_mainMenuSceneName, delay: 0.5f);
        }

        private void BeforeChangeScene()
        {
            PrepareGameSaveLoadController.Instance.SaveLastPick();
            PlayerManager.Instance.Progress.SaveAll();
        }
    }
}
