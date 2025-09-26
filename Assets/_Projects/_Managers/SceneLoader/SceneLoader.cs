using Asce.Managers;
using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Asce.Game.Managers
{
    public class SceneLoader : DontDestroyOnLoadSingleton<SceneLoader>
    {
        [SerializeField] private string _loadingSceneName = "Loading";

        /// <summary>
        ///     Scene name for the loading screen.
        /// </summary>
        public string LoadingSceneName => _loadingSceneName;

        /// <summary>
        ///     Raised when a scene loading progress updates. Value range [0,1].
        /// </summary>
        public event Action<float> OnLoadingProgress;

        /// <summary>
        ///     Fire-and-forget load. Use when you don't need to await.
        /// </summary>
        public void Load(string sceneName, bool showLoadingScene = true, float delay = 0f, bool additiveLoadingScene = false)
        {
            _ = LoadAsync(sceneName, showLoadingScene, delay, additiveLoadingScene);
        }

        /// <summary>
        ///     Load a new scene asynchronously. Caller can await this Task.
        /// </summary>
        public async Task LoadAsync(string sceneName, bool showLoadingScene = true, float delay = 0f, bool additiveLoadingScene = false)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogWarning("[SceneLoader] Cannot load scene: name is null or empty.");
                return;
            }

            // Step 1: Load the loading screen if required
            if (showLoadingScene && !string.IsNullOrEmpty(_loadingSceneName))
            {
                LoadSceneMode mode = additiveLoadingScene ? LoadSceneMode.Additive : LoadSceneMode.Single;
                AsyncOperation loadingOp = SceneManager.LoadSceneAsync(_loadingSceneName, mode);
                while (!loadingOp.isDone)
                {
                    await Task.Yield();
                }
            }

            // Step 2: Start loading the target scene asynchronously
            AsyncOperation targetOp = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            if (targetOp == null)
            {
                Debug.LogError($"[SceneLoader] Failed to load scene: {sceneName}");
                return;
            }

            targetOp.allowSceneActivation = false;

            // Step 3: Monitor progress until loading reaches 90%
            while (targetOp.progress < 0.9f)
            {
                float progress = Mathf.Clamp01(targetOp.progress / 0.9f);
                OnLoadingProgress?.Invoke(progress);
                await Task.Yield();
            }

            // Step 4: Tween progress from 0.9 -> 1.0 during delay
            float elapsed = 0f;
            while (elapsed < delay)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / delay);
                float progress = Mathf.Lerp(0.9f, 1f, t);
                OnLoadingProgress?.Invoke(progress);
                await Task.Yield();
            }

            // Ensure 100% progress reported
            OnLoadingProgress?.Invoke(1f);

            // Step 5: Allow scene activation
            targetOp.allowSceneActivation = true;

            // Wait until it actually finishes
            while (!targetOp.isDone)
            {
                await Task.Yield();
            }

            // Step 6: Unload loading scene if it was additive
            if (showLoadingScene && additiveLoadingScene)
            {
                if (SceneManager.GetSceneByName(_loadingSceneName).isLoaded)
                {
                    await SceneManager.UnloadSceneAsync(_loadingSceneName);
                }
            }
        }
    }
}
