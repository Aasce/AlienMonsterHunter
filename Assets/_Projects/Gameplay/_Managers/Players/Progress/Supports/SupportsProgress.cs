using Asce.Game.Supports;
using Asce.Game.Managers;
using Asce.SaveLoads;
using System.Collections.Generic;

namespace Asce.Game.Players
{
    public class SupportsProgress : PlayerCollectionProgress<Support, SupportProgress>
    {
        protected override IEnumerable<Support> Collection => GameManager.Instance.AllSupports.Supports;

        protected override SupportProgress CreateProgressInstance(Support item) => new(item.Information.Name, item.Information.Progress);
        protected override string GetInformationName(Support item) => item == null ? string.Empty : item.Information.Key;


        public void ApplyTo(SupportContainer supportContainer)
        {
            if (supportContainer == null) return;
            if (!supportContainer.IsValid) return;
            SupportProgress supportProgress = Get(supportContainer.Information.Name);
            if (supportProgress == null) return;
            if (!supportProgress.IsUnlocked) return;

            supportContainer.Level = supportProgress.Level;
        }

    }
}
