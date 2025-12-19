using System;
using System.Collections;
using System.Collections.Generic;
using BoomLib.BoomTween;
using BoomLib.Inputs;
using BoomLib.Tools;
using TMPro;
using UnityEngine;

namespace BoomLib.Dialog_System
{
    public class DialogManager : MonoBehaviour
    {
        public static DialogManager instance;
        
        [SerializeField] private Transform panel;
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        
        [Space]
        [SerializeField] private Transform displayedPosition;
        [SerializeField] private Transform hiddenPosition;

        private bool isDialogDisplayed;
        public bool IsDialogDisplayed => isDialogDisplayed;

        private InputPacker inputPacker = new InputPacker();
        
        private void Awake()
        {
            if (instance != null)
                Destroy(instance.gameObject);
            instance = this;

            panel.localPosition = hiddenPosition.localPosition;
        }

        public void StartDialog(List<string> dialog)
        {
            if (isDialogDisplayed)
                return;
            
            StartCoroutine(DisplayDialog(dialog));
        }
        
        private IEnumerator DisplayDialog(List<string> dialog)
        {
            isDialogDisplayed = true;
            
            ClearText();
            
            yield return BTween.TweenLocalPosition(panel, displayedPosition.localPosition, 0.2f, unscaledTime: true); //FadeIn

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
            
            yield return BTween.TweenLocalPosition(panel, hiddenPosition.localPosition, 0.2f, unscaledTime: true); //FadeOut
            
            isDialogDisplayed = false;
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
    }
}
