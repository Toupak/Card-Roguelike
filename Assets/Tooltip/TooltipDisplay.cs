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
        private bool isDisplayedOnTheLeft;
        
        private bool isSetup;

        public void Setup(string mainText, TooltipType type)
        {
            Setup("", mainText, type, false);
        }
        
        public void Setup(string title, string mainText, TooltipType type, bool displayOnTheLeft)
        {
            SetupText(type, title, mainText);
            isDisplayedOnTheLeft = displayOnTheLeft;
            transform.localPosition = CursorInfo.instance.currentPosition + (ComputeOffset() * 0.7f);
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
            Vector2 cursorPosition = CursorInfo.instance.currentPosition + ComputeOffset();
            Vector3 clampedPosition = Tools.ClampPositionInScreen(cursorPosition, rectTransform.rect.size);
            
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, clampedPosition, ref velocity, smoothTime);
        }

        private Vector2 ComputeOffset()
        {
            Vector2 currentOffset = offset;
            if (isDisplayedOnTheLeft)
                currentOffset.x *= -1.0f;

            return currentOffset;
        }

        public void Hide()
        {
            Destroy(gameObject);
        }
    }
}
