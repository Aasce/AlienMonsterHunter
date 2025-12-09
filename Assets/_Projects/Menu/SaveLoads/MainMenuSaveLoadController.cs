using Asce.Core;
using Asce.MainMenu.Picks;
using Asce.MainMenu.SaveLoads;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.MainMenu
{
    public class MainMenuSaveLoadController : MonoBehaviourSingleton<MainMenuSaveLoadController>
    {
        public void SaveLastPick()
        {
            if (PickController.Instance is ISaveable<LastMapLevelPickSaveData> saveable)
            {
                LastMapLevelPickSaveData lastPickData = saveable.Save();
                SaveLoadManager.Instance.Save("MainMenuLastPick", lastPickData);
            }
        }

        public void LoadLastPick()
        {
            if (PickController.Instance is ILoadable<LastMapLevelPickSaveData> loadable)
            {
                LastMapLevelPickSaveData lastPickData = SaveLoadManager.Instance.Load<LastMapLevelPickSaveData>("MainMenuLastPick");
                loadable.Load(lastPickData);
            }
        }
    }
}