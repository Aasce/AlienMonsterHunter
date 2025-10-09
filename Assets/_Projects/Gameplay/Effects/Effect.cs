using Asce.Game.Entities;
using Asce.Managers;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Effects
{
    public abstract class Effect : GameComponent
    {
        [SerializeField] protected SO_EffectInformation _information;
        [SerializeField] protected Entity _receiver;
        [SerializeField] protected Cooldown _duration = new();
        [SerializeField] protected float _strength;


        public SO_EffectInformation Information => _information;

        public Entity Receiver
        {
            get => _receiver;
            set => _receiver = value;
        }
        public Cooldown Duration => _duration;
        public float Strength
        {
            get => _strength;
            set => _strength = value;
        }

        public virtual void ResetStatus() { }
        public virtual void Initialize() { }

        public abstract void Apply();
        public abstract void Unpply();

        public virtual void SetData(EffectData data)
        {
            Duration.SetBaseTime(data.Duration);
            Strength = data.Strength;
        }
    }
}
