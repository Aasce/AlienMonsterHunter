using Asce.Game.Entities.Machines;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class HorizonBreaker_Ability : CharacterAbility
    {
        [SerializeField] private HorizonBreaker_Machine _machine;

        public HorizonBreaker_Machine Machine => _machine;


        protected override void Start()
        {
            base.Start();
            if (Machine != null)
            {
                Machine.Initialize();
                Machine.OnDead += MachineHealth_OnDead;
            }
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            if (Machine != null) Machine.ResetStatus();
        }

        public override void SetPosition(Vector2 position)
        {
            base.SetPosition(position);

            Vector2 direction = position - (Vector2)_owner.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Machine.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        private void MachineHealth_OnDead()
        {
            this.DespawnTime.ToComplete();
        }
    }
}
