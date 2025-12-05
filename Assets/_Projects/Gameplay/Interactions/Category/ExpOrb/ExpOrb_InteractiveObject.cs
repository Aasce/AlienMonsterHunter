using Asce.Game.Entities.Characters;
using Asce.Game.SaveLoads;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using UnityEngine;

namespace Asce.Game.Interactions
{
    public class ExpOrb_InteractiveObject : InteractiveObject
    {
        [Header("References")]
        [SerializeField, Readonly] private CircleCollider2D _collider;
        [SerializeField, Readonly] private Rigidbody2D _rigidbody;

        [Space]
        [SerializeField] private Cooldown _despawnCooldown = new(60f);
        [SerializeField] private LayerMask _interactLayer;
        private bool _isValid = true;


        [Space]
        [SerializeField, Min(0)] private int _expAmount = 0;

        public Rigidbody2D Rigidbody => _rigidbody;
        public int ExpAmount
        {
            get => _expAmount;
            set => _expAmount = Mathf.Max(0, value);
        }

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _collider);
            this.LoadComponent(out _rigidbody);
        }

        public override void OnActive()
        {
            base.OnActive();
            _isValid = true;
            _despawnCooldown.Reset();
        }

        private void Update()
        {
            if (!_isValid) _despawnCooldown.ToComplete();
            else _despawnCooldown.Update(Time.deltaTime);
            
            if (_despawnCooldown.IsComplete)
            {
                InteractionController.Instance.Despawn(this);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!_isValid) return;
            if (!LayerUtils.IsInLayerMask(collision.gameObject.layer, _interactLayer)) return;
            if (!collision.TryGetComponent(out Character character)) return;

            character.Leveling.AddExp(ExpAmount);

            _isValid = false;
        }

        public override void Interact(GameObject interacter)
        {

        }

        protected override void OnBeforeSave(InteractiveObjectSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("ExpAmount", ExpAmount);
            data.SetCustom("DespawnCooldown", _despawnCooldown.CurrentTime);
        }

        protected override void OnAfterLoad(InteractiveObjectSaveData data)
        {
            base.OnAfterLoad(data);
            ExpAmount = data.GetCustom<int>("ExpAmount");
            _despawnCooldown.CurrentTime = data.GetCustom<float>("DespawnCooldown");
        }
    }
}