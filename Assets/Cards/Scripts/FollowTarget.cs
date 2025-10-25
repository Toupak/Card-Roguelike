using System;
using Board.Script;
using Cards.HandCurves;
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

        [Space]
        [SerializeField] private float tiltSpeed;
        
        [Space] 
        [SerializeField] private HandCurveData curve;
        [SerializeField] private Transform tiltParent;

        private Canvas canvas;
        private CardMovement target;
        
        private Vector3 rotationDelta;
        private Vector3 movementDelta;
        private Vector3 velocity;
        
        private float curveYOffset;
        private float curveRotationOffset;
        
        public void SetTarget(CardMovement newTarget)
        {
            canvas = GetComponent<Canvas>();
            target = newTarget;
            target.OnSetNewSlot.AddListener(UpdateSortingOrder);
            target.OnStartDrag.AddListener(UpdateSortingOrder);
            target.OnDrop.AddListener(UpdateSortingOrder);
            Container.OnAnyContainerUpdated.AddListener(UpdateSortingOrder);
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

            UpdateHandPositionAndRotationOffsets();
        }

        private void UpdateHandPositionAndRotationOffsets()
        {
            bool canTilt = !target.IsDragging && target.ContainerType == Container.ContainerType.Hand && target.SlotSiblingCount >= 3;
            
            int siblingCount = Math.Max(target.SlotSiblingCount, 0);
            float normalizedPosition = Tools.NormalizeValue(target.SlotIndex, 0, siblingCount);
            curveYOffset = (curve.positioning.Evaluate(normalizedPosition) * curve.positioningInfluence) * siblingCount;
            curveYOffset = canTilt ? curveYOffset : 0.0f;
            curveRotationOffset = curve.rotation.Evaluate(normalizedPosition);
            
            float tiltZ = canTilt ? (curveRotationOffset * (curve.rotationInfluence * siblingCount)) : 0.0f;
            float lerpZ = Mathf.LerpAngle(tiltParent.eulerAngles.z, tiltZ, tiltSpeed / 2 * Time.deltaTime);

            tiltParent.eulerAngles = new Vector3(0.0f, 0.0f, lerpZ);
        }

        private void FollowPositionSticky()
        {
            Vector3 stickyTarget = target.SlotPosition;
            stickyTarget += (target.transform.position - target.SlotPosition).normalized * stickyMoveDistance;

            transform.position = Vector3.Lerp(transform.position, stickyTarget, Time.deltaTime * speed);
        }
        
        private void FollowPosition()
        {
            Vector3 verticalOffset = (Vector3.up * (target.IsDragging ? 0 : curveYOffset));
            transform.position = Vector3.Lerp(transform.position, target.transform.position + verticalOffset, Time.deltaTime * speed);
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
