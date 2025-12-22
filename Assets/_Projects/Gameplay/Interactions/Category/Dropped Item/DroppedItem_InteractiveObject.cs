using Asce.Core.Attributes;
using Asce.Core.Utils;
using Asce.Game.Entities.Characters;
using Asce.Game.Items;
using Asce.Game.Managers;
using Asce.Game.SaveLoads;
using UnityEngine;

namespace Asce.Game.Interactions
{
    public class DroppedItem_InteractiveObject : InteractiveObject
    {
        [Header("References")]
        [SerializeField, Readonly] private CircleCollider2D _collider;
        [SerializeField, Readonly] private Rigidbody2D _rigidbody;
        [SerializeField, Readonly] private SpriteRenderer _iconRenderer;

        [Space]
        [SerializeField] private Cooldown _despawnCooldown = new(60f);
        [SerializeField] private LayerMask _interactLayer;
        [SerializeField] private Vector2 _forceRange = new Vector2(3f, 5f);

        [Header("Runtime")]
        [SerializeField, Readonly] private Item _item;
        private bool _isValid = true;

        public CircleCollider2D Collider => _collider;
        public Rigidbody2D Rigidbody => _rigidbody;
        public SpriteRenderer IconRenderer => _iconRenderer;
        public Item Item => _item;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _collider);
            this.LoadComponent(out _rigidbody);
        }

        public override void OnSpawn()
        {
            base.OnSpawn();
            _rigidbody.linearVelocity = Vector2.zero;
            _item = null;
            _isValid = true;
            _despawnCooldown.Reset();
        }

        public override void OnActive()
        {
            base.OnActive();
            IconRenderer.sprite = _item != null ? _item.Information.Icon : null;
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
            if (_item == null)
            {
                _isValid = false;
                return;
            }

            if (!LayerUtils.IsInLayerMask(collision.gameObject.layer, _interactLayer)) return;
            if (!collision.TryGetComponent(out Character character)) return;

            SpoilsController.Instance.AddSpoil(_item.Information.Name, _item.Quantity);
            _isValid = false;
        }

        public override void Interact(GameObject interacter)
        {

        }

        public void SetItem(Item item) => _item = item;
        public void SetItem(SO_ItemInformation itemInformation, int quantity = 1)
        {
            if (itemInformation == null) return;
            _item = new Item(itemInformation)
            {
                Quantity = quantity
            };
        }
        public void SetItem(string name, int quantity = 1)
        {
            SO_ItemInformation itemInformation = GameManager.Instance.AllItems.Get(name);
            this.SetItem(itemInformation, quantity);
        }


        public void Force()
        {
            _rigidbody.AddForce(Random.insideUnitCircle.normalized * Random.Range(_forceRange.x, _forceRange.y), ForceMode2D.Impulse);
        }

        protected override void OnBeforeSave(InteractiveObjectSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("DespawnCooldown", _despawnCooldown.CurrentTime);
        }

        protected override void OnAfterLoad(InteractiveObjectSaveData data)
        {
            base.OnAfterLoad(data);
            _despawnCooldown.CurrentTime = data.GetCustom<float>("DespawnCooldown");
        }
    }
}
