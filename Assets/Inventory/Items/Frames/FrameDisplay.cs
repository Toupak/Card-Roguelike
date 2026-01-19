using System;
using Cards.Scripts;
using Run_Loop;
using UnityEngine;
using UnityEngine.UI;

namespace Items.Frames
{
    public class FrameDisplay : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private FrameController defaultFrameControllerPrefab;
        [SerializeField] private Transform frameControllerParent;
        [SerializeField] private FrameTabDisplay frameTabDisplayPrefab;
        [SerializeField] private Transform frameTabHolder ;

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

            PlayerInventory.instance.UnEquipFrame(data, transform.position);
            data = null;
            
            background.sprite = startingSprite;
            background.material = startingMaterial;
            frameController.Remove();
            
            if (currentTab != null)
                currentTab.Remove();
        }

        public FrameController SetupFrame(CardController controller, FrameData frameData)
        {
            if (!isStartingMaterialSaved)
                SaveStartingMaterial();

            data = frameData;
            SetMaterial(frameData.material);
            PlayerInventory.instance.EquipFrame(data, controller.deckCard);
            CreateTab(data, controller);
            return SetupFrameController(controller);
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
        
        private void SetMaterial(Material material)
        {
            background.material = material;
            background.sprite = null;
        }

        private FrameController SetupFrameController(CardController controller)
        {
            frameController = Instantiate(data.frameController != null ? data.frameController : defaultFrameControllerPrefab, frameControllerParent);
            frameController.Setup(controller, data);
            return frameController;
        }
    }
}
