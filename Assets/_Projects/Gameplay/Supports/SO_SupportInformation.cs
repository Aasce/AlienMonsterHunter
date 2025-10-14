using Asce.Managers.Attributes;
using UnityEngine;

namespace Asce.Game.Supports
{
    [CreateAssetMenu(menuName = "Asce/Supports/Information", fileName = "Support Information")]
    public class SO_SupportInformation : ScriptableObject
    {
        [SerializeField] private string _id = string.Empty;
        [SerializeField] private string _name = string.Empty;
        [SerializeField, TextArea(3, 10)] private string _description = string.Empty;
        [SerializeField, SpritePreview] private Sprite _icon;

        [Space]
        [SerializeField, Min(0f)] private float _cooldown = 0f;
        [SerializeField, Min(0f)] private float _cooldownOnRecall = 0f;

        public string Id => _id;
        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;
        public float Cooldown => _cooldown;
        public float CooldownOnRecall => _cooldownOnRecall;
    }
}
