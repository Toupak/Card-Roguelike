using System.Collections.Generic;
using Frames;
using UnityEngine;
using UnityEngine.Events;

namespace Run_Loop
{
    public class FrameItem
    {
        public FrameData data { get; private set; }
        public DeckCard target;

        public FrameItem(FrameData data, DeckCard target = null)
        {
            this.data = data;
            this.target = target;
        }
    }
    
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private List<FrameData> debugFrames;

        public static UnityEvent<FrameItem> OnEquipFrame = new UnityEvent<FrameItem>();
        public static UnityEvent<FrameItem, Vector3> OnUnEquipFrame = new UnityEvent<FrameItem, Vector3>();
        
        public static PlayerInventory instance;

        public List<FrameItem> frames { get; private set; } = new List<FrameItem>();

        public bool isEmpty => frames.Count < 1;
        
        private void Awake()
        {
            instance = this;

            LoadDebugInventory(); //TODO : comment this
        }

        private void LoadDebugInventory()
        {
            foreach (FrameData frame in debugFrames)
            {
                frames.Add(new FrameItem(frame));
            }
        }

        public void LootFrame(FrameData newFrame)
        {
            frames.Add(new FrameItem(newFrame));
        }

        public void EquipFrame(FrameData newFrame, DeckCard target)
        {
            foreach (FrameItem frameItem in frames)
            {
                if (frameItem.data == newFrame)
                {
                    frameItem.target = target;
                    OnEquipFrame?.Invoke(frameItem);
                    return;
                }
            }
        }

        public void UnEquipFrame(FrameData target, Vector3 position)
        {
            foreach (FrameItem frameItem in frames)
            {
                if (frameItem.data == target)
                {
                    frameItem.target = null;
                    OnUnEquipFrame?.Invoke(frameItem, position);
                    return;
                }
            }
        }
    }
}
