using Asce.Game.UIs;
using Asce.Game.UIs.HUDs;
using Asce.Game.UIs.Panels;
using Asce.MainGame.Managers;
using Asce.MainGame.UIs.Panels;
using Asce.Managers;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.MainGame.UIs
{
    public class UIMainGameController : GameComponent
    {
        [SerializeField] private UIGameHUDController _hud;
        [SerializeField] private UIPanelController _panel;


        public UIGameHUDController HUDController => _hud;
        public UIPanelController PanelController => _panel;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _hud);
            if (_hud == null) Debug.LogError($"[{typeof(UIMainGameController).ToString().ColorWrap(Color.red)}]] UIHUDController is not assigned", this);

            this.LoadComponent(out _panel);
            if (_panel == null) Debug.LogError($"[{typeof(UIMainGameController).ToString().ColorWrap(Color.red)}]] UIPanelController is not assigned", this);
        }

        public void Initialze()
        {
            HUDController.Initialize();
            PanelController.Initialize();
        }

        public void AssignUI()
        {
            HUDController.SetSettings(MainGameManager.Instance.Player.Settings);
            HUDController.SupportsInformation.Caller = MainGameManager.Instance.Player.SupportCaller;
            HUDController.CharacterInformation.Character = MainGameManager.Instance.Player.Character;

            if (MainGameManager.Instance.Player.Character != null)
            {
                HUDController.GunInformation.Gun = MainGameManager.Instance.Player.Character.Gun;
                MainGameManager.Instance.Player.Character.OnDead += Character_OnDead;
            } 

            MainGameManager.Instance.Player.OnCharacterChanged += Player_OnCharacterChanged;
            MainGameManager.Instance.GameStateController.OnGameStateChanged += MainGameManager_OnGameStateChanged;
        }

        private void Player_OnCharacterChanged(ValueChangedEventArgs<Game.Entities.Characters.Character> args)
        {
            if (args.OldValue != null) args.OldValue.OnDead -= Character_OnDead;
            if (args.NewValue != null)
            {
                args.NewValue.OnDead -= Character_OnDead;
                args.NewValue.OnDead += Character_OnDead;

                HUDController.CharacterInformation.Character = args.NewValue;
                HUDController.GunInformation.Gun = args.NewValue.Gun;
            }
        }

        private void Character_OnDead(Game.Combats.DamageContainer container)
        {
            MainGameManager.Instance.Player.Character.gameObject.SetActive(false);
            UIDeathPanel deathPanel = PanelController.GetPanelByName("Death") as UIDeathPanel;
            if (deathPanel == null) return;

            deathPanel.OnReviveClicked -= DeathPanel_OnReviveClicked;
            deathPanel.OnReviveClicked += DeathPanel_OnReviveClicked;
            deathPanel.Show();
        }

        private void DeathPanel_OnReviveClicked()
        {
            MainGameManager.Instance.Player.ReviveCharacter(isReviveAtSpawnPoint: true);
        }

        private void MainGameManager_OnGameStateChanged(ValueChangedEventArgs<MainGameState> args)
        {
            if (args.NewValue == MainGameState.Completed)
            {
                UIGameVictoryPanel victoryPanel = PanelController.GetPanelByName("Game Victory") as UIGameVictoryPanel;
                if (victoryPanel == null) return;

                victoryPanel.Show();
            }

            else if (args.NewValue == MainGameState.Failed)
            {
                UIGameDefeatPanel defeatPanel = PanelController.GetPanelByName("Game Defeat") as UIGameDefeatPanel;
                if (defeatPanel == null) return;

                defeatPanel.Show();
            }
        }

    }
}
