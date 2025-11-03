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

        public CursorMode currentMode { get; private set; } = CursorMode.Free;
        
        public Vector2 currentPosition { get; private set; }
        
        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            currentPosition = ComputeCursorPosition();
            
            if (Mouse.current.rightButton.isPressed)
                EventSystem.current.SetSelectedGameObject(null);
        }

        private void FixedUpdate()
        {
            List<RaycastResult> results = new List<RaycastResult>();
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;

            EventSystem.current.RaycastAll(pointerEventData, results);
            
            if (results.Count < 1)
                return;

            List<RaycastResult> containers = results.Where((c) => c.gameObject.CompareTag("Container")).ToList();
            SetLastContainer(containers);
            
            List<RaycastResult> cards = results.Where((c) => c.gameObject.CompareTag("CardMovement")).ToList();
            SetCurrentCard(cards);
        }

        private void SetCurrentCard(List<RaycastResult> cards)
        {
            currentCardMovement = cards.Count > 0 ? cards[0].gameObject.GetComponent<CardMovement>() : null;
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
