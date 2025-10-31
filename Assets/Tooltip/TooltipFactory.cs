using UnityEngine;

namespace Tooltip
{
    public class TooltipFactory : MonoBehaviour
    {
        [SerializeField] private TooltipDisplay tooltipDisplayPrefab;
        
        public static TooltipFactory instance;

        private void Awake()
        {
            instance = this;
        }

        public TooltipDisplay CreateTooltip()
        {
            return Instantiate(tooltipDisplayPrefab, transform);
        }
    }
}
