using Asce.Game.Entities.Characters;
using Asce.Game.Players;
using UnityEngine;

namespace Asce.SaveLoads
{
    public class SaveLoadPlayerProgressController : SaveLoadController
    {
        protected override void LoadName()
        {
            _name = "Player Progress";
        }

        public void SaveAllProgress()
        {
            foreach (CharacterProgress progress in PlayerManager.Instance.Progress.CharactersProgress.AllProgresses)
            {
                this.SaveCharacterProgress(progress);
            }

            foreach (GunProgress progress in PlayerManager.Instance.Progress.GunsProgress.AllProgresses)
            {
                this.SaveGunProgress(progress);
            }
        }

        public void SaveCharacterProgress(CharacterProgress characterProgress)
        {
            if (characterProgress is not ISaveable<CharacterProgressSaveData> saveable) return;
            CharacterProgressSaveData saveData = saveable.Save();

            SaveLoadManager.Instance.SaveIntoFolder("CharactersProgress", characterProgress.Name, saveData);
        }

        public void LoadCharacterProgress(CharacterProgress characterProgress)
        {
            CharacterProgressSaveData saveData = SaveLoadManager.Instance.LoadFromFolder<CharacterProgressSaveData>("CharactersProgress", characterProgress.Name);
            if (saveData == null) return;
            if (characterProgress is ILoadable<CharacterProgressSaveData> loadable)
            {
                loadable.Load(saveData);
            }
        }

        public void SaveGunProgress(GunProgress gunProgress)
        {
            if (gunProgress is not ISaveable<GunProgressSaveData> saveable) return;
            GunProgressSaveData saveData = saveable.Save();

            SaveLoadManager.Instance.SaveIntoFolder("GunsProgress", gunProgress.Name, saveData);
        }

        public void LoadGunProgress(GunProgress gunProgress)
        {
            GunProgressSaveData saveData = SaveLoadManager.Instance.LoadFromFolder<GunProgressSaveData>("GunsProgress", gunProgress.Name);
            if (saveData == null) return;
            if (gunProgress is ILoadable<GunProgressSaveData> loadable)
            {
                loadable.Load(saveData);
            }
        }
    }
}
