using Asce.Game.Players;
using UnityEngine;

namespace Asce.Game.Progress
{
    [CreateAssetMenu(menuName = "Asce/Progress/Unlocks/Character Reaching Levels", fileName = "Character Reaching Levels")]
    public class SO_UnlockWithCharacterReachingLevels : SO_UnlockCondition
    {
        [Header("Character Reaching Levels")]
        [SerializeField] private string _characterName = string.Empty;
        [SerializeField] private int _level = 0;

        public string CharacterName => _characterName;
        public int Level => _level;

        private void Reset()
        {
            _name = "Character Reaching Levels";
        }

        public override bool IsMet()
        {
            if (!Application.isPlaying) return false;
            if (PlayerManager.Instance == null) return false;

            CharacterProgress characterProgress = PlayerManager.Instance.Progress.CharactersProgress.Get(_characterName);
            if (characterProgress == null) return false;

            return characterProgress.Level >= _level;
        }

        public override void Met()
        {
            
        }
    }
}