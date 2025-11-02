using Asce.Game.Entities.Characters;
using Asce.Game.Entities.Machines;
using Asce.Game.SaveLoads;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class AerialVanguard_Ability : CharacterAbility, IControlMachineAbility
    {
        [SerializeField] private AerialVanguard_Machine _machine;

        public AerialVanguard_Machine Machine => _machine;
        Machine IControlMachineAbility.Machine => _machine;


        public override void Initialize()
        {
            base.Initialize();
            Machine.Initialize();
            Machine.OnDead += MachineHealth_OnDead;
            Leveling.OnLevelChanged += Leveling_OnLevelChanged;
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            Machine.ResetStatus();
        }

        public override void OnActive()
        {
            base.OnActive();
            Machine.Owner = Owner.GetComponent<Character>();
        }

        public override void SetPosition(Vector2 position)
        {
            Machine.Agent.Warp(FindValidPosition(position));
        }

        private void MachineHealth_OnDead(Combats.DamageContainer container)
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
