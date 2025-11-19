using Asce.Game.Supports;
using Asce.Game.Managers;
using Asce.SaveLoads;
using System.Collections.Generic;

namespace Asce.Game.Players
{
    public class SupportsProgress : PlayerCollectionProgress<Support, SupportProgress>
    {
        protected override IEnumerable<Support> Collection => GameManager.Instance.AllSupports.Supports;

        protected override SupportProgress CreateProgressInstance(string name) => new(name);
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

        public void SetLevel(string name, int level)
        {
            SupportProgress supportProgress = Get(name);
            if (supportProgress == null) return;
            if (!supportProgress.IsUnlocked) return;

            supportProgress.Level = level;
            this.SaveProgress(supportProgress);
        }

        protected override void SaveProgress(SupportProgress supportProgress)
        {
            SaveLoadPlayerProgressController playerProgressController = SaveLoadManager.Instance.GetController("Player Progress") as SaveLoadPlayerProgressController;
            if (playerProgressController == null) return;

            playerProgressController.SaveSupportProgress(supportProgress);
        }

        protected override void LoadProgress(SupportProgress supportProgress)
        {
            SaveLoadPlayerProgressController playerProgressController = SaveLoadManager.Instance.GetController("Player Progress") as SaveLoadPlayerProgressController;
            if (playerProgressController == null) return;

            playerProgressController.LoadSupportProgress(supportProgress);
        }
    }
}
