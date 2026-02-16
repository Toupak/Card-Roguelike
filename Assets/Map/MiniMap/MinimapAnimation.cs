using PrimeTween;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Map.MiniMap
{
    public class MinimapAnimation : MonoBehaviour
    {
        [SerializeField] private Camera minimapCamera;
        [SerializeField] private SpriteRenderer borderImage;
        [SerializeField] private RawImage miniMapImage;
        
        [SerializeField] private Vector2 targetPosition;
        [SerializeField] private float targetSizeFactor;
        [SerializeField] private float transitionDuration;

        private float startingFieldOfView;
        
        private Vector2 miniMapStartingPosition;
        private Vector2 miniMapStartingSize;
        
        private bool isSmallMode = true;
        
        private void Start()
        {
            startingFieldOfView = minimapCamera.orthographicSize;
            miniMapStartingPosition = miniMapImage.rectTransform.anchoredPosition;
            miniMapStartingSize = miniMapImage.rectTransform.sizeDelta;
        }

        private void Update()
        {
            if (Keyboard.current.tabKey.wasPressedThisFrame)
            {
                isSmallMode = !isSmallMode;

                if (isSmallMode)
                    GoToSmallMode();
                else
                    GoToBigMode();
            }
        }

        private void GoToBigMode()
        {
            Sequence.Create()
                .Group(Tween.UIAnchoredPosition(miniMapImage.rectTransform, targetPosition, transitionDuration))
                .Group(Tween.UISizeDelta(miniMapImage.rectTransform, miniMapStartingSize * targetSizeFactor, transitionDuration))
                .Group(Tween.Alpha(borderImage, 0.0f, transitionDuration))
                .Group(Tween.CameraOrthographicSize(minimapCamera, startingFieldOfView * targetSizeFactor, transitionDuration));
        }

        private void GoToSmallMode()
        {
            Sequence.Create()
                .Group(Tween.UIAnchoredPosition(miniMapImage.rectTransform, miniMapStartingPosition, transitionDuration))
                .Group(Tween.UISizeDelta(miniMapImage.rectTransform, miniMapStartingSize, transitionDuration))
                .Group(Tween.Alpha(borderImage, 1.0f, transitionDuration))
                .Group(Tween.CameraOrthographicSize(minimapCamera, startingFieldOfView, transitionDuration));
        }
    }
}
