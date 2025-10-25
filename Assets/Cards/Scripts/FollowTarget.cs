using Board.Script;
using UnityEditor;
using UnityEngine;
using Tools = BoomLib.Tools.Tools;

namespace Cards.Scripts
{
    public class FollowTarget : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float rotationAmount;
        [SerializeField] private float rotationSpeed;
        [SerializeField] private float maxAngle;

        [Space]
        [SerializeField] private float stickyMoveDistance;
        [SerializeField] private float stickyMaxDistance;

        private Canvas canvas;
        private CardMovement target;
        
        private Vector3 rotationDelta;
        private Vector3 movementDelta;
        private Vector3 velocity;

        public void SetTarget(CardMovement newTarget)
        {
            canvas = GetComponent<Canvas>();
            target = newTarget;
            target.OnSetNewSlot.AddListener(UpdateSortingOrder);
            target.OnStartDrag.AddListener(UpdateSortingOrder);
            target.OnDrop.AddListener(UpdateSortingOrder);
            UpdateSortingOrder();
        }

        private void UpdateSortingOrder()
        {
            if (target.IsDragging)
                canvas.sortingOrder = 1000;
            else
                canvas.sortingOrder = target.SlotIndex + 1;
        }
        
        private void LateUpdate()
        {
            if (target.ContainerType == Container.ContainerType.Sticky && Vector3.Distance(target.SlotPosition, target.transform.position) <= stickyMaxDistance && target.IsDragging)
                FollowPositionSticky();
            else
                FollowPosition();

            FollowRotation();
        }
        
        private void FollowPositionSticky()
        {
            Vector3 stickyTarget = target.SlotPosition;
            stickyTarget += (target.transform.position - target.SlotPosition).normalized * stickyMoveDistance;

            float distance = (target.SlotPosition - target.transform.position).magnitude;
            float stickySpeed = (speed * 0.01f) * (1.0f - Tools.NormalizeValue(distance, 0.0f, stickyMoveDistance));
            
            transform.position = Vector3.Lerp(transform.position, stickyTarget, Time.deltaTime * stickySpeed);
        }
        
        private void FollowPosition()
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * speed);
        }
        
        private void FollowRotation()
        {
            Vector3 movement = (transform.position - target.transform.position);
            movementDelta = Vector3.Lerp(movementDelta, movement, 25 * Time.deltaTime);
            Vector3 movementRotation = (target.IsDragging ? movementDelta : movement) * rotationAmount;
            rotationDelta = Vector3.Lerp(rotationDelta, movementRotation, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Clamp(rotationDelta.x, -maxAngle, maxAngle));
        }
    }
}
