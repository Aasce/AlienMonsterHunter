using Asce.Game.Entities.Machines;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class BlueSoldier_Turret_Ability : CharacterAbility
    {
        [SerializeField] private BlueSoldier_Turret_Entity _turret;

        public BlueSoldier_Turret_Entity Turret => _turret;

        private void Start()
        {
            if (Turret != null)
            {
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
