using Asce.Core.UIs;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.PrepareGame.UIs
{
    public class UIPicked : UIComponent
    {
        [SerializeField] private Button _loadLastPickButton;

        [Space]
        [SerializeField] private UICharacterPickedSlot _characterSlot;
        [SerializeField] private UIGunPickedSlot _gunSlot;
        [SerializeField] private List<UISupportPickedSlot> _supportSlots;

        public UICharacterPickedSlot CharacterSlot => _characterSlot;
        public UIGunPickedSlot GunSlot => _gunSlot;

        public List<UISupportPickedSlot> SupportSlots => _supportSlots;

        private void Start()
        {
            this.Initialize();
            _loadLastPickButton.onClick.AddListener(LoadLastPickButton_OnClick);
        }

        private void Initialize()
        {
            for (int i = 0; i < SupportSlots.Count; i++)
            {
                UISupportPickedSlot slot = SupportSlots[i];
                slot.SlotIndex = i;
            }

            this.CharacterSlot.Initialize();
            this.GunSlot.Initialize();
            foreach (UISupportPickedSlot supportSlot in this.SupportSlots)
            {
                supportSlot.Initialize();
            }
        }


        private void LoadLastPickButton_OnClick()
        {
            PrepareGameSaveLoadController.Instance.LoadLastPick();
        }

    }
}
