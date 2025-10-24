using Asce.SaveLoads;

namespace Asce.Game.SaveLoads
{
    [System.Serializable]
    public class EnemySaveData : EntitySaveData
    {
        public float attackCooldownRemaining;

        public override void CopyFrom(SaveData other)
        {
            base.CopyFrom(other);
            if (other is EnemySaveData enemyData)
            {
                attackCooldownRemaining = enemyData.attackCooldownRemaining;
            }
        }
    }
}