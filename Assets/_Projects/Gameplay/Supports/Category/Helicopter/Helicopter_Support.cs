using Asce.Game.Abilities;
using Asce.Game.Entities.Machines;
using Asce.Game.Enviroments;
using Asce.Game.SaveLoads;
using Asce.Managers.Utils;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.Supports
{
    public class HelicopterSupport : Support, IControlMachineAbility
    {
        [SerializeField] private Helicopter_Machine _machine;
        Bounds _mapBounds;

        public Helicopter_Machine Machine => _machine;
        Machine IControlMachineAbility.Machine => _machine;

        public override void Initialize()
        {
            base.Initialize();

            _mapBounds = EnviromentController.Instance.MapBounds;
            _mapBounds.Expand(10f);

            Machine.Initialize();
            Machine.OnDead += Machine_OnDead;
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            Machine.ResetStatus();
        }

        public override void OnActive()
        {
            base.OnActive();
            this.SetMachinePositionAndDirection();
        }

        public override void Recall()
        {
            base.Recall();
            this.SetMachinePositionAndDirection();
        }

        public override void OnLoad()
        {
            base.OnLoad();
        }

        private void Update()
        {
            // If machine is outside of map bounds, despawn safely
            if (!_mapBounds.Contains(Machine.transform.position))
            {
                SupportController.Instance.Despawn(this);
            }
        }

        private void SetMachinePositionAndDirection()
        {
            Vector2 direction = CallPosition - (Vector2)transform.position;
            Machine.Direction = direction;

            Bounds mapBounds = EnviromentController.Instance.MapBounds;
            if (Vector2Utils.BoundsIntersection(CallPosition, -direction, mapBounds, out Vector2 position))
            {
                Machine.transform.position = position;
            }
            else
            {
                Machine.transform.position = transform.position;
            }
        }

        private void Machine_OnDead(Combats.DamageContainer container)
        {
            SupportController.Instance.Despawn(this);
        }

        protected override void OnBeforeSave(SupportSaveData data)
        {
            base.OnBeforeSave(data);
            MachineSaveData machineData = (Machine as ISaveable<MachineSaveData>).Save();
            data.SetCustom("Machine", machineData);
        }

        protected override void OnAfterLoad(SupportSaveData data)
        {
            base.OnAfterLoad(data);
            if (data == null) return;
            MachineSaveData machineData = data.GetCustom<MachineSaveData>("Machine");
            ((ILoadable<MachineSaveData>)Machine).Load(machineData);
        }
    }
}
