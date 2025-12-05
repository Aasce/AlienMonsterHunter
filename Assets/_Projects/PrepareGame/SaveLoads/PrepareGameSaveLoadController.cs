using Asce.Core;
using Asce.PrepareGame.Picks;
using Asce.PrepareGame.SaveLoads;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.PrepareGame
{
    public class PrepareGameSaveLoadController : MonoBehaviourSingleton<PrepareGameSaveLoadController>
    {
        public void SaveLastPick()
        {
            if (PickController.Instance is ISaveable<LastPickSaveData> saveable)
            {
                LastPickSaveData lastPickData = saveable.Save();
                SaveLoadManager.Instance.Save("PrepareGameLastPick", lastPickData);
            }
        }

        public void LoadLastPick()
        {
            if (PickController.Instance is ILoadable<LastPickSaveData> loadable)
            {
                LastPickSaveData lastPickData = SaveLoadManager.Instance.Load<LastPickSaveData>("PrepareGameLastPick");
                loadable.Load(lastPickData);
            }
        }
    }
}
