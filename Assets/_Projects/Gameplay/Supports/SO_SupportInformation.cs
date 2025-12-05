using Asce.Game.Levelings;
using Asce.Game.Progress;
using Asce.Core.Attributes;
using UnityEngine;

namespace Asce.Game.Supports
{
    [CreateAssetMenu(menuName = "Asce/Supports/Information", fileName = "Support Information")]
    public class SO_SupportInformation : ScriptableObject
    {
        [SerializeField] private string _key = string.Empty;
        [SerializeField] private string _name = string.Empty;
        [SerializeField, TextArea(3, 10)] private string _description = string.Empty;
        [SerializeField, SpritePreview] private Sprite _icon;

        [Header("References")]
        [SerializeField] protected SO_LevelingInformation _leveling;
        [SerializeField] private SO_ProgressInformation _progress;

        [Space]
        [SerializeField, Min(0f)] private float _cooldown = 0f;
        [SerializeField, Min(0f)] private float _cooldownOnRecall = 0f;

        public string Key => _key;
        public string Name => _name;
        public string Description => _description;
        public Sprite Icon => _icon;
        public SO_LevelingInformation Leveling => _leveling;
        public SO_ProgressInformation Progress => _progress;

        public float Cooldown => _cooldown;
        public float CooldownOnRecall => _cooldownOnRecall;
    }
}
