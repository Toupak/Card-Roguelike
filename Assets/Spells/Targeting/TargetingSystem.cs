using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Spells.Data;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Spells.Targeting
{
    public class TargetingSystem : MonoBehaviour
    {
        public static TargetingSystem instance;

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

            isCanceled = false;
            currentTargets = new List<Transform>();
            SpawnTargetingCursor();
            
            while (!isCanceled && currentTargets.Count < targetCount)
            {
                UpdateTargetingCursorPosition();
                if (Mouse.current.leftButton.wasPressedThisFrame)
                    CheckForTarget();
                if (Mouse.current.rightButton.wasPressedThisFrame)
                    isCanceled = true;
                yield return null;
            }

            DestroyTargetingCursor();

            if (isCanceled)
                currentTargets = new List<Transform>();
        }

        private void CheckForTarget()
        {
            
        }

        private void SpawnTargetingCursor()
        {
            
        }

        private void UpdateTargetingCursorPosition()
        {
            
        }

        private void DestroyTargetingCursor()
        {
            
        }
    }
}
