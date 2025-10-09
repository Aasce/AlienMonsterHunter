using Asce.Managers.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class VeylarEgg_Enemy : Enemy
    {
        [Header("Veylar Egg")]
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Cooldown _hatchCooldown = new(5f);
        [SerializeField] private string _veylarEnemyName = string.Empty;

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

        protected override void Attack()
        {

        }

        protected override void MoveToTaget()
        {

        }

        private IEnumerator SpawnVeylar()
        {
            if (View != null && View.Animator != null)
            {
                View.Animator.SetTrigger("Hatch");
            }


            yield return new WaitForSeconds(0.5f);
            Veylar_Enemy veylar = EnemyController.Instance.Spawn(_veylarEnemyName, transform.position) as Veylar_Enemy;
            if (veylar == null) yield break;

            yield return new WaitForSeconds(0.5f);
            veylar.Layable = false;
            EnemyController.Instance.Despawn(this);
        }

    }
}