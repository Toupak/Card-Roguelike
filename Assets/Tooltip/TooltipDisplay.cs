using System.Collections.Generic;
using System.Linq;
using BoomLib.Tools;
using Cursor.Script;
using Localization;
using Localization.Icons_In_Text;
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

        public virtual void SetPosition(Vector2 position, bool isOnTheLeft = false)
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
        
        protected string CheckForIcons(string message)
        {
            string finalString = message;
            string[] chunks = message.Split('$');

            List<TextToIconRule> rules = LocalizationSystem.instance.textToIconData.rules;
            
            foreach (string chunk in chunks)
            {
                if (rules.Where((r) => r.tag.Equals(chunk)).ToList().Count > 0)
                {
                    TextToIconRule rule = rules.First((r) => r.tag.Equals(chunk));
                    finalString = finalString.Replace($"${rule.tag}$", $"<sprite name=\"{rule.icon.name}\">");
                }
            }
            
            return finalString;
        }
        
        protected string CheckForDamage(string description, int damage)
        {
            return CheckForDamage(description, $"{damage}");
        }
        
        protected string CheckForDamage(string description, string damage)
        {
            return description.Replace("$d$", damage);
        }
    }
}
