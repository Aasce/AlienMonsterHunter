namespace Asce.Game.Stats
{
    public static class StatExtension
    {
        public static float GetRatio (this ResourceStat stat)
        {
            if (stat == null) return 0f;
            if (stat.FinalValue == 0) return 0f;

            return stat.CurrentValue / stat.FinalValue;
        }

        public static void ToFull(this ResourceStat stat)
        {
            if (stat == null) return;
            stat.CurrentValue = stat.FinalValue;
        }

        public static void ToEmpty(this ResourceStat stat)
        {
            if (stat == null) return;
            stat.CurrentValue = 0;
        }
    }
}