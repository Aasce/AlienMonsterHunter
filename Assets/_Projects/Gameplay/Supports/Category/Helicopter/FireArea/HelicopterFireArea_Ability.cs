using Asce.Game.Effects;
using Asce.Game.Entities;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Core.Attributes;
using Asce.Core.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class HelicopterFireArea_Ability : Ability
    {
        private readonly HashSet<ITargetable> _targets = new();
        [SerializeField] private LayerMask _targetLayer;
        [SerializeField, Readonly] private Cooldown _updateCooldown = new(0.5f);

        [SerializeField, Readonly] private float _igniteDamage = 5f;
        private Entity _ownerEntity;

        public override void Initialize()
        {
            base.Initialize();
            _igniteDamage = Information.GetCustomValue("IgniteDamage");
        }

        public override void OnActive()
        {
            base.OnActive();
            _ownerEntity = Owner != null ? Owner.GetComponent<Entity>() : null;
        }

        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            _igniteDamage = Information.GetCustomValue("IgniteDamage");
            base.Leveling_OnLevelSetted(newLevel);
        }

        protected override void LevelTo(int newLevel)
        {
            base.LevelTo(newLevel);
            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

            if (modificationGroup.TryGetModification("IgniteDamage", out LevelModification igniteDamageModification))
            {
                _igniteDamage += igniteDamageModification.Value;
            }
        }

        private void Update()
        {
            _updateCooldown.Update(Time.deltaTime);
            if (_updateCooldown.IsComplete)
            {
                _updateCooldown.Reset();
                UpdateEffects();
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.enabled) return;
            if (collision.gameObject == Owner) return;
            if (!LayerUtils.IsInLayerMask(collision, _targetLayer)) return;
            if (!collision.TryGetComponent(out ITargetable target)) return;
            if (_targets.Contains(target)) return;

            _targets.Add(target);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (!collision.enabled) return;
            if (collision.gameObject == Owner) return;
            if (!LayerUtils.IsInLayerMask(collision.gameObject.layer, _targetLayer)) return;
            if (!collision.TryGetComponent(out ITargetable target)) return;

            _targets.Remove(target);
        }

        public override void Despawn()
        {
            base.Despawn();
            foreach (ITargetable target in _targets)
            {
                if (target is not Entity entity) continue;
                Effect effect = entity.Effects.Get("Helicopter Ignite");
                EffectController.Instance.RemoveEffect(effect);
            }
        }

        private void UpdateEffects()
        {
            if (DespawnTime.CurrentTime <= 0.1f) return;
            foreach (ITargetable target in _targets)
            {
                if (target is not Entity entity) continue;
                Effect effect = entity.Effects.Get("Helicopter Ignite");
                if (!target.IsTargetable)
                {
                    EffectController.Instance.RemoveEffect(effect);
                    continue;
                }
                
                EffectController.Instance.AddEffect("Helicopter Ignite", _ownerEntity, entity, new EffectData()
                {
                    Strength = _igniteDamage,
                    Duration = DespawnTime.CurrentTime,
                });
            }
        }


        protected override void OnBeforeSave(AbilitySaveData data)
        {
            base.OnBeforeSave(data);

            data.SetCustom("IgniteDamage", _igniteDamage);
        }

        protected override void OnAfterLoad(AbilitySaveData data)
        {
            base.OnAfterLoad(data);
            if (data == null) return;
            _igniteDamage = data.GetCustom<float>("IgniteDamage");
        }

        protected override IEnumerator LoadOwner(AbilitySaveData data)
        {
            yield return base.LoadOwner(data);
            _ownerEntity = Owner != null ? Owner.GetComponent<Entity>() : null;
        }
    }
}
