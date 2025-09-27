using Asce.Game.Entities.Enemies;
using Asce.Game.Stats;
using UnityEngine;

namespace Asce.Game.Guns
{
    public class DefaultGun : Gun
    {
        [SerializeField] private float _distance = 20f;
        [SerializeField] private LayerMask _hitLayer;


        protected override void Shooting(Vector2 direction)
        {
            RaycastHit2D hit = Physics2D.Raycast(BarrelPosition, direction, _distance, _hitLayer);
            if (hit.collider == null) return;
            if (!hit.transform.TryGetComponent(out Enemy enemy)) return;
            
            CombatController.Instance.DamageDealing(enemy, Damage);
        }
    }
}
