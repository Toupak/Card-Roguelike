using Cards.Scripts;
using Inventory.Items.Consumables;
using Inventory.Items.Frames;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Inventory.Items
{
    public class ItemController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private Image icon;
        [SerializeField] public RectTransform regularTooltipPivot;
        [SerializeField] public RectTransform inspectionTooltipPivot;
        
        public RectTransform tooltipPivot => cardMovement.isInspected ? inspectionTooltipPivot : regularTooltipPivot;

        [HideInInspector] public UnityEvent OnKillItem = new UnityEvent();
        
        public RectTransform rectTransform { get; private set; }
        public Vector2 screenPosition => rectTransform.anchoredPosition;
        
        private FollowTarget followTarget;
        private CardRarityDisplay cardRarityDisplay;
        public CardMovement cardMovement { get;  private set; }

        private bool isFrame;
        private FrameCardItem frameCardItem;
        
        private bool isConsumable;
        private ConsumableController consumableController;
        
        public void SetupItem(CardMovement movement)
        {
            rectTransform = GetComponent<RectTransform>();
            followTarget = GetComponent<FollowTarget>();
            cardRarityDisplay = GetComponent<CardRarityDisplay>();
            followTarget.SetTarget(movement);
            
            cardMovement = movement;
        }

        public void SetupAsFrameItem(FrameData frameData)
        {
            isFrame = true;
            gameObject.name = frameData.frameName;
            SetItemName(frameData.frameName);
            SetIcon(frameData.icon);
            SetRarity(frameData.rarity);
            frameCardItem = GetComponent<FrameCardItem>();
            frameCardItem.Setup(frameData);
        }

        public void SetupAsConsumableItem(ConsumableData consumableData)
        {
            isConsumable = true;
            gameObject.name = consumableData.itemName;
            SetItemName(consumableData.itemName);
            SetIcon(consumableData.icon);
            SetRarity(consumableData.rarity);
            consumableController = Instantiate(consumableData.controller, transform);
            consumableController.Setup(this, consumableData);
        }

        private void SetRarity(CardData.Rarity rarity)
        {
            cardRarityDisplay.SetupBackground(rarity);
        }

        private void SetItemName(string newName)
        {
            itemName.text = newName;
        }
        
        private void SetIcon(Sprite sprite)
        {
            icon.sprite = sprite;
        }

        public void KillItem(bool removeFromDeck)
        {
            cardMovement.CurrentSlot.board.DeleteCurrentSlot(cardMovement.SlotIndex);
            OnKillItem?.Invoke();
            Destroy(gameObject);
        }

        public bool CanEquipItem(CardMovement target)
        {
            Debug.Log("CanEquipItem");
            
            if (isFrame)
                return frameCardItem.CanEquipFrame(target);

            if (isConsumable)
            {
                Debug.Log($"CanEquipItem stuff : {consumableController.CanUseConsumable(target)}");
                return consumableController.CanUseConsumable(target);
            }
            
            return false;
        }

        public void EquipItem(CardMovement target)
        {
            Debug.Log("EquipItem");
            
            if (isFrame)
                frameCardItem.EquipFrame(target, () => KillItem(false));

            if (isConsumable)
            {
                Debug.Log("EquipItem use consu");
                consumableController.UseConsumable(target, () => KillItem(true));
            }
        }
    }
}
