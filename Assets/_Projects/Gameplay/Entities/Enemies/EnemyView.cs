using Asce.Managers.Utils;
using UnityEngine;

namespace Asce.Game.Entities.Enemies
{
    public class EnemyView : EntityView
    {
        [SerializeField] protected Animator _animator;

        public Animator Animator => _animator;



        protected override void RefReset()
        {
            base.RefReset();
            this.LoadComponent(out  _animator);
        }

        public override void ResetStatus()
        {
            base.ResetStatus();
            if (Animator != null)
            {
                Animator.Rebind();
            }
        }
    }
}
