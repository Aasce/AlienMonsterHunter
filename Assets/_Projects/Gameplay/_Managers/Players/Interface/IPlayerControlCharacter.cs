using Asce.Game.Entities.Characters;
using Asce.Core;
using System;
using UnityEngine;

namespace Asce.Game.Players
{
    public interface IPlayerControlCharacter
    {
        public event Action<ValueChangedEventArgs<Character>> OnCharacterChanged;

        public Character Character { get; set; }
    }
}