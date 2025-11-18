using Asce.Managers;
using Asce.SaveLoads;
using UnityEngine;

namespace Asce.Game.Players
{
    public class PlayerProgress : GameComponent
    {
        [SerializeField] private CharactersProgress _charactersProgress;


        public CharactersProgress CharactersProgress => _charactersProgress;

        public void SaveAll()
        {
            SaveLoadPlayerProgressController playerProgressController = SaveLoadManager.Instance.GetController("Player Progress") as SaveLoadPlayerProgressController;
            if (playerProgressController == null) return;

            foreach (CharacterProgress progress in CharactersProgress.AllProgresses)
            {
                playerProgressController.SaveCharacterProgress(progress);
            }
        }
    }
}