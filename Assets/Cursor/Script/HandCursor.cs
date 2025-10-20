using BoomLib.BoomTween;
using BoomLib.Dialog_System;
using BoomLib.Inputs;
using BoomLib.Music_Player.Scripts;
using BoomLib.SFX_Player.Scripts;
using BoomLib.Tools;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class HandCursor : MonoBehaviour
{
    public static HandCursor instance;

    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Sprite openHand;
    [SerializeField] private Sprite pointingFinger;

    private Camera mainCamera;
    private Collider2D hitbox;

    private bool isHovering;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        mainCamera = Camera.main;

        Cursor.visible = false;
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
