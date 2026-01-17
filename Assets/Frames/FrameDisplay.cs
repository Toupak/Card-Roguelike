using System;
using Cards.Scripts;
using Inventory;
using Run_Loop;
using UnityEngine;
using UnityEngine.UI;

namespace Frames
{
    public class FrameDisplay : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private FrameController defaultFrameControllerPrefab;
        [SerializeField] private Transform frameControllerParent;
        [SerializeField] private FrameTabDisplay frameTabDisplayPrefab;
        [SerializeField] private Transform frameTabHolder ;
        
        [Space]
        [SerializeField] private Texture commonTexture;
        [SerializeField] private Texture rareTexture;
        [SerializeField] private Texture legendaryTexture;
        [SerializeField] private Texture exoticTexture;

        public FrameData data { get; private set; }
        public FrameController frameController { get; private set; }
        public bool hasFrame => frameController != null;

        private FrameTabDisplay currentTab;
        
        private bool isStartingMaterialSaved;
        private Material startingMaterial;
        private Sprite startingSprite;
        
        public void RemoveFrame()
        {
            if (!isStartingMaterialSaved)
                return;

            PlayerInventory.instance.UnEquipFrame(data, GetComponent<RectTransform>().anchoredPosition);
            data = null;
            background.sprite = startingSprite;
            background.material = startingMaterial;
            frameController.Remove();
            
            if (currentTab != null)
                currentTab.Remove();
        }

        public FrameController SetupFrame(CardController controller, CardData.Rarity rarity, FrameData frameData)
        {
            if (!isStartingMaterialSaved)
                SaveStartingMaterial();

            data = frameData;
            SetMaterial(rarity, frameData.material);
            PlayerInventory.instance.EquipFrame(data, controller.deckCard);
            CreateTab(data, controller);
            return SetupFrameController(controller, frameData);
        }
        
        private void CreateTab(FrameData frameData, CardController cardController)
        {
            if (currentTab != null)
                currentTab.Remove();
            
            currentTab = Instantiate(frameTabDisplayPrefab, frameTabHolder);
            currentTab.Setup(frameData, cardController);
        }

        private void SaveStartingMaterial()
        {
            startingSprite = background.sprite;
            startingMaterial = background.material;
            isStartingMaterialSaved = true;
        }
        
        private void SetMaterial(CardData.Rarity rarity, Material material)
        {
            background.material = new Material(material) { mainTexture = GetTextureFromRarity(rarity) };
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
        
        private FrameController SetupFrameController(CardController controller, FrameData data)
        {
            frameController = Instantiate(data.frameController != null ? data.frameController : defaultFrameControllerPrefab, frameControllerParent);
            frameController.Setup(controller, data);
            return frameController;
        }
    }
}
