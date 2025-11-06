using Asce.Game.Players;
using UnityEngine;

namespace Asce.Game.Entities
{
    public static class EntityExtension
    {
        public static bool IsControlByPlayer(this Entity entity)
        {
            if (entity == null) return false;
            if (PlayerManager.Instance.Player is IPlayerControlCharacter playerControlCharacter)
            {
                if (playerControlCharacter.Character == entity) return true;

            }
            return false;
        }
    }
}