using PrimeTween;
using UnityEngine;

namespace Map.UI.Selection_Cursor
{
    public class SelectionCursor : MonoBehaviour
    {
        [SerializeField] private Transform topLeft;
        [SerializeField] private Transform topRight;
        [SerializeField] private Transform botRight;
        [SerializeField] private Transform botLeft;

        [Space] 
        [SerializeField] private float endValue;
        [SerializeField] private float duration;
        [SerializeField] private Ease ease;
        
        public void Setup(Vector2 size, Vector2 offset)
        {
            topLeft.localPosition = offset + new Vector2(-size.x, size.y);
            topRight.localPosition = offset + new Vector2(size.x, size.y);
            botRight.localPosition = offset + new Vector2(size.x, -size.y);
            botLeft.localPosition = offset + new Vector2(-size.x, -size.y);
        }

        private void Start()
        {
            Sequence.Create(cycles:-1, cycleMode: Sequence.SequenceCycleMode.Yoyo)
                .Chain(Tween.Scale(transform, endValue, duration, ease));
        }
    }
}
