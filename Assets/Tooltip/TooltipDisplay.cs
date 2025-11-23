using BoomLib.Tools;
using Cursor.Script;
using UnityEngine;

namespace Tooltip
{
    public abstract class TooltipDisplay : MonoBehaviour
    {
        [Space]
        [SerializeField] private Vector2 leftOffset;
        [SerializeField] private Vector2 rightOffset;

        private RectTransform rectTransform;
        private Vector3 velocity;
        private float smoothTime = 0.1f;
        
        private bool isDisplayedOnTheLeft;
        
        private bool isSetup;

        public virtual void Setup()
        {
            rectTransform = GetComponent<RectTransform>();
            isSetup = true;
        }

        public virtual void SetPosition(Vector2 position, bool isOnTheLeft = true)
        {
            isDisplayedOnTheLeft = isOnTheLeft;
            transform.localPosition = Tools.ClampPositionInScreen(position + ComputeOffset(), rectTransform.rect.size);
        }

        private void FollowCursor()
        {
            Vector2 cursorPosition = CursorInfo.instance.currentPosition + ComputeOffset();
            Vector3 clampedPosition = Tools.ClampPositionInScreen(cursorPosition, rectTransform.rect.size);
            
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, clampedPosition, ref velocity, smoothTime);
        }

        private Vector2 ComputeOffset()
        {
            return isDisplayedOnTheLeft ? leftOffset : rightOffset;
        }

        public virtual void Hide()
        {
            Destroy(gameObject);
        }
    }
}
