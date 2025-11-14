using BoomLib.Tools;
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
            Vector3 targetPosition = transform.parent.position;

            if (followTarget.Target.isDragging)
                targetPosition += offset.ToVector3();

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, speed);
        }
    }
}
