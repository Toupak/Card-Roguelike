using System.Collections.Generic;
using Frames;
using UnityEngine;

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
    }
}
