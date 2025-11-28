using Cards.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Frames
{
    public class FrameDisplay : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private FrameController defaultFrameControllerPrefab;

        public FrameController frameController { get; private set; }
        public bool hasFrame => frameController != null;
        
        public FrameController SetupFrame(CardController controller, FrameData data)
        {
            SetMaterial(data.material);
            return SetupFrameController(controller, data);
        }
        
        private FrameController SetupFrameController(CardController controller, FrameData data)
        {
            frameController = Instantiate(data.frameController != null ? data.frameController : defaultFrameControllerPrefab, transform);
            frameController.Setup(controller, data);
            return frameController;
        }
        
        private void SetMaterial(Material material)
        {
            background.material = material;
            background.sprite = null;
        }
    }
}
