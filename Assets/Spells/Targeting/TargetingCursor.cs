using BoomLib.Tools;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Spells.Targeting
{
    public class TargetingCursor : MonoBehaviour
    {
        private RectTransform rectTransform;
        private UILineRenderer lineRenderer;
        
        private Transform startingPositionTarget;

        private bool isSetup;
        
        public void Setup(Transform startingPosition)
        {
            rectTransform = GetComponent<RectTransform>();
            lineRenderer = GetComponent<UILineRenderer>();
            startingPositionTarget = startingPosition;

            isSetup = true;
        }

        public void UpdatePosition()
        {
            if (!isSetup)
                return;

            transform.position = startingPositionTarget.position;
            lineRenderer.SetLinePositions(Vector2.zero, Mouse.current.position.value - rectTransform.position.ToVector2());
        }
        
        public void DestroyCursor()
        {
            Destroy(gameObject);
        }
    }
}
