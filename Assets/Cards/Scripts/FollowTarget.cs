using System;
using BoomLib.BoomTween;
using BoomLib.Tools;
using Cards.HandCurves;
using UnityEngine;
using static Board.Script.CardContainer;
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
        [SerializeField] private float selectedHeightOffsetHand;
        [SerializeField] private float selectedHeightOffsetPlayer;
        [SerializeField] private float selectedHeightOffsetEnemy;
        
        [Space] 
        [SerializeField] private float scaleMultiplierOnSelect;
        [SerializeField] private float scaleMultiplierOnDrag;
        [SerializeField] private float scaleSpeed;
        
        [Space]
        [SerializeField] private float stickyMoveFactorNormalized;
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
        public CardMovement Target => target;

        private RectTransform rectTransform;
        
        private Vector3 rotationDelta;
        private Vector3 movementDelta;
        private Vector3 velocity;
        
        private float curveYOffset;
        private float curveRotationOffset;

        private float targetScale = 1.0f;
        private float scaleVelocity;

        private bool isLocked;

        public void SetTarget(CardMovement newTarget)
        {
            rectTransform = GetComponent<RectTransform>();
            canvas = GetComponent<Canvas>();
            target = newTarget;
            target.OnSetNewSlot.AddListener(UpdateSortingOrder);
            target.OnStartDrag.AddListener(UpdateSortingOrder);
            target.OnDrop.AddListener(UpdateSortingOrder);
            target.OnSelected.AddListener(UpdateSortingOrder);
            target.OnDeselected.AddListener(UpdateSortingOrder);
            target.OnHover.AddListener(Squeeze);
            OnAnyContainerUpdated.AddListener(UpdateSortingOrder);
            UpdateSortingOrder();
        }

        private void LateUpdate()
        {
            if (isLocked)
                return;
            
            if (target.ContainerType == ContainerType.Sticky && Vector3.Distance(target.SlotPosition, target.transform.position) <= stickyMaxDistance && target.isDragging)
                FollowPositionSticky();
            else
                FollowPosition();

            FollowRotation();
            
            UpdateHandPositionAndRotationOffsets();
            CardTilt();
            UpdateScale();
        }

        private void UpdateScale()
        {
            if (target.isDragging)
                targetScale = scaleMultiplierOnDrag;
            else if (target.isSelected)
                targetScale = scaleMultiplierOnSelect;
            else
                targetScale = 1.0f;

            float current = transform.localScale.x;
            float newScale = Mathf.SmoothDamp(current, targetScale, ref scaleVelocity, scaleSpeed);

            transform.localScale = Vector3.one * newScale;
        }

        private void UpdateHandPositionAndRotationOffsets()
        {
            bool canMove = !target.isDragging && target.ContainerType == ContainerType.Hand && target.SlotSiblingCount >= 3;
            
            int siblingCount = Mathf.Max(target.SlotSiblingCount, 1);
            float normalizedPosition = Tools.NormalizeValue(target.SlotIndex, 0.0f, siblingCount);
            curveYOffset = (curve.positioning.Evaluate(normalizedPosition) * curve.positioningInfluence) * siblingCount;
            curveYOffset = canMove ? curveYOffset : 0.0f;
            curveRotationOffset = curve.rotation.Evaluate(normalizedPosition);
        }
        
        private void CardTilt()
        {
            int siblingCount = Math.Max(target.SlotSiblingCount, 0);
         
            float sine = Mathf.Sin(Time.time + target.SlotIndex) * (target.isHovering ? 0.2f : 1);
            float cosine = Mathf.Cos(Time.time + target.SlotIndex) * (target.isHovering ? 0.2f : 1);

            Vector3 offset = transform.position - Input.mousePosition;

            float screenSizedManualTiltAmount = manualTiltAmount / (Screen.width / 960.0f);
            float tiltX = target.isHovering && !target.isSelected ? ((offset.y * -1) * screenSizedManualTiltAmount) : 0;
            float tiltY = target.isHovering && !target.isSelected ? ((offset.x) * screenSizedManualTiltAmount) : 0;
            float tiltZ = target.isDragging || target.ContainerType != ContainerType.Hand ? 0.0f : (curveRotationOffset * (curve.rotationInfluence * siblingCount));

            Vector3 currentAngles = tiltParent.eulerAngles;
            float lerpX = Mathf.LerpAngle(currentAngles.x, tiltX + (sine * autoTiltAmount), tiltSpeed * Time.deltaTime);
            float lerpY = Mathf.LerpAngle(currentAngles.y, tiltY + (cosine * autoTiltAmount), tiltSpeed * Time.deltaTime);
            float lerpZ = Mathf.LerpAngle(currentAngles.z, tiltZ, tiltSpeed / 2 * Time.deltaTime);

            tiltParent.eulerAngles = new Vector3(lerpX, lerpY, lerpZ);
        }

        private void FollowPositionSticky()
        {
            Vector3 direction = target.transform.position - target.SlotPosition;
            float distance = direction.magnitude;
            
            Vector3 stickyTarget = target.SlotPosition + direction.normalized * (distance * stickyMoveFactorNormalized);

            transform.position = Vector3.Lerp(transform.position, stickyTarget, Time.deltaTime * speed);
        }
        
        private void FollowPosition()
        {
            float verticalOffset = ComputeVerticalOffset();
            Vector2 targetPosition = target.rectTransform.position.ToVector2() + Vector2.up * verticalOffset;
            Vector3 clampedPosition = target.isDragging ? Tools.ClampPositionInScreen(targetPosition, rectTransform.rect.size) : targetPosition;
            
            rectTransform.position = Vector3.Lerp(rectTransform.position, clampedPosition, Time.deltaTime * speed);
        }

        private float ComputeVerticalOffset()
        {
            float verticalOffset = target.isDragging ? 0.0f : curveYOffset;
                
            if (target.isSelected)
            {
                switch (target.ContainerType)
                {
                    case ContainerType.Hand:
                        verticalOffset += selectedHeightOffsetHand;
                        break;
                    case ContainerType.Enemy:
                        verticalOffset += selectedHeightOffsetEnemy;
                        break;
                    case ContainerType.Board:
                    case ContainerType.Inventory:
                    case ContainerType.Sticky:
                        verticalOffset += selectedHeightOffsetPlayer;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return verticalOffset;
        }
        
        private void FollowRotation()
        {
            Vector3 movement = (transform.position - target.transform.position);
            movementDelta = Vector3.Lerp(movementDelta, movement, 25 * Time.deltaTime);
            Vector3 movementRotation = (target.isDragging ? movementDelta : movement) * rotationAmount;
            rotationDelta = Vector3.Lerp(rotationDelta, movementRotation, rotationSpeed * Time.deltaTime);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Clamp(rotationDelta.x, -maxAngle, maxAngle));
        }
        
        private void Squeeze()
        {
            if (target.isSelected)
                return;
            
            StopAllCoroutines();
            StartCoroutine(BTween.Squeeze(tiltParent, Vector3.one, new Vector2(0.95f, 1.05f), 0.1f));
        }
        
        private void UpdateSortingOrder()
        {
            if (target.isDragging || target.isSelected)
                canvas.sortingOrder = 1000;
            else
                canvas.sortingOrder = target.SlotIndex + 1;
        }

        public void SetSortingOrderAsAbove()
        {
            canvas.sortingOrder = 999;
        }

        public void ResetSortingOrder()
        {
            UpdateSortingOrder();
        }

        public void SetFollowState(bool state)
        {
            isLocked = !state;
        }
    }
}
