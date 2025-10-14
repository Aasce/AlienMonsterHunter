using Asce.Game.Players;
using Asce.PrepareGame.Players;
using UnityEngine;

namespace Asce.Game.Entities
{
    public static class EntityExtension
    {
        public static bool IsControlByPlayer(this Entity entity)
        {
            if (entity == null) return false;
            if (Player.HasInstance)
            {
                if (Player.Instance.Character == entity) return true;
            }
            if (PrepareGamePlayer.HasInstance)
            {
                if (PrepareGamePlayer.Instance.Character == entity) return true;
            }
            return false;
        }
    }
}