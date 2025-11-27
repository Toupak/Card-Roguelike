using BoomLib.Tools;
using Cursor.Script;
using TMPro;
using UnityEngine;

namespace Tooltip
{
    public class TooltipDisplay : MonoBehaviour
    {
        [SerializeField] protected TextMeshProUGUI titleText;
        [SerializeField] protected TextMeshProUGUI mainText;
        
        private RectTransform rectTransform;
        private Vector3 velocity;
        private float smoothTime = 0.1f;
        
        public virtual void Setup()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void SetupTooltip(string title, string main)
        {
            titleText.text = title;
            mainText.text = main;
        }

        public virtual void SetPosition(Vector2 position)
        {
            rectTransform.position = Tools.ClampPositionInScreenUpPivot(position, rectTransform.rect.size);
        }

        private void FollowCursor()
        {
            Vector2 cursorPosition = CursorInfo.instance.currentPosition;
            Vector3 clampedPosition = Tools.ClampPositionInScreenUpPivot(cursorPosition, rectTransform.rect.size);

            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, clampedPosition, ref velocity, smoothTime);
        }

        public virtual void Hide()
        {
            Destroy(gameObject);
        }
    }
}
