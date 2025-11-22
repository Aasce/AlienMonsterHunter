using Asce.Game.Guns;
using Asce.Game.Managers;
using Asce.SaveLoads;
using System.Collections.Generic;

namespace Asce.Game.Players
{
    public class GunsProgress : PlayerCollectionProgress<Gun, GunProgress>
    {
        protected override IEnumerable<Gun> Collection => GameManager.Instance.AllGuns.Guns;

        protected override GunProgress CreateProgressInstance(Gun item) => new (item.Information.Name, item.Information.Progress);
        protected override string GetInformationName(Gun item) => item == null ? string.Empty : item.Information.Name;


        public void ApplyTo(Gun gun)
        {
            if (gun == null) return;
            GunProgress gunProgress = Get(gun.Information.Name);
            if (gunProgress == null) return;
            if (!gunProgress.IsUnlocked) return;

            gun.Leveling.SetLevel(gunProgress.Level);
        }
    }
}
