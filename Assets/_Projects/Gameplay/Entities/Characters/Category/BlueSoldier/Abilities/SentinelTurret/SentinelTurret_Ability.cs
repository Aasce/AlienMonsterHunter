using Asce.Game.Entities.Machines;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class SentinelTurret_Ability : CharacterAbility
    {
        [SerializeField] private SentinelTurret_Machine _turret;

        public SentinelTurret_Machine Turret => _turret;

        protected override void Start()
        {
            base.Start();
            if (Turret != null)
            {
                Turret.Initialize();
                Turret.OnTakeDamage += TurretHealth_OnTakeDamage;
            }
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            if (Turret != null) Turret.ResetStatus();
        }

        private void TurretHealth_OnTakeDamage(float damage)
        {
            if (Turret.Stats.Health.CurrentValue <= 0f) this.DespawnTime.ToComplete();
        }
    }
}
