using System;
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
        
        [Space]
        [SerializeField] protected Color subTooltipColor;

        private RectTransform rectTransform;
        private Vector2 targetPosition;
        
        private Vector3 velocity;
        private float smoothTime = 0.1f;

        public void SetupTooltip(string title, string main)
        {
            titleText.text = title;
            mainText.text = main;
            
        }

        public void SetAsSubTooltip()
        {
            titleText.color = subTooltipColor;
            mainText.color = subTooltipColor;
        }

        private void Update()
        {
            if (rectTransform != null)
                rectTransform.position = Tools.ClampPositionInScreenUpPivot(targetPosition, rectTransform.rect.size);
        }

        public void SetPosition(Vector2 position)
        {
            targetPosition = position;
            rectTransform = GetComponent<RectTransform>();
            rectTransform.position = Tools.ClampPositionInScreenUpPivot(targetPosition, rectTransform.rect.size);
        }

        private void FollowCursor()
        {
            Vector2 cursorPosition = CursorInfo.instance.currentPosition;
            Vector3 clampedPosition = Tools.ClampPositionInScreenUpPivot(cursorPosition, rectTransform.rect.size);

            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, clampedPosition, ref velocity, smoothTime);
        }

        public void Hide()
        {
            TooltipFactory.instance.Hide();
        }
    }
}
