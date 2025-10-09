using Asce.Game.Entities.Characters;
using Asce.Game.Guns;
using Asce.Game.Managers;
using Asce.Game.Players;
using Asce.Game.UIs;
using Asce.Game.UIs.Panels;
using Asce.Managers;
using System;
using UnityEngine;

namespace Asce.Game
{
    public class MainGameManager : MonoBehaviourSingleton<MainGameManager>
    {
        private void Start()
        {
            this.InitializeController();
            this.CreateCharacterForPlayer();
            this.AssignUI();
        }

        private void InitializeController()
        {
            UIGameController.Instance.PanelController.HideAll();
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

        private void AssignUI()
        {
            Player.Instance.Character.OnDead += Character_OnDead;
            Player.Instance.OnCharacterChanged += Player_OnCharacterChanged;
        }

        private void Player_OnCharacterChanged(ValueChangedEventArgs<Character> args)
        {
            if (args.OldValue != null) args.OldValue.OnDead -= Character_OnDead;
            if (args.NewValue != null)
            {
                args.NewValue.OnDead -= Character_OnDead;
                args.NewValue.OnDead += Character_OnDead;
            }
        }

        private void Character_OnDead()
        {
            Player.Instance.Character.gameObject.SetActive(false);
            UIDeathPanel deathPanel = UIGameController.Instance.PanelController.GetPanel<UIDeathPanel>();
            if (deathPanel == null) return;

            deathPanel.OnReviveClicked -= DeathPanel_OnReviveClicked;
            deathPanel.OnReviveClicked += DeathPanel_OnReviveClicked;
            deathPanel.Show();
        }

        private void DeathPanel_OnReviveClicked()
        {
            Player.Instance.ReviveCharacter(isReviveAtSpawnPoint: true);
        }
    }
}