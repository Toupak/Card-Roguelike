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

        public static UnityEvent<string> OnLoadScene = new UnityEvent<string>();
        public static UnityEvent OnLoadRoom = new UnityEvent();

        private bool isLoading;
        public bool IsLoading => isLoading;
        
        public IEnumerator LoadScene(string sceneName, bool isNewRoom, Action callback = null)
        {
            isLoading = true;
            Debug.Log($"Load Scene : {sceneName}");
            yield return Fader.Fade(blackScreen, 0.15f, true);

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            yield return new WaitUntil(() => operation.isDone);
            
            if (callback != null)
                callback.Invoke();
            
            if (isNewRoom)
                OnLoadRoom?.Invoke();
            
            OnLoadScene?.Invoke(sceneName);
            
            yield return Fader.Fade(blackScreen, 0.15f, false);
            isLoading = false;
        }
    }
}
