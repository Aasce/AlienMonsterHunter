using Asce.Managers.Attributes;
using UnityEngine;

namespace Asce.Game.Items
{
    [CreateAssetMenu(menuName = "Asce/Items/Item Information", fileName = "Item Information")]
    public class SO_ItemInformation : ScriptableObject
    {
        [SerializeField] private string _name = "New Item";
        [SerializeField, TextArea(3, 10)] private string _description = "Item Description";
        [SerializeField, SpritePreview] private Sprite _icon = null;

        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;
    }
}