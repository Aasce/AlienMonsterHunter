using Asce.Game.Managers;
using Asce.Managers;
using UnityEngine;

namespace Asce.PrepareGame
{
    public class PrepareGameManager : MonoBehaviourSingleton<PrepareGameManager>
    {
        [SerializeField] private string _mainGameSceneName;


        public void PlayGame()
        {
            SceneLoader.Instance.Load(_mainGameSceneName);
        }

    }
}
