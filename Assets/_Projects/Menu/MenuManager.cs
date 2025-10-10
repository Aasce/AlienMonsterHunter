using Asce.Game.Managers;
using Asce.Managers;
using UnityEngine;

namespace Asce.Menu
{
    public class MenuManager : MonoBehaviourSingleton<MenuManager>
    {
        [SerializeField] private string _mainGameSceneName;
        [SerializeField] private string _prepareGameSceneName;


        public void PlayGame()
        {
            SceneLoader.Instance.Load(_mainGameSceneName, delay: 0.5f);
        }

        public void PlayNewGame()
        {
            SceneLoader.Instance.Load(_prepareGameSceneName, delay: 0.5f);
        }
    }
}
