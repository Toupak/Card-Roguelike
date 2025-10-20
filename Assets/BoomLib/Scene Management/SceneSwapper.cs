using System.Collections;
using BoomLib.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace BoomLib.Scene_Management
{
    public class SceneSwapper : MonoBehaviour
    {
        [SerializeField] private Image blackScreen;
        
        public static UnityEvent OnLoadNewScene = new UnityEvent(); 
        
        public static SceneSwapper instance;

        private Coroutine swapCoroutine;
        
        private void Awake()
        {
            if (instance != null)
                Destroy(instance.gameObject);
            instance = this;
            
            DontDestroyOnLoad(gameObject);
        }

        public void SwapScene(SceneField sceneField)
        {
            SwapScene(sceneField.SceneName);
        }

        public void SwapScene(string sceneName)
        {
            if (swapCoroutine != null)
                return;
            
            swapCoroutine = StartCoroutine(SwapSceneCoroutine(sceneName));
        }

        private IEnumerator SwapSceneCoroutine(string sceneName)
        {
            yield return Fader.Fade(blackScreen, 0.3f, true);
            
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            yield return new WaitUntil(() => operation.isDone);

            yield return Fader.Fade(blackScreen, 1.0f, false);

            OnLoadNewScene?.Invoke();
            swapCoroutine = null;
        }
    }
}
