using BoomLib.Tools;
using UnityEngine;

namespace Cards.Scripts
{
    public class ParallaxArtwork : MonoBehaviour
    {
        [SerializeField] private CardController cardController;
        [SerializeField] private float parallaxPower;

        private Vector2 velocity;
        private Vector2 center;

        private void Start()
        {
            center = new Vector2(Screen.width / 2.0f, Screen.height / 2.0f);
        }

        private void Update()
        {
            Vector2 mousePosition = ComputeMousePosition();
            mousePosition *= parallaxPower;
            transform.localPosition = Vector2.SmoothDamp(transform.localPosition.ToVector2(), mousePosition, ref velocity, 0.1f);
        }

        private Vector2 ComputeMousePosition()
        {
            return cardController.screenPosition - center;
        }
    }
}
