using UnityEngine;

namespace BoomLib.Toast.Scripts
{
    /// <summary>
    /// 
    /// Hello ! I am the Toast Spawner, my job is to spawn toasts.
    ///
    /// I am a Singleton, you must put me in the scene.
    /// I need a reference to a ToastController prefab in your project that I can Instantiate each time a toast is requested.
    /// 
    /// </summary>
    public class ToastSpawner : MonoBehaviour
    {
        [SerializeField] private Transform spawnPosition;
        [SerializeField] private Transform targetPosition;
        [SerializeField] private ToastController toastPrefab;
        
        public static ToastSpawner instance;

        private void Awake()
        {
            if (instance != null)
                Destroy(instance.gameObject);
            instance = this;

        }

        public void SpawnToast(string message)
        {
            ToastController toast = Instantiate(toastPrefab, Vector3.zero, Quaternion.identity, transform);
            toast.Setup(message, spawnPosition.localPosition, targetPosition.localPosition);
        }
    }
}
