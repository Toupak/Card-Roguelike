using BoomLib.Tools;
using Cursor.Script;
using TMPro;
using UnityEngine;

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
        
        [SerializeField] private Vector2 offset;

        private RectTransform rectTransform;
        
        private Vector3 velocity;
        
        private bool isSetup;

        public void Setup(string mainText, TooltipType type)
        {
            Setup("", mainText, type);
        }
        
        public void Setup(string title, string mainText, TooltipType type)
        {
            SetupText(type, title, mainText);
            transform.localPosition = CursorInfo.instance.currentPosition + offset;
            rectTransform = GetComponent<RectTransform>();
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
            Vector2 cursorPosition = CursorInfo.instance.currentPosition + offset;
            Vector3 clampedPosition = Tools.ClampPositionInScreen(cursorPosition, rectTransform.rect.size);
            
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, clampedPosition, ref velocity, smoothTime);
        }

       
        public void Hide()
        {
            Destroy(gameObject);
        }
    }
}
