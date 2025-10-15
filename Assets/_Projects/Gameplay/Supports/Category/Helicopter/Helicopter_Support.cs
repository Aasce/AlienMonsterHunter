using Asce.Game.Abilities;
using Asce.Game.Entities.Machines;
using Asce.Game.Enviroments;
using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Supports
{
    public class HelicopterSupport : Support, IControlMachineAbility
    {
        [SerializeField] private Helicopter_Machine _machine;

        public Helicopter_Machine Machine => _machine;
        Machine IControlMachineAbility.Machine => _machine;

        protected override void Start()
        {
            base.Start();

            if (Machine == null)
            {
                Debug.LogWarning($"{nameof(HelicopterSupport)} has no assigned machine.");
                return;
            }

            Machine.Initialize();
            Machine.OnDead += Machine_OnDead;
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            Machine?.ResetStatus();
        }

        public override void OnActive()
        {
            base.OnActive();

            if (Machine == null)
                return;

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

        private void Update()
        {
            Bounds mapBounds = EnviromentController.Instance.MapBounds;
            mapBounds.Expand(10f);

            // If machine is outside of map bounds, despawn safely
            if (!mapBounds.Contains(Machine.transform.position))
            {
                SupportController.Instance.Despawn(this);
            }
        }

        public override void Recall()
        {
            base.Recall();
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

        private void Machine_OnDead()
        {
            SupportController.Instance.Despawn(this);
        }
    }
}
