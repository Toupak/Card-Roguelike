using System;
using Cards.Scripts;
using Inventory.Items.Frames;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.Items
{
    public class FrameCardItem : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private Texture mainTexture;
        [SerializeField] private Texture maskTexture;
        
        private static readonly int mask = Shader.PropertyToID("_Mask");

        public FrameData data { get; private set; }
        
        public void Setup(FrameData frameData)
        {
            data = frameData;
            
            Material newMaterial = new Material(frameData.material);
            newMaterial.mainTexture = mainTexture;
            newMaterial.SetTexture(mask, maskTexture);
            
            background.sprite = null;
            background.material = newMaterial;
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
