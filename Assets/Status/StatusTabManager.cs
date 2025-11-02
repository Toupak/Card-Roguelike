using System.Collections.Generic;
using Cards.Scripts;
using UnityEngine;

namespace Status
{
    public class StatusTabManager : MonoBehaviour
    {
        [SerializeField] private StatusTabDisplay statusTabDisplayPrefab;
        [SerializeField] private Transform statusTabHolder;
        
        private CardStatus cardStatus;

        private Dictionary<StatusType, StatusTabDisplay> currentTabs = new Dictionary<StatusType, StatusTabDisplay>();

        private void Start()
        {
            cardStatus = GetComponent<CardStatus>();
            cardStatus.OnUpdateStatus.AddListener(UpdateStatusTabs);
        }

        private void UpdateStatusTabs(StatusType statusType)
        {
            int stackCount = cardStatus.currentStacks.TryGetValue(statusType, out int stack) ? stack : 0;
            bool isInCurrentTabs = currentTabs.ContainsKey(statusType) && currentTabs[statusType] != null;
            
            if (isInCurrentTabs && stackCount < 1)
                RemoveTab(statusType);
            else if (isInCurrentTabs && stackCount > 0)
                UpdateTab(statusType, stackCount);
            else if (!isInCurrentTabs && stackCount > 0)
                CreateTab(statusType, stackCount);
        }

        private void CreateTab(StatusType statusType, int stackCount)
        {
            StatusTabDisplay statusTabDisplay = Instantiate(statusTabDisplayPrefab, statusTabHolder);
            statusTabDisplay.Setup(StatusSystem.instance.GetStatusData(statusType), stackCount);

            currentTabs[statusType] = statusTabDisplay;
        }

        private void RemoveTab(StatusType statusType)
        {
            currentTabs[statusType].Remove();
            currentTabs[statusType] = null;
        }

        private void UpdateTab(StatusType statusType, int stackCount)
        {
            currentTabs[statusType].UpdateStackCount(stackCount);
        }
    }
}
