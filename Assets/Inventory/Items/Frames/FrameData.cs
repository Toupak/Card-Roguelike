using Cards.Scripts;
using UnityEngine;

namespace Inventory.Items.Frames
{
    [CreateAssetMenu(fileName = "FrameData", menuName = "Scriptable Objects/FrameData")]
    public class FrameData : ScriptableObject
    {
        public string frameName;
        public string frameDescription;
        public Sprite icon;
        public CardData.Rarity rarity;
        public Color barColor;
        public Color circleColor;
        
        [Space] 
        public Material material;

        [Space] 
        public FrameController frameController;
        
        [Space] 
        public string localizationKey;
        
        public bool isLowRarity => rarity == CardData.Rarity.Common || rarity == CardData.Rarity.Rare;
    }
}
