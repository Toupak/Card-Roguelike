using System;
using BoomLib.Tools;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Tooltip
{
    public class TooltipDisplay : MonoBehaviour
    {
        public enum TooltipType
        {
            Spell,
            Regular
        }
        
        [SerializeField] private TextMeshProUGUI titleTextMeshPro;
        [SerializeField] private TextMeshProUGUI mainTextMeshPro;
        [SerializeField] private TextMeshProUGUI regularTextMeshPro;
        [SerializeField] private float smoothTime;

        private CanvasScaler canvasScaler;
        
        private Vector3 velocity;
        
        private bool isSetup;

        public void SetCanvasScaler(CanvasScaler scaler)
        {
            canvasScaler = scaler;
        }
        
        public void Setup(string mainText, TooltipType type)
        {
            SetupText(type, "", mainText);
            isSetup = true;
        }
        
        public void Setup(string title, string mainText, TooltipType type)
        {
            SetupText(type, title, mainText);
            transform.localPosition = ComputeCursorPosition();
            isSetup = true;
        }

        private void SetupText(TooltipType type, string title, string main)
        {
            titleTextMeshPro.gameObject.SetActive(type == TooltipType.Spell);
            mainTextMeshPro.gameObject.SetActive(type == TooltipType.Spell);
            regularTextMeshPro.gameObject.SetActive(type == TooltipType.Regular);

            titleTextMeshPro.text = title;
            mainTextMeshPro.text = main;
            regularTextMeshPro.text = main;
        }

        private void Update()
        {
            if (!isSetup)
                return;
            
            FollowCursor();
        }

        private void FollowCursor()
        {
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, ComputeCursorPosition(), ref velocity, smoothTime);
        }

        private Vector2 ComputeCursorPosition()
        {
            return new Vector2(Input.mousePosition.x * canvasScaler.referenceResolution.x / Screen.width, Input.mousePosition.y * canvasScaler.referenceResolution.y / Screen.height);
        }

        public void Hide()
        {
            Destroy(gameObject);
        }
    }
}
