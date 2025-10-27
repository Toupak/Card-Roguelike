using System;
using System.Collections;
using System.Collections.Generic;
using Board.Script;
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

        private List<Transform> currentTargets = new List<Transform>();
        public List<Transform> Targets => currentTargets;

        private bool isCanceled;
        public bool IsCanceled => isCanceled;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            SpellController.OnCancelSpell.AddListener(DestroyAllTargetingCursor);
        }

        public IEnumerator SelectTargets(Transform startPosition, TargetType targetType, TargetingMode targetingMode, int targetCount = 1)
        {
            Debug.Log("Start Targeting");

            isCanceled = false;
            currentTargets = new List<Transform>();
            previousCursors = new List<TargetingCursor>();
            
            if (targetingMode == TargetingMode.All)
            {
                currentTargets = ComputeTargetAllList(startPosition, targetType);
                yield break;
            }

            int maxTargetCount = ComputeMaxAmountOfTargets(targetType, targetingMode, targetCount);
            Debug.Log($"Target Count : {maxTargetCount}");

            yield return null;//skip a frame to prevent targeting the card that just started the spell
            while (!isCanceled && currentTargets.Count < maxTargetCount)
            {
                if (currentTargetingCursor == null)
                    SpawnTargetingCursor(startPosition);
                
                UpdateTargetingCursorPosition();
                if (Mouse.current.leftButton.wasPressedThisFrame)
                    CheckForTarget();
                if (Mouse.current.rightButton.wasPressedThisFrame)
                    isCanceled = true;
                yield return null;
            }

            DestroyAllTargetingCursor();

            if (isCanceled)
                currentTargets = new List<Transform>();
        }

        private List<Transform> ComputeTargetAllList(Transform current, TargetType targetType)
        {
            List<Transform> list = new List<Transform>();
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

        private List<Transform> RetrieveBoard(CardContainer container)
        {
            List<Transform> list = new List<Transform>();
            foreach (Slot slot in container.Slots)
            {
                if (!slot.IsEmpty)
                    list.Add(slot.transform.GetChild(0));
            }

            return list;
        }

        private int ComputeMaxAmountOfTargets(TargetType targetType, TargetingMode targetingMode, int targetCount)
        {
            switch (targetingMode)
            {
                case TargetingMode.Single:
                    return 1;
                case TargetingMode.Multi:
                    return Math.Min(targetCount, ComputeTargetAllList(transform, targetType).Count);
                case TargetingMode.All:
                    Debug.LogError("error : TargetingMode.All should have been handled by ComputeTargetAllList()");
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetingMode), targetingMode, null);
            }
        }

        private void CheckForTarget()
        {
            Debug.Log($"Targeting : Check for targets : {CursorInfo.instance.currentCardMovement}");

            if (CursorInfo.instance.currentCardMovement != null) //TODO Check for target
            {
                currentTargets.Add(CursorInfo.instance.currentCardMovement.transform);
                previousCursors.Add(currentTargetingCursor);
                currentTargetingCursor = null;
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
    }
}
