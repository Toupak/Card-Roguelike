using System;
using System.Collections;
using BoomLib.Tools;
using MapMaker.Rooms;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Run_Loop
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private Image blackScreen;

        public static SceneLoader instance;
        
        public static UnityEvent<string> OnLoadScene = new UnityEvent<string>();

        private bool isLoading;
        public bool IsLoading => isLoading;

        private void Awake()
        {
            instance = this;
        }

        public IEnumerator LoadScene(string sceneName, Action callback = null)
        {
            isLoading = true;
            Debug.Log($"Load Scene : {sceneName}");
            yield return FadeScreen(true);

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            yield return new WaitUntil(() => operation.isDone);
            
            if (callback != null)
                callback.Invoke();
            
            OnLoadScene?.Invoke(sceneName);
            
            yield return FadeScreen(false);
            isLoading = false;
        }

        public IEnumerator LoadRoom(string roomName, RoomData.RoomType roomType, bool hasRoomBeenCleared, Action callback = null)
        {
            isLoading = true;
            Debug.Log($"Load Room : {roomName}");
            yield return FadeScreen(true);

            AsyncOperation operation = SceneManager.LoadSceneAsync(roomName);
            yield return new WaitUntil(() => operation.isDone);

            RoomFiller.instance.FillRoom(RoomBuilder.instance.GetCurrentRoomType(), RoomBuilder.instance.HasRoomBeenCleared());
            
            if (callback != null)
                callback.Invoke();
            
            OnLoadScene?.Invoke(roomName);
            
            yield return FadeScreen(false);
            isLoading = false;
        }

        private IEnumerator FadeScreen(bool fadeIn)
        {
            yield return Fader.Fade(blackScreen, 0.15f, fadeIn);
        }
    }
}
