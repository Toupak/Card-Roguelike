using Cards.Scripts;
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
        public CardMovement cardMovement { get;  private set; }
        
        
        public void SetupItem(CardMovement movement)
        {
            rectTransform = GetComponent<RectTransform>();
            followTarget = GetComponent<FollowTarget>();
            followTarget.SetTarget(movement);
            
            cardMovement = movement;
        }

        public void SetupAsFrameItem(FrameData frameData)
        {
            gameObject.name = frameData.frameName;
            SetItemName(frameData.frameName);
            SetIcon(frameData.icon);
            GetComponent<FrameCardItem>().Setup(frameData);
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
            return GetComponent<FrameCardItem>().CanEquipItem(target);
        }

        public void EquipItem(CardMovement target)
        {
            GetComponent<FrameCardItem>().EquipItem(target);
            KillItem(false);
        }
    }
}
