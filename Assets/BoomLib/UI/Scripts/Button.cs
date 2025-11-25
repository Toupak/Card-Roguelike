using System.Collections;
using BoomLib.BoomTween;
using BoomLib.SFX_Player.Scripts;
using BoomLib.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace BoomLib.UI.Scripts
{
    public class Button : MonoBehaviour, IPointerDownHandler 
    {
        [SerializeField] private Image border;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private float clickAnimationDuration;
        [SerializeField] private AudioClip clickSound;

        public UnityEvent OnClick = new UnityEvent(); 
        
        private bool isDisplayed = true;
        private bool isClicked;

        private Vector3 startingScale;
        
        private void Start()
        {
            startingScale = border.rectTransform.localScale;
        }

        public void Display()
        {
            if (isDisplayed)
                return;

            border.gameObject.SetActive(true);
            text.gameObject.SetActive(true);

            isDisplayed = true;
            
            StopAllCoroutines();
            StartCoroutine(Fader.Fade(border, 3.0f, true));
            StartCoroutine(Fader.Fade(text, 3.0f, true));
        }
        
        public void Hide()
        {
            if (!isDisplayed)
                return;
            
            isDisplayed = false;
            
            StopAllCoroutines();
            StartCoroutine(Fader.Fade(border, 1.0f, false));
            StartCoroutine(Fader.Fade(text, 1.0f, false));
        }
        
        public void HideInstantly()
        {
            if (!isDisplayed)
                return;

            StopAllCoroutines();
            border.gameObject.SetActive(false);
            text.gameObject.SetActive(false);
            
            isDisplayed = false;
        }
        
        public void OnPointerDown (PointerEventData eventData) 
        {
            if (!isDisplayed || isClicked)
                return;
                
            StopAllCoroutines();
            StartCoroutine(WaitAnimationAndTrigger());
        }

        private IEnumerator WaitAnimationAndTrigger()
        {
            SFXPlayer.instance.PlaySFX(clickSound);
            yield return BTween.Squeeze(border.transform, Vector3.one, new Vector2(1.5f, 0.5f), clickAnimationDuration, unscaledTime: true);
            yield return new WaitForSecondsRealtime(0.1f);
            
            OnClick?.Invoke();
        }

        public void SetText(string message)
        {
            text.text = message;
        }
    }
}
