using Asce.Game.Players;
using UnityEngine;

namespace Asce.Game.Progress
{
    [CreateAssetMenu(menuName = "Asce/Progress/Unlocks/First Game Start", fileName = "First Game Start")]
    public class SO_UnlockFirstGameStart : SO_UnlockCondition
    {
        private void Reset()
        {
            _name = "First Game Start";
        }

        public override bool IsMet()
        {
            if (!Application.isPlaying) return false;
            if (PlayerManager.Instance == null) return false;

            return PlayerManager.Instance.Progress.GameProgress.OpenGamesCount <= 1;
        }

        public override void Met()
        {

        }
    }
}