using Asce.Game.Entities;
using Asce.Game.Entities.Enemies;
using Asce.Game.Stats;
using System.Collections;
using UnityEngine;

namespace Asce.Game.Abilities
{
    public class VeylarEggs_Abiliity : Ability
    {
        [SerializeField] private string _veylarEggEnemyName = string.Empty;

        [Space]
        [SerializeField, Min(0)] private int _eggCount = 3;
        [SerializeField, Min(0)] private float _eggForce = 300f;
        [SerializeField, Min(0)] private float _hatchTime = 5f;


        public int EggCount
        {
            get => _eggCount;
            set => _eggCount = value;
        }

        public override void OnActive()
        {
            base.OnActive();
            StartCoroutine(this.SpawnEggs());
        }

        private IEnumerator SpawnEggs()
        {
            if (_eggCount <= 0) yield break;
            yield return new WaitForSeconds(0.1f);
            for (int i = 0; i < _eggCount; i++)
            {
                VeylarEgg_Enemy egg = EnemyController.Instance.Spawn(_veylarEggEnemyName, transform.position) as VeylarEgg_Enemy;
                if (egg == null) continue;

                Vector2 forceDirection = Random.insideUnitCircle.normalized;
                egg.Rigidbody.AddForce(forceDirection * _eggForce, ForceMode2D.Impulse);
                egg.HatchCooldown.SetBaseTime(Random.Range(_hatchTime * 0.75f, _hatchTime * 1.25f));
                yield return new WaitForSeconds(0.1f);
            }
        }

    }
}