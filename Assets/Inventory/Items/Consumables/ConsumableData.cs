using Cards.Scripts;
using UnityEngine;

namespace Inventory.Items.Consumables
{
    [CreateAssetMenu(fileName = "ConsumableData", menuName = "Scriptable Objects/ConsumableData")]
    public class ConsumableData : ScriptableObject
    {
        public string itemName;
        public string itemDescription;
        public Sprite icon;
        public CardData.Rarity rarity;

        public int maxAmount = -1;
        
        public bool isStackable;
        public int maxStackCount = -1;
        
        public ConsumableController controller;
    }
}
