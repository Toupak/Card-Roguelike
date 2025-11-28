using UnityEngine;

namespace Tooltip
{
    public class TooltipFactory : MonoBehaviour
    {
        [SerializeField] private MultiTooltipDisplay tooltipPrefab;
        
        public static TooltipFactory instance;

        private void Awake()
        {
            instance = this;
        }

        public MultiTooltipDisplay CreateTooltip()
        {
            MultiTooltipDisplay tooltip = Instantiate(tooltipPrefab, transform);
            return tooltip;
        }
    }
}
