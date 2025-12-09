using Asce.Core;
using Asce.Core.Attributes;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Asce.Game.Maps
{
    [CreateAssetMenu(menuName = "Asce/Maps/Map Information", fileName = "Map Information")]
    public class SO_MapInformation : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField, TextArea(3, 10)] private string _description;

        [Space]
        [SerializeField, SpritePreview] private Sprite _icon;
        [SerializeField, SpritePreview] private Sprite _fullMap;
        [SerializeField, SpritePreview] private Sprite _thumbnail;

        [Space]
        [SerializeField]
        private ListObjects<int, SO_MapLevelInformation> _levels = new((level) =>
        {
            if (level == null) return 0;
            return level.Level;
        });


        public string Name => _name;
        public string Description => _description;

        public Sprite Icon => _icon;
        public Sprite FullMap => _fullMap;
        public Sprite Thumbnail => _thumbnail;

        public ReadOnlyCollection<SO_MapLevelInformation> Levels => _levels.List;
        public SO_MapLevelInformation GetLevel(int level) => _levels.Get(level);
        public bool IsLevelValid(int level) => this.GetLevel(level) != null;
    }
}
