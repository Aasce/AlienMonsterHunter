using Asce.Game.Entities.Machines;
using Asce.Game.SaveLoads;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class OblivionTurret_Ability : CharacterAbility, IControlMachineAbility
    {
        [SerializeField] private OblivionTurret_Machine _machine;

        public OblivionTurret_Machine Machine => _machine;
        Machine IControlMachineAbility.Machine => _machine;

        public override void Initialize()
        {
            base.Initialize();
            Machine.Initialize();
            Machine.OnDead += Machine_OnDead;
            Leveling.OnLevelChanged += Leveling_OnLevelChanged;
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            Machine.ResetStatus();
        }

        public override void SetPosition(Vector2 position)
        {
            base.SetPosition(position);
            if (_owner == null) return;

            Vector2 direction = position - (Vector2)_owner.transform.position;
            Machine.Rotate(direction);
        }

        private void Machine_OnDead(Combats.DamageContainer container)
        {
            this.DespawnTime.ToComplete();
        }

        private void Leveling_OnLevelChanged(int newLevel)
        {
            Machine.Leveling.SetLevel(newLevel);
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
