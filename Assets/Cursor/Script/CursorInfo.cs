using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using Board.Script;
using Cards.Scripts;
using JetBrains.Annotations;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Cursor.Script
{
    public class CursorInfo : MonoBehaviour
    {
        public enum CursorMode
        {
            Free,
            Targeting
        }
        
        [SerializeField] private CanvasScaler canvasScaler;
        
        public static CursorInfo instance;

        [CanBeNull] public CardContainer LastCardContainer { get; private set; } = null;
        [CanBeNull] public CardMovement currentCardMovement { get; private set; } = null;
        [CanBeNull] public CardMovement currentInteractionCard { get; private set; } = null;
        [CanBeNull] public List<CardMovement> currentCardMovementHitStack { get; private set; } = new List<CardMovement>();

        public bool wasSpellButtonHit { get; private set; }

        public CursorMode currentMode { get; private set; } = CursorMode.Free;
        
        public Vector2 currentPosition { get; private set; }
        public bool isDragging { get; private set; }
        public bool isInspecting { get; private set; }
        
        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            currentPosition = ComputeCursorPosition();

            if (Mouse.current.leftButton.wasPressedThisFrame)
                OnLeftClick();
            
            if (Mouse.current.leftButton.wasReleasedThisFrame)
                OnReleaseLeftClick();
            
            if (Mouse.current.rightButton.wasPressedThisFrame)
                OnRightClick();
        }

        private void OnLeftClick()
        {
            if (isInspecting)
                StopInspecting();
            
            if (currentMode != CursorMode.Free)
                return;

            if (!wasSpellButtonHit && currentCardMovement != null && currentCardMovement.canBeDragged)
                StartDragging();
        }
        
        private void OnReleaseLeftClick()
        {
            if (currentMode != CursorMode.Free)
                return;

            if (currentInteractionCard != null && isDragging)
                StopDragging();
        }
        
        private void OnRightClick()
        {
            if (isInspecting)
            {
                StopInspecting();
                return;
            }
            
            if (currentMode != CursorMode.Free || isDragging)
                return;

            if (!isInspecting && currentCardMovement != null && currentCardMovement.canBeInspected)
                StartInspecting();
        }

        private void StartDragging()
        {
            currentInteractionCard = currentCardMovement;
            currentInteractionCard.OnBeginDrag();
            isDragging = true;
        }

        private void StopDragging()
        {
            if (currentInteractionCard != null)
                currentInteractionCard.OnEndDrag();
            currentInteractionCard = null;
            isDragging = false;
        }
        
        private void StartInspecting()
        {
            currentInteractionCard = currentCardMovement;
            currentInteractionCard.OnInspect();
            isInspecting = true;
        }
        
        private void StopInspecting()
        {
            if (currentInteractionCard != null)
                currentInteractionCard.OnDeselect();
            currentInteractionCard = null;
            isInspecting = false;
        }
        
        private void FixedUpdate()
        {
            List<RaycastResult> results = new List<RaycastResult>();
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;

            EventSystem.current.RaycastAll(pointerEventData, results);

            results = results.OrderByDescending((c) => c.depth).ToList();
            wasSpellButtonHit = ComputeSpellButtonHit(results);
            
            List<RaycastResult> containers = results.Where((c) => c.gameObject.CompareTag("Container")).ToList();
            SetLastContainer(containers);
            
            List<RaycastResult> cards = results.Where((c) => c.gameObject.CompareTag("CardMovement")).ToList();
            SetCurrentCard(cards);
        }

        private bool ComputeSpellButtonHit(List<RaycastResult> results)
        {
            int cardsAboveButton = 0;
            foreach (RaycastResult result in results)
            {
                if (result.gameObject.CompareTag("CardMovement"))
                    cardsAboveButton += 1;

                if (result.gameObject.CompareTag("SpellButton"))
                {
                    return cardsAboveButton < 2;
                }
            }

            return false;
        }

        private void SetCurrentCard(List<RaycastResult> cards)
        {
            currentCardMovement = cards.Count > 0 ? cards[0].gameObject.GetComponent<CardMovement>() : null;
            currentCardMovementHitStack = new List<CardMovement>();
            foreach (RaycastResult result in cards)
            {
                currentCardMovementHitStack.Add(result.gameObject.GetComponent<CardMovement>());
            }
        }

        private void SetLastContainer(List<RaycastResult> containers)
        {
            if (containers.Count < 1)
                return;
            
            LastCardContainer = containers[0].gameObject.GetComponent<CardContainer>();
        }

        public void SetCursorMode(CursorMode newMode)
        {
            Debug.Log($"Setting Cursor Mode to : {newMode}");
            currentMode = newMode;
        }
        
        private Vector2 ComputeCursorPosition()
        {
            return new Vector2(Input.mousePosition.x * canvasScaler.referenceResolution.x / Screen.width, Input.mousePosition.y * canvasScaler.referenceResolution.y / Screen.height);
        }
    }
}
