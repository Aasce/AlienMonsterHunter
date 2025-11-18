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
        }

        public void SaveCharacterProgress(CharacterProgress characterProgress)
        {
            if (characterProgress is not ISaveable<CharacterProgressSaveData> saveable) return;
            CharacterProgressSaveData saveData = saveable.Save();

            SaveLoadManager.Instance.SaveIntoFolder("Characters", characterProgress.Name, saveData);
        }

        public void LoadCharacterProgress(CharacterProgress characterProgress)
        {
            CharacterProgressSaveData saveData = SaveLoadManager.Instance.LoadFromFolder<CharacterProgressSaveData>("Characters", characterProgress.Name);
            if (saveData == null) return;
            if (characterProgress is ILoadable<CharacterProgressSaveData> loadable)
            {
                loadable.Load(saveData);
            }
        }
    }
}
