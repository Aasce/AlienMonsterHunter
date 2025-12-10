using Asce.Core.Attributes;
using UnityEngine;

namespace Asce.Game.Items
{
    [CreateAssetMenu(menuName = "Asce/Items/Item Information", fileName = "Item Information")]
    public class SO_ItemInformation : ScriptableObject
    {
        [SerializeField] private string _name = "New Item";
        [SerializeField, TextArea(3, 10)] private string _description = "Item Description";
        [SerializeField, SpritePreview] private Sprite _icon = null;

        [Space]
        [SerializeField] private ItemType _type = ItemType.Default;

        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;

        public ItemType Type => _type;
    }
}