using Asce.Game.Entities.Machines;
using System;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class GreenSoldier_PlazmaTurret_Ability : CharacterAbility
    {
        [SerializeField] private GreenSoldier_PlazmaTurret_Entity _turret;

        public GreenSoldier_PlazmaTurret_Entity Turret => _turret;

        private void Start()
        {
            if (Turret != null)
            {
                Turret.OnTakeDamage += TurretHealth_OnTakeDamage;
                Turret.OnCurrentAmmoChanged += Turret_OnCurrentAmmoChanged;
            }
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            if (Turret != null)
            {
                Turret.ResetStatus();
            }
        }

        public override void SetPosition(Vector2 position)
        {
            base.SetPosition(position);
            if (_owner == null) return;

            Vector2 direction = position - (Vector2)_owner.transform.position;
            Turret.Rotate(direction);
        }

        private void TurretHealth_OnTakeDamage(float damage)
        {
            if (Turret.Stats.Health.CurrentValue <= 0f) this.DespawnTime.ToComplete();
        }

        private void Turret_OnCurrentAmmoChanged(int newValue)
        {
            if (newValue <= 0)
            {
                this.DespawnTime.CurrentTime = Mathf.Min(this.DespawnTime.CurrentTime, 5f);
            }
        }

    }
}
