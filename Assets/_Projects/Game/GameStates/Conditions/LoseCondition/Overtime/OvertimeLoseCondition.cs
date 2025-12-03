using Asce.Managers;
using UnityEngine;

namespace Asce.MainGame.Managers
{
    public class OvertimeLoseCondition : LoseCondition
    {
        [SerializeField] private float _time = 300f;

        public override bool IsSatisfied()
        {
            return MainGameManager.Instance.PlayTimeController.ElapsedTime > _time;
        }
    }
}
