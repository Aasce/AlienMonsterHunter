using Asce.Game.Entities.Characters;
using Asce.Game.Managers;
using System.Collections.Generic;

namespace Asce.PrepareGame.UIs.Collections
{
    public class UICharacterCollectionView : UICollectionView<Character>
    {
        public override IEnumerable<Character> Collection => GameManager.Instance.AllCharacters.Characters;

        public override void ItemClick(UICollectionItem<Character> uiItem)
        {
            base.ItemClick(uiItem);
            if (UIPrepareGameController.Instance.HUD.Picked.CharacterSlot == null) return; 
            UIPrepareGameController.Instance.HUD.Picked.CharacterSlot.Set(uiItem.Item);
        }

    }
}
