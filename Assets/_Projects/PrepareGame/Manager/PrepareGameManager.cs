using Asce.Core;
using Asce.Game.Managers;
using Asce.Game.Players;
using Asce.Game.Progress;
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
            GameManager.Instance.Shared.SetOrAdd("NewGame", true);
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
            PickLoadoutShareData loadoutData = PickController.Instance.CreateLoadoutData();
            if (string.IsNullOrEmpty(loadoutData.CharacterName)) return;
            if (string.IsNullOrEmpty(loadoutData.GunName)) return;

            GameManager.Instance.Shared.SetOrAdd("Loadout", loadoutData);

            this.SaveAll();
            SceneLoader.Instance.Load(_mainGameSceneName, delay: 0.5f);
        }

        public void BackToMainMenu()
        {
            this.SaveAll();
            SceneLoader.Instance.Load(_mainMenuSceneName, delay: 0.5f);
        }

        private void SaveAll()
        {
            PrepareGameSaveLoadController.Instance.SaveLastPick();
            PlayerManager.Instance.Progress.SaveAll();
            PlayerManager.Instance.Items.SaveAll();
        }
    }
}
