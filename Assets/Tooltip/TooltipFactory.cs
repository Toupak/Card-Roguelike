using UnityEngine;
using UnityEngine.UI;

namespace Tooltip
{
    public class TooltipFactory : MonoBehaviour
    {
        [SerializeField] private TooltipDisplay tooltipDisplayPrefab;
        
        public static TooltipFactory instance;

        private CanvasScaler canvasScaler;
        
        private void Awake()
        {
            instance = this;
        }
        
        private void Start()
        {
            canvasScaler = GetComponentInParent<CanvasScaler>();
        }

        public TooltipDisplay CreateTooltip()
        {
            TooltipDisplay tooltipDisplay = Instantiate(tooltipDisplayPrefab, transform); 
            tooltipDisplay.SetCanvasScaler(canvasScaler);
            return tooltipDisplay;
        }
    }
}
