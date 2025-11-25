using Localization;
using Status.Data;
using TMPro;
using UnityEngine;

namespace Tooltip.Status
{
    public class StatusTooltip : TooltipDisplay
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI mainText;
        
        public void SetupStatusTooltip(StatusData data)
        {
            title.text = LocalizationSystem.instance.GetStatusTitle(data.localizationKey);
            mainText.text = LocalizationSystem.instance.GetStatusDescription(data.localizationKey);
        }
    }
}
