using UnityEngine;

namespace Asce.Game.Effects
{
    [System.Serializable]
    public struct EffectData
    {
        [SerializeField] private float _duration;
        [SerializeField] private float _strength;
        [SerializeField] private int _stack;

        public float Duration
        {
            readonly get => _duration;
            set => _duration = value;
        }

        public float Strength
        {
            readonly get => _strength;
            set => _strength = value;
        }

        public int Stack
        {
            readonly get => _stack;
            set => _stack = value;
        }
    }
}