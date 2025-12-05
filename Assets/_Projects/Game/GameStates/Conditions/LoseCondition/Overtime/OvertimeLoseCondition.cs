using Asce.Core;
using Asce.Game.Managers;
using Asce.Game.SaveLoads;
using UnityEngine;

namespace Asce.MainGame.Managers
{
    public class OvertimeLoseCondition : LoseCondition
    {
        [SerializeField] private float _time = 300f;

        public override string ConditionName => "Overtime";


        public override bool IsSatisfied()
        {
            return MainGameManager.Instance.PlayTimeController.ElapsedTime > _time;
        }

        protected override void OnBeforeSave(GameStateConditionSaveData data)
        {
            base.OnBeforeSave(data);
            data.SetCustom("Time", _time);
        }

        protected override void OnAfterLoad(GameStateConditionSaveData data)
        {
            base.OnAfterLoad(data);
            _time = data.GetCustom<float>("Time");
        }
    }
}
