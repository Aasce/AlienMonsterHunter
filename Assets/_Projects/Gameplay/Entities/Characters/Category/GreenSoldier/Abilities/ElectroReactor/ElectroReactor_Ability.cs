using Asce.Game.Entities.Machines;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class ElectroReactor_Ability : CharacterAbility
    {
        [SerializeField] private ElectroReactor_Machine _reactor;

        public ElectroReactor_Machine Reactor => _reactor;

        protected override void Start()
        {
            base.Start();
            if (Reactor != null)
            {
                Reactor.Initialize();
                Reactor.OnTakeDamage += TurretHealth_OnTakeDamage;
            }
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            if (Reactor != null) Reactor.ResetStatus();
        }

        public override void SetPosition(Vector2 position)
        {
            base.SetPosition(position);
        }

        private void TurretHealth_OnTakeDamage(float damage)
        {
            if (Reactor.Stats.Health.CurrentValue <= 0f) this.DespawnTime.ToComplete();
        }
    }
}
