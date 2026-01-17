using System;
using UnityEngine;
using UnityEngine.UI;

namespace Cards.Scripts
{
    public class CardRarityDisplay : MonoBehaviour
    {
        [SerializeField] private Image rarityOverlay;
        
        [Space]
        [SerializeField] private Sprite commonSprite;
        [SerializeField] private Sprite rareSprite;
        [SerializeField] private Sprite legendarySprite;
        [SerializeField] private Sprite exoticSprite;
        
        public void SetupBackground(CardData.Rarity rarity)
        {
            rarityOverlay.sprite = GetBackgroundSprite(rarity);
        }

        private Sprite GetBackgroundSprite(CardData.Rarity rarity)
        {
            switch (rarity)
            {
                case CardData.Rarity.Common:
                    return commonSprite;
                case CardData.Rarity.Rare:
                    return rareSprite;
                case CardData.Rarity.Legendary:
                    return legendarySprite;
                case CardData.Rarity.Exotic:
                    return exoticSprite;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null);
            }
        }
    }
}
