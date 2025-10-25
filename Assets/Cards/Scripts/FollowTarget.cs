using System;
using Board.Script;
using BoomLib.BoomTween;
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
        [SerializeField] private float scaleMultiplierOnDrag;
        [SerializeField] private float scaleSpeed;
        
        [Space]
        [SerializeField] private float stickyMoveDistance;
        [SerializeField] private float stickyMaxDistance;

        [Space] 
        [SerializeField] private float manualTiltAmount;
        [SerializeField] private float autoTiltAmount;
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

        private float targetScale = 1.0f;
        private float scaleVelocity;
        
        public void SetTarget(CardMovement newTarget)
        {
            canvas = GetComponent<Canvas>();
            target = newTarget;
            target.OnSetNewSlot.AddListener(UpdateSortingOrder);
            target.OnStartDrag.AddListener(UpdateSortingOrder);
            target.OnDrop.AddListener(UpdateSortingOrder);
            target.OnHover.AddListener(Squeeze);
            Container.OnAnyContainerUpdated.AddListener(UpdateSortingOrder);
            UpdateSortingOrder();
        }

        private void LateUpdate()
        {
            if (target.ContainerType == Container.ContainerType.Sticky && Vector3.Distance(target.SlotPosition, target.transform.position) <= stickyMaxDistance && target.IsDragging)
                FollowPositionSticky();
            else
                FollowPosition();

            FollowRotation();
            
            bool canTilt = !target.IsDragging && target.ContainerType == Container.ContainerType.Hand && target.SlotSiblingCount >= 3;
            
            UpdateHandPositionAndRotationOffsets(canTilt);
            CardTilt(canTilt);
            UpdateScale();
        }

        private void UpdateScale()
        {
            if (target.IsDragging)
                targetScale = scaleMultiplierOnDrag;
            else
                targetScale = 1.0f;

            float current = transform.localScale.x;
            float newScale = Mathf.SmoothDamp(current, targetScale, ref scaleVelocity, scaleSpeed);

            transform.localScale = Vector3.one * newScale;
        }

        private void UpdateHandPositionAndRotationOffsets(bool canTilt)
        {
            int siblingCount = target.SlotSiblingCount;
            float normalizedPosition = Tools.NormalizeValue(target.SlotIndex, 0.0f, siblingCount);
            curveYOffset = (curve.positioning.Evaluate(normalizedPosition) * curve.positioningInfluence) * siblingCount;
            curveYOffset = canTilt ? curveYOffset : 0.0f;
            curveRotationOffset = curve.rotation.Evaluate(normalizedPosition);
        }
        
        private void CardTilt(bool canTilt)
        {
            int siblingCount = Math.Max(target.SlotSiblingCount, 0);
         
            float sine = Mathf.Sin(Time.time + target.SlotIndex) * (target.IsHovering ? 0.2f : 1);
            float cosine = Mathf.Cos(Time.time + target.SlotIndex) * (target.IsHovering ? 0.2f : 1);

            Vector3 offset = transform.position - Input.mousePosition;
            
            float tiltX = target.IsHovering ? ((offset.y * -1) * manualTiltAmount) : 0;
            float tiltY = target.IsHovering ? ((offset.x) * manualTiltAmount) : 0;
            float tiltZ = canTilt ? (curveRotationOffset * (curve.rotationInfluence * siblingCount)) : 0.0f;

            Vector3 currentAngles = tiltParent.eulerAngles;
            float lerpX = Mathf.LerpAngle(currentAngles.x, tiltX + (sine * autoTiltAmount), tiltSpeed * Time.deltaTime);
            float lerpY = Mathf.LerpAngle(currentAngles.y, tiltY + (cosine * autoTiltAmount), tiltSpeed * Time.deltaTime);
            float lerpZ = Mathf.LerpAngle(currentAngles.z, tiltZ, tiltSpeed / 2 * Time.deltaTime);

            tiltParent.eulerAngles = new Vector3(lerpX, lerpY, lerpZ);
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
        
        private void Squeeze()
        {
            StopAllCoroutines();
            StartCoroutine(BTween.Squeeze(tiltParent, Vector3.one, new Vector2(0.95f, 1.05f), 0.1f));
            StartCoroutine(BTween.Shake(tiltParent, 0.1f, 0.1f, true));
        }
        
        private void UpdateSortingOrder()
        {
            if (target.IsDragging)
                canvas.sortingOrder = 1000;
            else
                canvas.sortingOrder = target.SlotIndex + 1;
        }
    }
}
