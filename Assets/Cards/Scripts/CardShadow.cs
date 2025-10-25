using UnityEngine;

namespace Cards.Scripts
{
    public class CardShadow : MonoBehaviour
    {
        [SerializeField] private FollowTarget followTarget;
        [SerializeField] private Vector2 offset;
        [SerializeField] private float speed;

        private Vector3 velocity;
        
        private void Update()
        {
            Vector3 targetPosition = followTarget.Target.IsDragging ? offset : Vector3.zero;
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, targetPosition, ref velocity, speed);
        }
    }
}
