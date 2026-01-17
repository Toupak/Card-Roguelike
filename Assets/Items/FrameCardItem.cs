using System;
using Cards.Scripts;
using Frames;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class FrameCardItem : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private Texture commonMainTexture;
        [SerializeField] private Texture rareMainTexture;
        [SerializeField] private Texture legendaryMainTexture;
        [SerializeField] private Texture exoticMainTexture;
        [SerializeField] private Texture maskTexture;
        
        private static readonly int mask = Shader.PropertyToID("_Mask");

        public FrameData data { get; private set; }
        
        public void Setup(FrameData frameData)
        {
            data = frameData;
            
            Material newMaterial = new Material(frameData.material);
            newMaterial.mainTexture = GetTextureFromRarity(frameData.rarity);
            newMaterial.SetTexture(mask, maskTexture);
            
            background.sprite = null;
            background.material = newMaterial;
        }

        private Texture GetTextureFromRarity(CardData.Rarity rarity)
        {
            switch (rarity)
            {
                case CardData.Rarity.Common:
                    return commonMainTexture;
                case CardData.Rarity.Rare:
                    return rareMainTexture;
                case CardData.Rarity.Legendary:
                    return legendaryMainTexture;
                case CardData.Rarity.Exotic:
                    return exoticMainTexture;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null);
            }
        }

        public bool CanEquipItem(CardMovement target)
        {
            return true;
        }

        public void EquipItem(CardMovement target)
        {
            target.cardController.AddFrame(data);    
        }
    }
}
