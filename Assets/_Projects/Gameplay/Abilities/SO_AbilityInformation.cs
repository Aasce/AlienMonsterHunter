using Asce.Managers.Attributes;
using UnityEngine;

namespace Asce.Game.Abilities
{
    [CreateAssetMenu(menuName = "Asce/Abilities/Information", fileName = "Ability Information")]
    public class SO_AbilityInformation : ScriptableObject
    {
        [SerializeField] private string _id;
        [SerializeField] private string _name;
        [SerializeField, TextArea(3, 10)] private string _description;
        [SerializeField, SpritePreview] private Sprite _icon;

        [Space]
        [SerializeField] private bool _isPassive = false;
        [SerializeField, Min(0f)] private float _cooldown = 0f;

        public string Id => _id;
        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;

        public bool IsPassive => _isPassive;
        public bool IsActive => !_isPassive;
        public float Cooldown => _cooldown;

        private void OnValidate()
        {
            if (string.IsNullOrEmpty(Id)) return;
            if (string.IsNullOrWhiteSpace(Name)) _name = Id;
        }
    }

}