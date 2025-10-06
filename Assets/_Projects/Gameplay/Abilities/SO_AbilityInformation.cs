using Asce.Managers;
using Asce.Managers.Attributes;
using System.Collections.ObjectModel;
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
        [SerializeField, Min(0f)] private float _useRangeRadius = float.PositiveInfinity;
        [SerializeField, Min(0f)] private float _despawnTime = 0f;

        [Space]
        [SerializeField]
        private ListObjects<string, CustomValue> _customValues = new((custom) =>
        {
            return custom.Name;
        });


        public string Id => _id;
        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;

        public bool IsPassive => _isPassive;
        public bool IsActive => !_isPassive;
        public float Cooldown => _cooldown;
        public float UseRangeRadius => _useRangeRadius;
        public float DaspawnTime => _despawnTime;

        public ReadOnlyCollection<CustomValue> Customs => _customValues.List;
        public float GetCustomValue(string name)
        {
            if (_customValues.TryGet(name, out CustomValue customValue))
            {
                return customValue.Value;
            }

            Debug.LogWarning($"Custom Value \"{name}\" not found.", this);
            return 0f;
        }


        private void OnValidate()
        {
            if (string.IsNullOrEmpty(Id)) return;
            if (string.IsNullOrWhiteSpace(Name)) _name = Id;
        }
    }

}