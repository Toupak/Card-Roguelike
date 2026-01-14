using System;
using Cards.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Frames
{
    public class FrameDisplay : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private FrameController defaultFrameControllerPrefab;
        [SerializeField] private Transform frameControllerParent;
        
        [Space]
        [SerializeField] private Texture commonTexture;
        [SerializeField] private Texture rareTexture;
        [SerializeField] private Texture legendaryTexture;
        [SerializeField] private Texture exoticTexture;

        public FrameController frameController { get; private set; }
        public bool hasFrame => frameController != null;
        
        public FrameController SetupFrame(CardController controller, CardData data)
        {
            SetMaterial(data.rarity, data.frameData.material);
            return SetupFrameController(controller, data.frameData);
        }
        
        private FrameController SetupFrameController(CardController controller, FrameData data)
        {
            frameController = Instantiate(data.frameController != null ? data.frameController : defaultFrameControllerPrefab, frameControllerParent);
            frameController.Setup(controller, data);
            return frameController;
        }
        
        private void SetMaterial(CardData.Rarity rarity, Material material)
        {
            background.material = new Material(material)
            {
                mainTexture = GetTextureFromRarity(rarity)
            };
            background.sprite = null;
        }

        private Texture GetTextureFromRarity(CardData.Rarity rarity)
        {
            switch (rarity)
            {
                case CardData.Rarity.Common:
                    return commonTexture;
                case CardData.Rarity.Rare:
                    return rareTexture;
                case CardData.Rarity.Legendary:
                    return legendaryTexture;
                case CardData.Rarity.Exotic:
                    return exoticTexture;
                default:
                    throw new ArgumentOutOfRangeException(nameof(rarity), rarity, null);
            }
        }
    }
}
