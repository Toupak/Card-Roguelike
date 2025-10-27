using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Linq;
using Board.Script;
using Cards.Scripts;
using JetBrains.Annotations;
using UnityEngine.InputSystem;

namespace Cursor.Script
{
    public class CursorInfo : MonoBehaviour
    {
        public static CursorInfo instance;

        [CanBeNull] public CardContainer LastCardContainer { get; private set; } = null;
        [CanBeNull] public CardMovement currentCardMovement { get; private set; } = null;
        
        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
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
    }
}
