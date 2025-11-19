using Asce.Managers;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.Players
{
    public class PlayerProgress : GameComponent
    {
        [SerializeField] private CharactersProgress _charactersProgress;
        [SerializeField] private GunsProgress _gunsProgress;


        public CharactersProgress CharactersProgress => _charactersProgress;
        public GunsProgress GunsProgress => _gunsProgress;

        public void SaveAll()
        {
            SaveLoadPlayerProgressController playerProgressController = SaveLoadManager.Instance.GetController("Player Progress") as SaveLoadPlayerProgressController;
            if (playerProgressController == null) return;
            playerProgressController.SaveAllProgress();
        }
    }
}