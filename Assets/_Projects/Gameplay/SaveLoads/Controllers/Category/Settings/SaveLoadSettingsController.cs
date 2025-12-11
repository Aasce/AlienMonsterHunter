using Asce.Game.Sounds;

namespace Asce.SaveLoads
{
    public class SaveLoadSettingsController : SaveLoadController
    {
        protected override void LoadName()
        {
            _name = "Settings";
        }

        public override void Load()
        {
            base.Load();
            this.LoadSettings();
        }

        public void SaveSettings()
        {
            if (SoundManager.Instance.Settings is not ISaveable<SoundSettingsSaveData> saveable) return;
            SoundSettingsSaveData saveData = saveable.Save();

            SaveLoadManager.Instance.Save("SoundSettings", saveData);
        }

        public void LoadSettings()
        {
            SoundSettingsSaveData saveData = SaveLoadManager.Instance.Load<SoundSettingsSaveData>("SoundSettings");
            if (SoundManager.Instance.Settings is ILoadable<SoundSettingsSaveData> loadable)
            {
                loadable.Load(saveData);
            }
        }
    }
}
