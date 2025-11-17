using Asce.Game.Players;
using Asce.Game.UIs.Panels;
using Asce.MainGame.Managers;
using Asce.MainGame.UIs.HUDs;
using Asce.MainGame.UIs.Panels;
using Asce.MainGame.UIs.ToolTips;
using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.MainGame.UIs
{
    public class UIMainGameController : GameComponent
    {
        [SerializeField, Readonly] private UIGameHUDController _hud;
        [SerializeField, Readonly] private UIPanelController _panel;
        [SerializeField, Readonly] private UIWorldTooltipController _worldTooltip;


        public UIGameHUDController HUDController => _hud;
        public UIPanelController PanelController => _panel;
        public UIWorldTooltipController WorldTooltipController => _worldTooltip;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _hud);
            if (_hud == null) Debug.LogError($"[{typeof(UIMainGameController).ToString().ColorWrap(Color.red)}]] UIHUDController is not assigned", this);

            this.LoadComponent(out _panel);
            if (_panel == null) Debug.LogError($"[{typeof(UIMainGameController).ToString().ColorWrap(Color.red)}]] UIPanelController is not assigned", this);

            this.LoadComponent(out _worldTooltip);
            if (_worldTooltip == null) Debug.LogError($"[{typeof(UIMainGameController).ToString().ColorWrap(Color.red)}]] UITooltipController is not assigned", this);
        }

        public void Initialze()
        {
            HUDController.Initialize();
            PanelController.Initialize();
            WorldTooltipController.Initialize();
        }

        public void AssignUI()
        {
            this.SetSettings(MainGameManager.Instance.Player.Settings);
            HUDController.SupportsInformation.Caller = MainGameManager.Instance.Player.SupportCaller;
            HUDController.CharacterInformation.Character = MainGameManager.Instance.Player.Character;

            if (MainGameManager.Instance.Player.Character != null)
            {
                HUDController.GunInformation.Gun = MainGameManager.Instance.Player.Character.Gun;
                WorldTooltipController.InteractionTip.Interaction = MainGameManager.Instance.Player.Character.Interaction;
                MainGameManager.Instance.Player.Character.OnDead += Character_OnDead;
            } 

            MainGameManager.Instance.Player.OnCharacterChanged += Player_OnCharacterChanged;
            MainGameManager.Instance.GameStateController.OnGameStateChanged += MainGameManager_OnGameStateChanged;
        }

        public void SetSettings(PlayerSettings settings)
        {
            HUDController.CharacterInformation.Abilities.SetUseKeys(settings.UseAbilityKeys);
            HUDController.SupportsInformation.SetCallKeys(settings.CallSupportKeys);
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
                WorldTooltipController.InteractionTip.Interaction = args.NewValue.Interaction;
            }
        }

        private void Character_OnDead(Game.Combats.DamageContainer container)
        {
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
