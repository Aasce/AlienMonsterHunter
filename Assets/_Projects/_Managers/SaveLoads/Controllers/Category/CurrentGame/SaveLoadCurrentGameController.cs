using UnityEngine;

namespace Asce.SaveLoads
{
    public class SaveLoadCurrentGameController : SaveLoadController
    {
        protected override void LoadName()
        {
            _name = "Current Game";
        }

        public bool HasSaveCurrentGame()
        {
            CurrentGameConfigData gameConfigData = SaveLoadManager.Instance.Load<CurrentGameConfigData>("CurrentGameConfig");
            if (gameConfigData == null || !gameConfigData.hasSave)
            { 
                return false;
            }
            return true;
        }

        public void ClearCurrentGame()
        {
            CurrentGameConfigData gameConfigData = SaveLoadManager.Instance.Load<CurrentGameConfigData>("CurrentGameConfig");
            if (gameConfigData == null) return;
            SaveLoadManager.Instance.Save("CurrentGameConfig", new CurrentGameConfigData()
            {
                hasSave = false,
            });
        }
    }
}
