using Cards.Scripts;
using UnityEngine;

namespace Items.Frames
{
    public class FrameController : MonoBehaviour
    {
        public CardController cardController { get; private set; }
        public FrameData frameData { get; private set; }
        
        public virtual void Setup(CardController controller, FrameData data)
        {
            cardController = controller;
            frameData = data;
        }

        public virtual void Remove()
        {
            Destroy(gameObject);
        }
    }
}
