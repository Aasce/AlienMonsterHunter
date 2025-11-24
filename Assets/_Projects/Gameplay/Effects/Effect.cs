using Asce.Game.Entities;
using Asce.Game.SaveLoads;
using Asce.Managers;
using Asce.Managers.Attributes;
using Asce.Managers.Utils;
using Asce.SaveLoads;
using System;
using System.Collections;
using UnityEngine;

namespace Asce.Game.Effects
{
    public abstract class Effect : GameComponent, IIdentifiable, ISaveable<EffectSaveData>, ILoadable<EffectSaveData>
    {
        public const string PREFIX_ID = "effect";
        [SerializeField, Readonly] private string _id = string.Empty;
        
        [Space]
        [SerializeField] protected SO_EffectInformation _information;
        [SerializeField, Readonly] protected Entity _sender;
        [SerializeField, Readonly] protected Entity _receiver;
        [SerializeField, Readonly] protected Cooldown _duration = new();
        [SerializeField, Readonly] protected float _strength;
        [SerializeField, Readonly] protected int _stack = 1;

        public event Action OnApplied;
        public event Action OnUnapplied;
        public event Action OnStacked;

        public string Id => _id;
        public SO_EffectInformation Information => _information;

        public Entity Sender
        {
            get => _sender;
            set => _sender = value;
        }
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
        public int Stack
        {
            get => _stack;
            set => _stack = value;
        }
        string IIdentifiable.Id
        {
            get => Id;
            set => _id = value;
        }

        public virtual void Initialize() 
        {
            if (string.IsNullOrEmpty(_id)) _id = IdGenerator.NewId(PREFIX_ID);
        }

        public virtual void ResetStatus() 
        {
            Stack = 1;
        }

        public virtual void Apply()
        {
            this.InternalApply();
            OnApplied?.Invoke();
        }

        public virtual void Reapply(EffectData data)
        {
            float largestDuration = Mathf.Max(Duration.BaseTime, data.Duration);
            Duration.SetBaseTime(largestDuration);

            float largestStrength = Mathf.Max(Strength, data.Strength);
            Strength = largestStrength;
        }

        public virtual void Unapply()
        {
            this.InternalUnapply();
            OnUnapplied?.Invoke();
        }

        public virtual void ApplyStack(EffectData data)
        {
            Stack++;
            OnStacked?.Invoke();
        }


        protected abstract void InternalApply();
        protected abstract void InternalUnapply();

        public virtual void SetData(EffectData data)
        {
            Duration.SetBaseTime(data.Duration);
            Strength = data.Strength;
        }

        EffectSaveData ISaveable<EffectSaveData>.Save()
        {
            EffectSaveData effectData = new()
            {
                id = _id,
                name = Information.Name,
                senderId = Sender != null ? Sender.Id : string.Empty,
                baseDuration = Duration.BaseTime,
                duration = Duration.CurrentTime,
                strength = Strength,
                stack = Stack,
            };

            this.OnBeforeSave(effectData);
            return effectData;
        }

        void ILoadable<EffectSaveData>.Load(EffectSaveData data)
        {
            if (data == null) return;
            _id = data.id;
            Duration.BaseTime = data.baseDuration;
            Duration.CurrentTime = data.duration;
            Strength = data.strength;
            Stack = data.stack;
            StartCoroutine(LoadSender(data));

            this.OnAfterLoad(data);

            IEnumerator LoadSender(EffectSaveData data)
            {
                yield return null;
                _sender = ComponentUtils.FindComponentById<Entity>(data.senderId);
            }
        }

        protected virtual void OnBeforeSave(EffectSaveData data) { }
        protected virtual void OnAfterLoad(EffectSaveData data) { }
    }
}
