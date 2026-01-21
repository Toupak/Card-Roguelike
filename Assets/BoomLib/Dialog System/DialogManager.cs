using System;
using System.Collections;
using System.Collections.Generic;
using BoomLib.Inputs;
using BoomLib.Tools;
using PrimeTween;
using TMPro;
using UnityEngine;

namespace BoomLib.Dialog_System
{
    public class DialogManager : MonoBehaviour
    {
        public static DialogManager instance;
        
        [SerializeField] private Transform panel;
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        [SerializeField] private CanvasGroup canvasGroup;
        
        private bool isDialogDisplayed;
        public bool IsDialogDisplayed => isDialogDisplayed;

        private InputPacker inputPacker = new InputPacker();
        private RectTransform panelRect;
        
        private void Awake()
        {
            if (instance != null)
                Destroy(instance.gameObject);
            instance = this;

            panelRect = panel.GetComponent<RectTransform>();
            canvasGroup.alpha = 0.0f;
        }

        public void StartDialog(List<string> dialog, Vector2 position, Action callback = null)
        {
            if (isDialogDisplayed)
                return;
            
            StartCoroutine(DisplayDialog(dialog, position, callback));
        }
        
        private IEnumerator DisplayDialog(List<string> dialog, Vector2 position, Action callback = null)
        {
            isDialogDisplayed = true;
            
            ClearText();

            yield return DisplayPanel(WorldToScreenPosition(position));

            foreach (string line in dialog)
            {
                DisplayLine(line);
                yield return new WaitForSeconds(0.2f);
                yield return WaitForPlayerInput();

                if (!IsDialogFullyDisplayed())
                {
                    DisplayCurrentLineInFull();
                    yield return null;
                    yield return WaitForPlayerInput();
                }
            }

            ClearText();
            
            yield return HidePanel();
            
            isDialogDisplayed = false;
            
            callback?.Invoke();
        }

        private IEnumerator DisplayPanel(Vector3 targetPosition)
        {
            bool isComplete = false;

            float fadeDistance = 100.0f;
            Vector2 startingPosition = targetPosition.ToVector2() + Vector2.down * fadeDistance;

            panelRect.anchoredPosition = startingPosition;
            
            Sequence.Create()
                .Group(Tween.UIAnchoredPosition(panelRect, targetPosition, 0.2f, Ease.OutSine))
                .Group(Tween.Alpha(canvasGroup, 1.0f, 0.1f))
                .ChainCallback(() => isComplete = true);

            yield return new WaitUntil(() => isComplete);
        }
        
        private IEnumerator HidePanel()
        {
            bool isComplete = false;

            float fadeDistance = 100.0f;
            Vector2 targetPosition = panelRect.anchoredPosition + Vector2.down * fadeDistance;
            
            Sequence.Create()
                .Group(Tween.UIAnchoredPosition(panelRect, targetPosition, 0.2f, Ease.OutSine))
                .Group(Tween.Alpha(canvasGroup, 0.0f, 0.1f))
                .ChainCallback(() => isComplete = true);

            yield return new WaitUntil(() => isComplete);
        }
        
        private IEnumerator WaitForPlayerInput()
        {
            while (true)
            {
                if (HasPlayerSentInput())
                    yield break;
                yield return null;
            }
        }
        
        private bool HasPlayerSentInput()
        {
            return inputPacker.ComputeInputPackage().anyKey;
        }

        private bool IsDialogFullyDisplayed()
        {
            return true;
        }

        private void DisplayCurrentLineInFull()
        {
            
        }

        private void DisplayLine(string line)
        {
            textMeshProUGUI.text = line;
        }
        
        private void ClearText()
        {
            textMeshProUGUI.text = "";
        }

        private Vector3 WorldToScreenPosition(Vector3 position)
        {
            Vector3 myScreenPos = Camera.main.WorldToScreenPoint(position);
            myScreenPos = new Vector3(myScreenPos.x / 3, myScreenPos.y / 3, myScreenPos.z);

            return myScreenPos;
        }
    }
}
