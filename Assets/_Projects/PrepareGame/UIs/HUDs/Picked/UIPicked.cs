using Asce.Managers.UIs;
using UnityEngine;

namespace Asce.PrepareGame.UIs
{
    public class UIPicked : UIObject
    {
        [SerializeField] private UICharacterPickedSlot _characterSlot;
        [SerializeField] private UIGunPickedSlot _gunSlot;

        public UICharacterPickedSlot CharacterSlot => _characterSlot;
        public UIGunPickedSlot GunSlot => _gunSlot;

        private void Start()
        {
            bool hasSaveFile = false;
            if (hasSaveFile)
            {

            }
            else
            {
                if (CharacterSlot != null) CharacterSlot.Set(null);
                if (GunSlot != null) GunSlot.Set(null);
            }
        }
    }
}
