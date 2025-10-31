using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Tooltip
{
    public class TooltipDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMeshPro;
        [SerializeField] private float smoothTime;

        private Vector2 offset;
        private Vector3 velocity;
        
        private bool isSetup;
        
        public void Setup(string text)
        {
            textMeshPro.text = text;
            offset = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
            transform.localPosition = ComputeCursorPosition();
            isSetup = true;
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

        private Vector3 ComputeCursorPosition()
        {
            return Mouse.current.position.value - offset;
        }

        public void Hide()
        {
            Destroy(gameObject);
        }
    }
}
