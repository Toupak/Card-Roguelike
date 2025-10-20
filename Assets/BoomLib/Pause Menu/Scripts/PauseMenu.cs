using System.Collections;
using BoomLib.BoomTween;
using BoomLib.Tools;
using UnityEngine;
using UnityEngine.UI;

namespace BoomLib.Pause_Menu.Scripts
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private Image blackScreen;
        [SerializeField] private Transform panel;
        
        [Space]
        [SerializeField] private Transform displayedPosition;
        [SerializeField] private Transform hiddenPosition;
        
        public static PauseMenu instance;

        private bool isGamePaused;
        public bool IsGamePaused => isGamePaused;
        
        private void Awake()
        {
            if (instance != null)
                Destroy(instance.gameObject);
            instance = this;

            panel.localPosition = hiddenPosition.localPosition;
        }

        public void TriggerPauseMenu()
        {
            if (isGamePaused)
                ResumeGame();
            else
                PauseGame();
        }

        private void PauseGame()
        {
            isGamePaused = true;
            StopAllCoroutines();
            StartCoroutine(PauseGameCoroutine());
        }

        private IEnumerator PauseGameCoroutine()
        {
            Time.timeScale = 0.0f;

            StartCoroutine(Fader.Fade(blackScreen, 0.1f, true, 0.8f, unscaledTime:true));
            yield return BTween.TweenLocalPosition(panel, displayedPosition.localPosition, 0.2f, unscaledTime:true);
        }

        public void ResumeGame()
        {
            isGamePaused = false;
            StopAllCoroutines();
            StartCoroutine(ResumeGameCoroutine());
        }
        
        private IEnumerator ResumeGameCoroutine()
        {
            StartCoroutine(Fader.Fade(blackScreen, 0.1f, false, 0.8f, unscaledTime:true));
            yield return BTween.TweenLocalPosition(panel, hiddenPosition.localPosition, 0.1f, unscaledTime:true);
            
            Time.timeScale = 1.0f;
        }

        public void QuitGame()
        {
            Tools.QuitGameHelper.Quit();
        }
    }
}
