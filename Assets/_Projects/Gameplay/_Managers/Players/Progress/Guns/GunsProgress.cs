using Asce.Game.Guns;
using Asce.Game.Managers;
using Asce.SaveLoads;
using System.Collections.Generic;

namespace Asce.Game.Players
{
    public class GunsProgress : PlayerCollectionProgress<Gun, GunProgress>
    {
        protected override IEnumerable<Gun> Collection => GameManager.Instance.AllGuns.Guns;

        protected override GunProgress CreateProgressInstance(string name) => new(name);
        protected override string GetInformationName(Gun item) => item == null ? string.Empty : item.Information.Name;


        public void ApplyTo(Gun gun)
        {
            if (gun == null) return;
            GunProgress gunProgress = Get(gun.Information.Name);
            if (gunProgress == null) return;
            if (!gunProgress.IsUnlocked) return;

            gun.Leveling.SetLevel(gunProgress.Level);
        }

        public void SetLevel(string name, int level)
        {
            GunProgress gunProgress = Get(name);
            if (gunProgress == null) return;
            if (!gunProgress.IsUnlocked) return;

            gunProgress.Level = level;
            this.SaveProgress(gunProgress);
        }

        protected override void SaveProgress(GunProgress gunProgress)
        {
            SaveLoadPlayerProgressController playerProgressController = SaveLoadManager.Instance.GetController("Player Progress") as SaveLoadPlayerProgressController;
            if (playerProgressController == null) return;

            playerProgressController.SaveGunProgress(gunProgress);
        }

        protected override void LoadProgress(GunProgress gunProgress)
        {
            SaveLoadPlayerProgressController playerProgressController = SaveLoadManager.Instance.GetController("Player Progress") as SaveLoadPlayerProgressController;
            if (playerProgressController == null) return;

            playerProgressController.LoadGunProgress(gunProgress);
        }
    }
}
