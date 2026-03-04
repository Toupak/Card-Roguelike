using System;
using System.Collections.Generic;
using Cards.Scripts;
using Combat;
using Combat.Card_Container.Script;
using Inventory.Items;
using Inventory.Items.Consumables;
using PrimeTween;
using Run_Loop;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Button = BoomLib.UI.Scripts.Button;
using FrameItem = Run_Loop.FrameItem;

namespace Inventory
{
    public class InventoryDisplay : MonoBehaviour
    {
        private enum InventoryDisplayTab
        {
            Frames,
            Consumables
        }
        
        [SerializeField] private CardContainer inventoryContainer;
        [SerializeField] private HorizontalLayoutGroup horizontalLayoutGroup;
        [SerializeField] private Button inventoryButton;
        [SerializeField] private Button framesButton;
        [SerializeField] private Button consumablesButton;
        [SerializeField] private RectTransform inventorySubButtons;
        [SerializeField] private RectTransform energyBar;

        [SerializeField] private CardMovement cardMovementPrefab;

        [Space]
        [SerializeField] private float animationDuration;
        [SerializeField] private float scrollSpeed;

        private RectTransform inventoryContainerRect;
        
        private bool isDisplayed;
        private InventoryDisplayTab currentTab = InventoryDisplayTab.Frames;
        
        private void Start()
        {
            inventoryContainerRect = horizontalLayoutGroup.GetComponent<RectTransform>();
            
            PlayerInventory.OnUnEquipFrame.AddListener((frameItem, position) => CreateFrameItem(frameItem, position, false));
            CombatLoop.OnPlayerPlayStartFirstTurn.AddListener(() =>
            {
                framesButton.gameObject.SetActive(false);
                HideInventory(GoToConsumableTab);
            });
            
            LoadInventoryFrames();
            SetDisplayInitialState();
        }

        private void Update()
        {
            float mouseScroll = Mouse.current.scroll.value.y;

            if (Mathf.Abs(mouseScroll) > 0.01f)
            {
                horizontalLayoutGroup.padding.left += (int)(mouseScroll * scrollSpeed * Time.deltaTime);
                LayoutRebuilder.MarkLayoutForRebuild(inventoryContainerRect);
            }
        }

        private void SetDisplayInitialState()
        {
            if (PlayerInventory.instance.isEmpty)
                HideInventoryCompletely();
            if (PlayerInventory.instance.frames.Count == 0)
                framesButton.gameObject.SetActive(false);
            if (PlayerInventory.instance.consumables.Count == 0)
                consumablesButton.gameObject.SetActive(false);
        }

        private void LoadInventoryFrames()
        {
            List<FrameItem> frames = PlayerInventory.instance.frames;

            Vector3 position = inventoryContainer.transform.position;
            foreach (FrameItem frameItem in frames)
            {
                if (frameItem.target == null)
                {
                    CreateFrameItem(frameItem, position);
                }
            }
        }

        private void CreateFrameItem(FrameItem frameItem, Vector3 position, bool resetPosition = true)
        {
            ItemController controller = RunLoop.instance.DrawFrameToContainer(inventoryContainer);
            controller.SetupAsFrameItem(frameItem.data);

            if (!resetPosition)
            {
                controller.GetComponent<FollowTarget>().SetSlowMode(true);
                controller.transform.position = position;
            }
        }
        
        private void LoadInventoryConsumables()
        {
            Dictionary<ConsumableData, int> consumables = PlayerInventory.instance.consumables;

            Vector3 position = inventoryContainer.transform.position;
            foreach (KeyValuePair<ConsumableData, int> keyValuePair in consumables)
            {
                if (keyValuePair.Key.isStackable)
                    CreateConsumableItem(keyValuePair.Key, keyValuePair.Value);
                else
                {
                    for (int i = 0; i < keyValuePair.Value; i++)
                    {
                        CreateConsumableItem(keyValuePair.Key, 1);
                    }
                }
            }
        }

        private void CreateConsumableItem(ConsumableData data, int amount)
        {
            ItemController controller = RunLoop.instance.DrawConsumableToContainer(inventoryContainer);
            controller.SetupAsConsumableItem(data);
        }

        private void HideInventoryCompletely()
        {
            inventoryContainer.gameObject.SetActive(false);
            inventoryButton.gameObject.SetActive(false);
            framesButton.gameObject.SetActive(false);
            consumablesButton.gameObject.SetActive(false);
        }

        public void ClickDisplayInventory()
        {
            DisplayInventory();
        }

        public void DisplayInventory(Action callback = null)
        {
            if (isDisplayed)
                return;

            isDisplayed = true;
            Sequence.Create()
                .Group(Tween.UIAnchoredPositionX(inventoryContainer.rectTransform, inventoryContainer.rectTransform.anchoredPosition.x + 200.0f, animationDuration))
                .Group(Tween.UIAnchoredPositionX(inventoryButton.rectTransform, inventoryButton.rectTransform.anchoredPosition.x + 200.0f, animationDuration))
                .Group(Tween.UIAnchoredPositionX(inventorySubButtons, inventorySubButtons.anchoredPosition.x + 500.0f, animationDuration))
                .Group(Tween.UIAnchoredPositionX(energyBar, energyBar.anchoredPosition.x + 200.0f, animationDuration))
                .Group(Tween.UIAnchoredPositionY(energyBar, energyBar.anchoredPosition.y + 400.0f, animationDuration))
                .ChainCallback(() =>
                {
                    inventoryButton.OnClick.RemoveAllListeners();
                    inventoryButton.OnClick.AddListener(() => HideInventory());
                    inventoryButton.SetText("<");
                    
                    if (callback != null)
                        callback?.Invoke();
                });
        }

        public void HideInventory(Action callback = null)
        {
            if (!isDisplayed)
            {
                if (callback != null)
                    callback?.Invoke();
                return;
            }
            
            isDisplayed = false;
            Sequence.Create()
                .Group(Tween.UIAnchoredPositionX(inventoryContainer.rectTransform, inventoryContainer.rectTransform.anchoredPosition.x - 200.0f, animationDuration))
                .Group(Tween.UIAnchoredPositionX(inventoryButton.rectTransform, inventoryButton.rectTransform.anchoredPosition.x - 200.0f, animationDuration))
                .Group(Tween.UIAnchoredPositionX(inventorySubButtons, inventorySubButtons.anchoredPosition.x - 500.0f, animationDuration))
                .Group(Tween.UIAnchoredPositionX(energyBar, energyBar.anchoredPosition.x - 200.0f, animationDuration))
                .Group(Tween.UIAnchoredPositionY(energyBar, energyBar.anchoredPosition.y - 400.0f, animationDuration))
                .ChainCallback(() =>
                {
                    inventoryButton.OnClick.RemoveAllListeners();
                    inventoryButton.OnClick.AddListener(() => DisplayInventory());
                    inventoryButton.SetText(">");
                   
                    if (callback != null)
                        callback?.Invoke();
                });
        }

        public void GoToFrameTab()
        {
            if (currentTab == InventoryDisplayTab.Frames)
                return;
            
            Debug.Log("Go To Frame Tab");
            inventoryContainer.ResetContainer();
            LoadInventoryFrames();
            
            currentTab = InventoryDisplayTab.Frames;
        }

        public void GoToConsumableTab()
        {
            if (currentTab == InventoryDisplayTab.Consumables)
                return;
            
            Debug.Log("Go To Consumable Tab");
            inventoryContainer.ResetContainer();
            LoadInventoryConsumables();
            
            currentTab = InventoryDisplayTab.Consumables;
        }
    }
}
