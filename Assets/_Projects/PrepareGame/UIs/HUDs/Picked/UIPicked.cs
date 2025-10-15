using Asce.Managers.UIs;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.PrepareGame.UIs
{
    public class UIPicked : UIObject
    {
        [SerializeField] private UICharacterPickedSlot _characterSlot;
        [SerializeField] private UIGunPickedSlot _gunSlot;
        [SerializeField] private List<UISupportPickedSlot> _supportSlots;

        public UICharacterPickedSlot CharacterSlot => _characterSlot;
        public UIGunPickedSlot GunSlot => _gunSlot;

        public List<UISupportPickedSlot> SupportSlots => _supportSlots;

        private void Start()
        {
            this.Initialize();

            bool hasSaveFile = false;
            if (hasSaveFile)
            {

            }
            else
            {
                this.CharacterSlot.Set(null);
                this.GunSlot.Set(null);
                foreach (UISupportPickedSlot supportSlot in this.SupportSlots)
                {
                    supportSlot.Set(null);
                }
            }
        }

        private void Initialize()
        {
            for (int i = 0; i < SupportSlots.Count; i++)
            {
                UISupportPickedSlot slot = SupportSlots[i];
                slot.SlotIndex = i;
            }
        }
    }
}
