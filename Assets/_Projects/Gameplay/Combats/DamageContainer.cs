using UnityEngine;

namespace Asce.Game.Combats
{
    [System.Serializable]
    public class DamageContainer
    {
        private ISendDamageable _sender;
        private ITakeDamageable _receiver;

        [SerializeField] private float _damage = 0f;
        [SerializeField] private float _penetration = 0f;
        [SerializeField] private DamageType _damageType = DamageType.Physics;

        [Space]
        [SerializeField] private float _finalDamage = 0f;

        public ISendDamageable Sender
        {
            get => _sender;
            set => _sender = value;
        }

        public ITakeDamageable Receiver 
        { 
            get => _receiver; 
            set => _receiver = value; 
        }

        public float Damage 
        { 
            get => _damage; 
            set => _damage = value; 
        }

        public float Penetration 
        { 
            get => _penetration; 
            set => _penetration = value; 
        }

        public DamageType DamageType 
        { 
            get => _damageType; 
            set => _damageType = value; 
        }

        public float FinalDamage
        {
            get => _finalDamage;
            set => _finalDamage = value;
        }

        public DamageContainer() { }
        public DamageContainer(ISendDamageable sender, ITakeDamageable receiver)
        {
            Sender = sender;
            Receiver = receiver;
        }
    }
}