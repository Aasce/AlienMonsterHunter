using Asce.Managers.Attributes;
using UnityEngine;

namespace Asce.Game.Entities
{
    [CreateAssetMenu(menuName = "Asce/Entities/Information", fileName = "Entity Information")]
    public class SO_EntityInformation : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField, TextArea(3, 10)] private string _description;
        [SerializeField, SpritePreview] private Sprite _icon;

        [Space]
        [SerializeField] protected SO_EntityStats _stats;

        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;

        public SO_EntityStats Stats => _stats;
    }
}