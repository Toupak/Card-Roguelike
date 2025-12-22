using System;
using System.Collections;
using BoomLib.Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Run_Loop
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private Image blackScreen;

        public static UnityEvent OnLoadScene = new UnityEvent();

        private bool isLoading;
        public bool IsLoading => isLoading;
        
        public IEnumerator LoadScene(string sceneName, Action callback = null)
        {
            isLoading = true;
            Debug.Log($"Load Scene : {sceneName}");
            yield return Fader.Fade(blackScreen, 0.3f, true);

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            yield return new WaitUntil(() => operation.isDone);
            
            if (callback != null)
                callback.Invoke();
            
            OnLoadScene?.Invoke();
            
            yield return Fader.Fade(blackScreen, 0.3f, false);
            isLoading = false;
        }
    }
}
