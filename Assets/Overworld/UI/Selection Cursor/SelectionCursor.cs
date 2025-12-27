using UnityEngine;

namespace Overworld.UI.Selection_Cursor
{
    public class SelectionCursor : MonoBehaviour
    {
        [SerializeField] private Transform topLeft;
        [SerializeField] private Transform topRight;
        [SerializeField] private Transform botRight;
        [SerializeField] private Transform botLeft;

        public void Setup(Vector2 size, Vector2 offset)
        {
            topLeft.localPosition = offset + new Vector2(-size.x, size.y);
            topRight.localPosition = offset + new Vector2(size.x, size.y);
            botRight.localPosition = offset + new Vector2(size.x, -size.y);
            botLeft.localPosition = offset + new Vector2(-size.x, -size.y);
        }
    }
}
