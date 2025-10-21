using Asce.Game.Entities.Machines;
using Asce.Game.SaveLoads;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class HorizonBreaker_Ability : CharacterAbility, IControlMachineAbility
    {
        [SerializeField] private HorizonBreaker_Machine _machine;

        public HorizonBreaker_Machine Machine => _machine;
        Machine IControlMachineAbility.Machine => _machine;


        public override void Initialize()
        {
            base.Initialize();
            Machine.Initialize();
            Machine.OnDead += MachineHealth_OnDead;
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            Machine.ResetStatus();
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

        protected override void OnBeforeSave(AbilitySaveData data)
        {
            base.OnBeforeSave(data);
            MachineSaveData machineData = ((ISaveable<MachineSaveData>)Machine).Save();
            data.SetCustom("Machine", machineData);
        }

        protected override void OnAfterLoad(AbilitySaveData data)
        {
            base.OnAfterLoad(data);
            MachineSaveData machineData = data.GetCustom<MachineSaveData>("Machine");
            ((ILoadable<MachineSaveData>)Machine).Load(machineData);
        }
    }
}
