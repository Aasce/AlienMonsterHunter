using Asce.Core;
using Asce.Game.Maps;
using Asce.Game.SaveLoads;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.Managers
{
    public abstract class GameStateCondition : GameComponent, ISaveable<GameStateConditionSaveData>, ILoadable<GameStateConditionSaveData>
    {
        public abstract string ConditionName { get; }

        public virtual void Initialize() { }
        public virtual void SetData(MapLevelGameStateCondition data) { }
        public virtual void Ready() { }
        public virtual void OnCheck() { }

        public abstract bool IsSatisfied();

        GameStateConditionSaveData ISaveable<GameStateConditionSaveData>.Save()
        {
            GameStateConditionSaveData saveData = new()
            {
                name = ConditionName,
            };
            this.OnBeforeSave(saveData);
            return saveData;
        }

        void ILoadable<GameStateConditionSaveData>.Load(GameStateConditionSaveData data)
        {
            if (data == null) return;
            this.OnAfterLoad(data);
        }

        protected virtual void OnBeforeSave(GameStateConditionSaveData data) { }
        protected virtual void OnAfterLoad(GameStateConditionSaveData data) { }
    }
}