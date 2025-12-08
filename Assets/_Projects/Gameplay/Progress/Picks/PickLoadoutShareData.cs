using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Progress
{
    [System.Serializable]
    public class PickLoadoutShareData
    {
        [SerializeField] private string _characterName;
        [SerializeField] private string _gunName;
        [SerializeField] private List<string> _supportNames = new();

        public string CharacterName
        {
            get => _characterName;
            set => _characterName = value;
        }

        public string GunName
        {
            get => _gunName;
            set => _gunName = value;
        }

        public List<string> SupportNames => _supportNames;
    }
}