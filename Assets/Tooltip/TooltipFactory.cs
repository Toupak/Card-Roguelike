using UnityEngine;

namespace Tooltip
{
    public class TooltipFactory : MonoBehaviour
    {
        [SerializeField] private TooltipDisplay tooltipPrefab;
        
        public static TooltipFactory instance;

        private void Awake()
        {
            instance = this;
        }

        public TooltipDisplay CreateTooltip()
        {
            TooltipDisplay tooltip = Instantiate(tooltipPrefab, transform);
            tooltip.Setup();
            return tooltip;
        }
    }
}
