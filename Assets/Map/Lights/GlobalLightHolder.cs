using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Overworld.Lights
{
    public class GlobalLightHolder : MonoBehaviour
    {
        public static GlobalLightHolder instance;

        private Light2D light2D;
        public Light2D Light2D => light2D;

        private void Awake()
        {
            instance = this;
            light2D = GetComponent<Light2D>();
        }
    }
}
