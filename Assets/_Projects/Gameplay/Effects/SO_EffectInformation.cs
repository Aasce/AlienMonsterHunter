using Asce.Core;
using Asce.Core.Attributes;
using UnityEngine;

namespace Asce.Game.Effects
{
    [CreateAssetMenu(menuName = "Asce/Effects/Effect Information", fileName = "Effect Information")]
    public class SO_EffectInformation : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField, TextArea(3, 10)] private string _description;
        [SerializeField, SpritePreview] private Sprite _icon;
        [SerializeField] private EffectType _type;

        [Space]
        [SerializeField] private EffectApplyType _applyType;

        [Space]
        [SerializeField]
        private ListObjects<string, CustomValue> _customValues = new((value) =>
        {
            return value.Name;
        });


        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;
        public EffectType Type => _type;
        
        public EffectApplyType ApplyType => _applyType;

        public float GetCustomValue(string name) => _customValues.Get(name).Value;
    }
}
