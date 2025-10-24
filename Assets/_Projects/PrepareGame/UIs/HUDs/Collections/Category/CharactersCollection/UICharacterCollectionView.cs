using Asce.Game.Entities.Characters;
using Asce.Game.Managers;
using Asce.PrepareGame.Picks;
using System.Collections.Generic;

namespace Asce.PrepareGame.UIs.Collections
{
    public class UICharacterCollectionView : UICollectionView<Character>
    {
        public override IEnumerable<Character> Collection => GameManager.Instance.AllCharacters.Characters;

        public override void ItemClick(UICollectionItem<Character> uiItem)
        {
            base.ItemClick(uiItem);
            PickController.Instance.PickCharacter(uiItem.Item);
            //if (UIPrepareGameController.Instance.HUDController.Picked.CharacterSlot == null) return; 
            //UIPrepareGameController.Instance.HUDController.Picked.CharacterSlot.Set(uiItem.Item);
        }

    }
}
