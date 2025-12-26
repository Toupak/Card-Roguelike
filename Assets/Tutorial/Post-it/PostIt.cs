using PrimeTween;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tutorial.Post_it
{
    public class PostIt : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        private RectTransform rectTransform;
        private Animator animator;

        private bool isRemoved;
    
        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            animator = GetComponent<Animator>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (isRemoved)
                return;
            
            animator.Play("Hover");
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isRemoved)
                return;

            animator.Play("Idle");
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (isRemoved)
                return;

            isRemoved = true;
            animator.Play("Remove");

            FallAndDelete();
        }

        private void FallAndDelete()
        {
            float target = -200.0f - Screen.height;
            
            Sequence.Create()
                .Chain(Tween.UIAnchoredPositionY(rectTransform, target, 0.75f, Ease.InExpo))
                .ChainCallback(() =>
                {
                    Destroy(gameObject);
                });
        }
    }
}
