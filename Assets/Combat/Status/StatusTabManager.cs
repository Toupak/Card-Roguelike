using System;
using System.Collections.Generic;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Status
{
    public class StatusTabManager : MonoBehaviour
    {
        [SerializeField] private StatusTabDisplay statusTabDisplayPrefab;
        [SerializeField] private Transform statusTabHolder;

        private CardController cardController;

        private Dictionary<StatusType, StatusTabDisplay> currentTabs = new Dictionary<StatusType, StatusTabDisplay>();

        private void Start()
        {
            cardController = GetComponent<CardController>();
            cardController.cardStatus.OnUpdateStatus.AddListener(UpdateStatusTabs);
        }

        private void UpdateStatusTabs(StatusType statusType, StatusTabModification statusTabModification)
        {
            int stackCount = cardController.cardStatus.GetCurrentStackCount(statusType);

            switch (statusTabModification)
            {
                case StatusTabModification.Create:
                    CreateTab(statusType, stackCount);
                    break;
                case StatusTabModification.Edit:
                    UpdateTab(statusType, stackCount);
                    break;
                case StatusTabModification.Remove:
                    if (StatusSystem.instance.GetStatusData(statusType).neverRemoveTab)
                        UpdateTab(statusType, stackCount);
                    else
                        RemoveTab(statusType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(statusTabModification), statusTabModification, null);
            }
        }

        private void CreateTab(StatusType statusType, int stackCount)
        {
            StatusTabDisplay statusTabDisplay = Instantiate(statusTabDisplayPrefab, statusTabHolder);
            statusTabDisplay.Setup(StatusSystem.instance.GetStatusData(statusType), stackCount, cardController);

            currentTabs[statusType] = statusTabDisplay;
        }

        private void RemoveTab(StatusType statusType)
        {
            if (currentTabs.ContainsKey(statusType))
            {
                if (currentTabs[statusType] != null)
                    currentTabs[statusType].Remove();
                currentTabs[statusType] = null;
            }
        }

        private void UpdateTab(StatusType statusType, int stackCount)
        {
            if (currentTabs.ContainsKey(statusType) && currentTabs[statusType] != null)
                currentTabs[statusType].UpdateStackCount(stackCount);
        }
    }
}
