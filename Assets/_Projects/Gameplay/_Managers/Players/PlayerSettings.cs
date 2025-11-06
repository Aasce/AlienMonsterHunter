using Asce.Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Players
{
    public class PlayerSettings : GameComponent
    {
        [SerializeField] private KeyCode _reloadKey = KeyCode.R;
        [SerializeField] private KeyCode _interactKey = KeyCode.F;


        [SerializeField] private List<KeyCode> _callSupportKeys = new()
        {
            KeyCode.Z,
            KeyCode.X,
        };

        [SerializeField] private List<KeyCode> _useAbilityKeys = new()
        {
            KeyCode.Q,
            KeyCode.E,
            KeyCode.C,
        };

        public KeyCode ReloadKey => _reloadKey;
        public KeyCode InteractKey => _interactKey;

        public List<KeyCode> CallSupportKeys => _callSupportKeys;
        public List<KeyCode> UseAbilityKeys => _useAbilityKeys;


        public Vector2 MoveInput => new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;

    }
}