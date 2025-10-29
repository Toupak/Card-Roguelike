using System;
using System.Collections;
using System.Collections.Generic;
using Board.Script;
using Cards.Scripts;
using CardSlot;
using Cursor.Script;
using Spells.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Spells.Targeting
{
    public class TargetingSystem : MonoBehaviour
    {
        [SerializeField] private CardContainer playerBoard;
        [SerializeField] private CardContainer enemyBoard;
        [SerializeField] private TargetingCursor targetingCursorPrefab;

        public static TargetingSystem instance;
        
        private TargetingCursor currentTargetingCursor;
        private List<TargetingCursor> previousCursors;

        private List<CardMovement> potentialTargets = new List<CardMovement>();
        private List<CardMovement> currentTargets = new List<CardMovement>();
        public List<CardMovement> Targets => currentTargets;

        private bool isCanceled;
        public bool IsCanceled => isCanceled;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            SpellController.OnCancelSpell.AddListener(StopTargeting);
        }

        public IEnumerator SelectTargets(CardMovement castingCard, Transform startingPosition, TargetType targetType, TargetingMode targetingMode, int targetCount = 1)
        {
            Debug.Log("Start Targeting");

            isCanceled = false;
            currentTargets = new List<CardMovement>();
            previousCursors = new List<TargetingCursor>();

            potentialTargets = ComputeTargetAllList(castingCard, targetType);

            if (targetingMode == TargetingMode.All)
            {
                currentTargets = potentialTargets;
                yield break;
            }
            
            StartTargeting();

            int maxTargetCount = ComputeMaxAmountOfTargets(potentialTargets.Count, targetingMode, targetCount);
            Debug.Log($"Target Count : {maxTargetCount}");

            HighlightTargets(potentialTargets);

            yield return null;//skip a frame to prevent targeting the card that just started the spell
            while (!isCanceled && currentTargets.Count < maxTargetCount)
            {
                if (currentTargetingCursor == null)
                    SpawnTargetingCursor(startingPosition);
                
                UpdateTargetingCursorPosition();
                if (Mouse.current.leftButton.wasPressedThisFrame)
                    CheckForTarget(targetType);
                if (Mouse.current.rightButton.wasPressedThisFrame)
                    isCanceled = true;
                yield return null;
            }

            if (isCanceled)
                currentTargets = new List<CardMovement>();
            else
                yield return new WaitForSeconds(0.3f);
            
            StopTargeting();
        }

        private List<CardMovement> ComputeTargetAllList(CardMovement current, TargetType targetType)
        {
            List<CardMovement> list = new List<CardMovement>();
            switch (targetType)
            {
                case TargetType.Ally:
                    list = RetrieveBoard(playerBoard);
                    break;
                case TargetType.Enemy:
                    list = RetrieveBoard(enemyBoard);
                    break;
                case TargetType.Self:
                    list.Add(current);
                    break;
                case TargetType.None:
                    list = RetrieveBoard(playerBoard);
                    list.AddRange(RetrieveBoard(enemyBoard));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetType), targetType, null);
            }

            return list;
        }

        private List<CardMovement> RetrieveBoard(CardContainer container)
        {
            List<CardMovement> list = new List<CardMovement>();
            foreach (Slot slot in container.Slots)
            {
                if (!slot.IsEmpty)
                    list.Add(slot.transform.GetChild(0).GetComponent<CardMovement>());
            }

            return list;
        }

        private int ComputeMaxAmountOfTargets(int potentialTargetsCount, TargetingMode targetingMode, int targetCount)
        {
            switch (targetingMode)
            {
                case TargetingMode.Single:
                    return 1;
                case TargetingMode.Multi:
                    return Math.Min(targetCount, potentialTargetsCount);
                case TargetingMode.All:
                    Debug.LogError("error : TargetingMode.All should have been handled by ComputeTargetAllList()");
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetingMode), targetingMode, null);
            }
        }

        private void CheckForTarget(TargetType targetType)
        {
            Debug.Log($"Targeting : Check for targets : {CursorInfo.instance.currentCardMovement}");

            CardMovement card = CursorInfo.instance.currentCardMovement;
            
            if (card != null && IsSelectedTargetValid(card, targetType))
            {
                currentTargets.Add(card);
                previousCursors.Add(currentTargetingCursor);
                currentTargetingCursor = null;
            }
        }

        private bool IsSelectedTargetValid(CardMovement card, TargetType targetType)
        {
            switch (targetType)
            {
                case TargetType.Ally:
                    return card.IsEnemyCard == false;
                case TargetType.Enemy:
                    return card.IsEnemyCard;
                case TargetType.Self:
                    return false;
                case TargetType.None:
                    return true;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetType), targetType, null);
            }
        }

        private void SpawnTargetingCursor(Transform startPosition)
        {
            currentTargetingCursor = Instantiate(targetingCursorPrefab, startPosition.position, Quaternion.identity, transform);
            currentTargetingCursor.Setup(startPosition);
        }

        private void UpdateTargetingCursorPosition()
        {
            currentTargetingCursor.UpdatePosition();
        }

        private void StartTargeting()
        {
            CursorInfo.instance.SetCursorMode(CursorInfo.CursorMode.Targeting);
        }
        
        private void StopTargeting()
        {
            DestroyAllTargetingCursor();
            StopHighlightTargets(potentialTargets);
            CursorInfo.instance.SetCursorMode(CursorInfo.CursorMode.Free);
        }

        private void DestroyAllTargetingCursor()
        {
            if (previousCursors.Count > 0)
                foreach (TargetingCursor cursor in previousCursors)
                    cursor.DestroyCursor();

            if (currentTargetingCursor != null)
            {
                currentTargetingCursor.DestroyCursor();
                currentTargetingCursor = null;
            }
        }
        
        private void HighlightTargets(List<CardMovement> targets)
        {
            SetHighlightState(targets, true);
        }

        private void StopHighlightTargets(List<CardMovement> targets)
        {
            SetHighlightState(targets, false);
        }

        private void SetHighlightState(List<CardMovement> targets, bool state)
        {
            foreach (CardMovement cardMovement in targets)
            {
                cardMovement.cardController.displayCardEffect.SetTargetState(state);
            }
        }
    }
}
