using System;
using System.Collections;
using System.Collections.Generic;
using BoomLib.BoomTween;
using BoomLib.Inputs;
using BoomLib.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

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

        public void StartDialog(List<string> dialog, Vector2 position)
        {
            if (isDialogDisplayed)
                return;
            
            StartCoroutine(DisplayDialog(dialog, position));
        }
        
        private IEnumerator DisplayDialog(List<string> dialog, Vector2 position)
        {
            isDialogDisplayed = true;
            
            ClearText();
            
            yield return BTween.TweenLocalPosition(panel, WorldToScreenPosition(position), 0.2f, unscaledTime: true); //FadeIn

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

        private Vector3 WorldToScreenPosition(Vector3 position)
        {
            Vector3 myScreenPos = Camera.main.WorldToScreenPoint(position);
            myScreenPos = new Vector3(myScreenPos.x / 3, myScreenPos.y / 3, myScreenPos.z);

            return myScreenPos;
        }
    }
}
