using Asce.Core.UIs;
using Asce.Game.Sounds;
using Asce.SaveLoads;
using UnityEngine;
using UnityEngine.UI;

namespace Asce.Game.UIs.Panels
{
    public class UISoundVolume : UISettingContent
    {
        [SerializeField] protected Slider _masterVolume;
        [SerializeField] protected Slider _musicVolume;
        [SerializeField] protected Slider _sfxVolume;

        public override void Initialize()
        {
            base.Initialize();
            _masterVolume.value = SoundManager.Instance.Settings.MasterVolume;
            _masterVolume.onValueChanged.AddListener(MasterVolume_OnValueChanged);

            _musicVolume.value = SoundManager.Instance.Settings.MusicVolume;
            _musicVolume.onValueChanged.AddListener(MusicVolume_OnValueChanged);
            
            _sfxVolume.value = SoundManager.Instance.Settings.SFXVolume;
            _sfxVolume.onValueChanged.AddListener(SFXVolume_OnValueChanged);

            Panel.OnHide += SettingsPanel_OnHide;
        }

        private void MasterVolume_OnValueChanged(float value)
        {
            SoundManager.Instance.Settings.MasterVolume = value;
        }
        private void MusicVolume_OnValueChanged(float value)
        {
            SoundManager.Instance.Settings.MusicVolume = value;
        }
        private void SFXVolume_OnValueChanged(float value)
        {
            SoundManager.Instance.Settings.SFXVolume = value;
        }

        private void SettingsPanel_OnHide(object sender)
        {
            SaveLoadSettingsController saveLoadController = SaveLoadManager.Instance.GetController("Settings") as SaveLoadSettingsController;
            if (saveLoadController == null) return;

            saveLoadController.SaveSettings();
        }

    }
}