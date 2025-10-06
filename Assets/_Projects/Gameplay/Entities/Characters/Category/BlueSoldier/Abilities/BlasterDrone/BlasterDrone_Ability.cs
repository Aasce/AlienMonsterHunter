using Asce.Game.Entities.Machines;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class BlasterDrone_Ability : CharacterAbility
    {
        [SerializeField] private BlasterDrone_Machine _drone;

        public BlasterDrone_Machine Drone => _drone;

        protected override void Start()
        {
            base.Start();
            if (Drone != null)
            {
                Drone.OnTakeDamage += DroneHealth_OnTakeDamage;
            }
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            if (Drone != null)
            {
                Drone.ResetStatus();
                Drone.transform.SetLocalPositionAndRotation(Vector2.zero, Quaternion.identity);
            }
        }

        public override void SetPosition(Vector2 position)
        {
            base.SetPosition(position);
            if (_owner == null) return;

            Vector2 direction = position - (Vector2)_owner.transform.position;
            Drone.MoveDirection = direction;
        }

        private void DroneHealth_OnTakeDamage(float damage)
        {
            if (Drone.Stats.Health.CurrentValue <= 0f) this.DespawnTime.ToComplete();
        }
    }
}
