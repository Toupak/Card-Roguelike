using System.Collections.Generic;
using Board.Script;
using BoomLib.UI.Scripts;
using Cards.Scripts;
using Items;
using PrimeTween;
using Run_Loop;
using UnityEngine;
using FrameItem = Run_Loop.FrameItem;

namespace Inventory
{
    public class InventoryDisplay : MonoBehaviour
    {
        [SerializeField] private CardContainer inventoryContainer;
        [SerializeField] private Button inventoryButton;

        [SerializeField] private CardMovement cardMovementPrefab;

        [Space]
        [SerializeField] private float animationDuration;

        private RectTransform inventoryRect;
        private RectTransform buttonRect;

        private bool isDisplayed;
        
        private void Start()
        {
            PlayerInventory.OnUnEquipFrame.AddListener((frameItem, position) => CreateItem(frameItem, position, false));
            CombatLoop.CombatLoop.OnPlayerPlayStartFirstTurn.AddListener(HideInventoryDuringFight);
            
            inventoryRect = inventoryContainer.GetComponent<RectTransform>();
            buttonRect = inventoryButton.GetComponent<RectTransform>();

            LoadInventoryContent();
            
            if (PlayerInventory.instance.isEmpty)
                HideInventoryCompletely();
        }

        private void LoadInventoryContent()
        {
            List<FrameItem> frames = PlayerInventory.instance.frames;

            Vector3 position = inventoryContainer.transform.position;
            foreach (FrameItem frameItem in frames)
            {
                if (frameItem.target == null)
                {
                    CreateItem(frameItem, position);
                }
            }
        }

        public void CreateItem(FrameItem frameItem, Vector3 position, bool resetPosition = true)
        {
            CardMovement newCard = Instantiate(cardMovementPrefab, position, Quaternion.identity);

            inventoryContainer.ReceiveCard(newCard);
            
            ItemController controller = CardsVisualManager.instance.SpawnNewItemVisuals(newCard);
            newCard.SetItemController(controller);

            controller.SetupAsFrameItem(frameItem.data);

            if (!resetPosition)
            {
                controller.GetComponent<FollowTarget>().SetSlowMode(true);
                controller.transform.position = position;
            }
        }

        private void HideInventoryCompletely()
        {
            inventoryContainer.gameObject.SetActive(false);
            inventoryButton.gameObject.SetActive(false);
        }

        public void DisplayInventory()
        {
            if (isDisplayed)
                return;

            isDisplayed = true;
            Sequence.Create()
                .Group(Tween.UIAnchoredPositionX(inventoryRect, inventoryRect.anchoredPosition.x + 200.0f, animationDuration))
                .Group(Tween.UIAnchoredPositionX(buttonRect, buttonRect.anchoredPosition.x + 200.0f, animationDuration))
                .ChainCallback(() =>
                {
                    inventoryButton.OnClick.RemoveAllListeners();
                    inventoryButton.OnClick.AddListener(HideInventory);
                    inventoryButton.SetText("<");
                });
        }

        public void HideInventory()
        {
            if (!isDisplayed)
                return;
            
            isDisplayed = false;
            Sequence.Create()
                .Group(Tween.UIAnchoredPositionX(inventoryRect, inventoryRect.anchoredPosition.x - 200.0f, animationDuration))
                .Group(Tween.UIAnchoredPositionX(buttonRect, buttonRect.anchoredPosition.x - 200.0f, animationDuration))
                .ChainCallback(() =>
                {
                    inventoryButton.OnClick.RemoveAllListeners();
                    inventoryButton.OnClick.AddListener(DisplayInventory);
                    inventoryButton.SetText(">");
                });
        }

        private void HideInventoryDuringFight()
        {
            if (isDisplayed)
            {
                Sequence.Create()
                    .Group(Tween.UIAnchoredPositionX(inventoryRect, inventoryRect.anchoredPosition.x - 200.0f, animationDuration))
                    .Group(Tween.UIAnchoredPositionX(buttonRect, buttonRect.anchoredPosition.x - 200.0f, animationDuration))
                    .Chain(Tween.UIAnchoredPositionX(inventoryRect, inventoryRect.anchoredPosition.x - 500.0f, animationDuration))
                    .Chain(Tween.UIAnchoredPositionX(buttonRect, buttonRect.anchoredPosition.x - 500.0f, animationDuration));
            }
            else
            {
                Sequence.Create()
                    .Group(Tween.UIAnchoredPositionX(inventoryRect, inventoryRect.anchoredPosition.x - 500.0f, animationDuration))
                    .Group(Tween.UIAnchoredPositionX(buttonRect, buttonRect.anchoredPosition.x - 500.0f, animationDuration));
            }
        }
    }
}
