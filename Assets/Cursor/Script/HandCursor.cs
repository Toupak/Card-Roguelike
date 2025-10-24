using BoomLib.BoomTween;
using UnityEngine;

namespace Cursor.Script
{
    public class HandCursor : MonoBehaviour
    {
        public static HandCursor instance;

        [SerializeField] private SpriteRenderer spriteRenderer;

        [SerializeField] private Sprite openHand;
        [SerializeField] private Sprite pointingFinger;

        [SerializeField] private Transform pointerPosition;

        private Camera mainCamera;
        private Collider2D hitbox;

        private bool isHovering;

        public interface IInteractable
        {
            public void Interact();
            public void EndInteract();
            public void Hover();
            public void EndHover();
        }

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            mainCamera = Camera.main;

            UnityEngine.Cursor.visible = false;
            hitbox = GetComponent<Collider2D>();
        }

        private void Update()
        {
            FollowCursor();
        }

        private void UpdateGraphicsState()
        {
            if (isHovering == true)
                spriteRenderer.sprite = openHand;
            else
                spriteRenderer.sprite = pointingFinger;

            BTween.Squeeze(spriteRenderer.transform, Vector3.one, new Vector2(1.3f, 0.7f), 0.1f);
        }

        public void HideCursor()
        {
            spriteRenderer.enabled = false;
        }

        public void ShowCursor()
        {
            spriteRenderer.enabled = true;
        }

        private void FollowCursor()
        {
            Vector3 cursorPixelPosition = Input.mousePosition;

            cursorPixelPosition.x = Mathf.Clamp(cursorPixelPosition.x, 0, Screen.width);
            cursorPixelPosition.y = Mathf.Clamp(cursorPixelPosition.y, 0, Screen.height);

            Vector3 cursorScreenPosition = new Vector3(cursorPixelPosition.x, cursorPixelPosition.y, 10);

            Vector3 position = mainCamera.ScreenToWorldPoint(cursorScreenPosition);
            transform.position = position;
        }
    }
}
