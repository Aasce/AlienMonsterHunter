using System;

namespace Asce.Game.Abilities
{
    public interface ITrapAbility
    {
        public event Action OnCatched;

        public void Catching() { }
    }
}