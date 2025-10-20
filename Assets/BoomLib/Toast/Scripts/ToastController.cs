using System.Collections;
using BoomLib.BoomTween;
using TMPro;
using UnityEngine;

namespace BoomLib.Toast.Scripts
{
    /// <summary>
    /// 
    /// Hello ! I am the Toast Controller, my job is to move the toast and destroy it.
    ///
    /// If you want to modify the Toast's behaviour you should look at ToastSpawner.cs
    /// 
    /// </summary>
    public class ToastController : MonoBehaviour
    {
        [SerializeField] private float displayDuration;
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;

        private Vector2 spawnPosition;
        private Vector2 targetPosition;
        
        private bool isSetup;
        
        private IEnumerator Start()
        {
            yield return new WaitUntil(() => isSetup);

            yield return BTween.TweenLocalPosition(transform, targetPosition, 0.3f);
            yield return new WaitForSeconds(displayDuration);
            yield return BTween.TweenLocalPosition(transform, spawnPosition, 0.3f);
            Destroy(gameObject);
        }

        public void Setup(string message, Vector2 _spawnPosition, Vector2 _targetPosition)
        {
            textMeshProUGUI.text = message;
            transform.localPosition = _spawnPosition;
            spawnPosition = _spawnPosition;
            targetPosition = _targetPosition;
            isSetup = true;
        }
    }
}
