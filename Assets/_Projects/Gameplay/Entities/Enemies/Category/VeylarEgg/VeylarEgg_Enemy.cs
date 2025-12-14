using Asce.Game.Combats;
using Asce.Game.Levelings;
using Asce.Game.SaveLoads;
using Asce.Core.Utils;
using System;
using System.Collections;
using UnityEngine;
using Asce.Core.Attributes;

namespace Asce.Game.Entities.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class VeylarEgg_Enemy : Enemy
    {
        [Header("Veylar Egg")]
        [SerializeField, Readonly] private Rigidbody2D _rigidbody;
        [SerializeField] private Cooldown _hatchCooldown = new(5f);
        [SerializeField] private string _veylarEnemyName = string.Empty;


        public event Action OnHatched;

        public Rigidbody2D Rigidbody => _rigidbody;
        public Cooldown HatchCooldown => _hatchCooldown;

        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out _rigidbody);
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            Rigidbody.linearVelocity = Vector2.zero;
            _hatchCooldown.Reset();
        }
        protected override void Leveling_OnLevelSetted(int newLevel)
        {
            _hatchCooldown.SetBaseTime(Information.Stats.GetCustomStat("HatchCooldown"));
            base.Leveling_OnLevelSetted(newLevel);
        }

        protected override void LevelTo(int newLevel)
        {
            base.LevelTo(newLevel);
            LevelModificationGroup modificationGroup = Information.Leveling.GetLevelModifications(newLevel);
            if (modificationGroup == null) return;

            if (modificationGroup.TryGetModification("HatchCooldown", out LevelModification hatchCooldownModification))
            {
                _hatchCooldown.BaseTime += hatchCooldownModification.Value;
                _hatchCooldown.Reset();
            }
        }

        protected override void Update()
        {
            base.Update();
            _hatchCooldown.Update(Time.deltaTime);
            if (_hatchCooldown.IsComplete)
            {
                _hatchCooldown.Reset();
                StartCoroutine(this.SpawnVeylar());
            }
        }

        protected override void Attack() { }
        protected override void MoveToTaget() { }

        private IEnumerator SpawnVeylar()
        {
            View.Animator.SetTrigger("Hatch");
            OnHatched?.Invoke();

            yield return new WaitForSeconds(0.5f);
            Veylar_Enemy veylar = EnemyController.Instance.Spawn(_veylarEnemyName, transform.position) as Veylar_Enemy;
            if (veylar == null) yield break;

            yield return new WaitForSeconds(0.2f);
            veylar.Leveling.SetLevel(Leveling.CurrentLevel);
            veylar.Layable = false;
            veylar.gameObject.SetActive(true);

            CombatController.Instance.Killing(this, this);
        }

        protected override void OnBeforeSave(EnemySaveData data)
        {
            data.SetCustom("HatchCooldown", _hatchCooldown.CurrentTime);
        }

        protected override void OnAfterLoad(EnemySaveData data)
        {
            _hatchCooldown.CurrentTime = data.GetCustom("HatchCooldown", 0f);
        }
    }
}