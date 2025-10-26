using System.Collections;
using System.Collections.Generic;
using Spells.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Spells.Targeting
{
    public class TargetingSystem : MonoBehaviour
    {
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

        public IEnumerator SelectTargets(Transform startPosition, TargetType targetType, TargetingMode targetingMode, int targetCount = 1)
        {
            Debug.Log("Start Targeting");

            //TODO implement All targetingMode
            //if (targetingMode == TargetingMode.All) 
                //return ComputeTargetAllList();

            isCanceled = false;
            currentTargets = new List<Transform>();
            previousCursors = new List<TargetingCursor>();

            int maxTargetCount = ComputeMaxAmountOfTargets(targetType, targetingMode, targetCount);
            
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

        private int ComputeMaxAmountOfTargets(TargetType targetType, TargetingMode targetingMode, int targetCount)
        {
            //TODO compute max amount
            return 1;
        }

        private void CheckForTarget()
        {
            Debug.Log("Targeting : Check for targets");

            if (false) //TODO Check for target
            {
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
            foreach (TargetingCursor cursor in previousCursors)
            {
                cursor.DestroyCursor();
            }
            currentTargetingCursor.DestroyCursor();
        }
    }
}
