using System.Collections.Generic;
using Board.Script;
using BoomLib.UI.Scripts;
using Cards.Scripts;
using Frames;
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
        
        private void Start()
        {
            PlayerInventory.OnUnEquipFrame.AddListener(CreateItem);
            
            inventoryRect = inventoryContainer.GetComponent<RectTransform>();
            buttonRect = inventoryButton.GetComponent<RectTransform>();

            LoadInventoryContent();
            
            //if (PlayerInventory.instance.isEmpty) //TODO : uncomment this
                //HideInventoryCompletely();
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

        public void CreateItem(FrameItem frameItem, Vector3 position)
        {
            CardMovement newCard = Instantiate(cardMovementPrefab, position, Quaternion.identity);
            inventoryContainer.ReceiveCard(newCard);
            
            ItemController controller = CardsVisualManager.instance.SpawnNewItemVisuals(newCard);
            newCard.SetItemController(controller);

            controller.SetupAsFrameItem(frameItem.data);
        }

        private void HideInventoryCompletely()
        {
            inventoryContainer.gameObject.SetActive(false);
            inventoryButton.gameObject.SetActive(false);
        }

        public void DisplayInventory()
        {
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
    }
}
