using System;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Cards.Scripts
{
    public class CardHolographicDisplay : MonoBehaviour
    {
        [SerializeField] private Image background;

        [Space] 
        [SerializeField] private Material grayHolographicMaterial;
        [SerializeField] private Material greenHolographicMaterial;
        [SerializeField] private Material colorfulHolographicMaterial;
        [SerializeField] private Material redHolographicMaterial;
        
        public enum HolographicType
        {
            Regular,
            Gray,
            Green,
            Colorful,
            Red
        }

        public void SetHolographicMaterial(HolographicType holographicType)
        {
            if (holographicType == HolographicType.Regular)
                return;

            switch (holographicType)
            {
                case HolographicType.Gray:
                    SetMaterial(grayHolographicMaterial);
                    break;
                case HolographicType.Green:
                    SetMaterial(greenHolographicMaterial);
                    break;
                case HolographicType.Colorful:
                    SetMaterial(colorfulHolographicMaterial);
                    break;
                case HolographicType.Red:
                    SetMaterial(redHolographicMaterial);
                    break;
                case HolographicType.Regular:
                default:
                    throw new ArgumentOutOfRangeException(nameof(holographicType), holographicType, null);
            }
        }

        private void SetMaterial(Material material)
        {
            background.material = material;
            background.sprite = null;
        }
    }
}
