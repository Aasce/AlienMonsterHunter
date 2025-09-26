using Asce.Managers;
using UnityEngine;

namespace Asce.Game.Managers
{
    public class BootstrapManager : MonoBehaviourSingleton<BootstrapManager>
    {
		[SerializeField] private string _targetSceneName = "Menu";
		
		private void Start() 
		{
			SceneLoader.Instance.Load(_targetSceneName, showLoadingScene: true, delay: 2f, additiveLoadingScene: true);
		}
    }
}
