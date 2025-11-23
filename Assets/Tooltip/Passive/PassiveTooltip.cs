using Passives;
using TMPro;
using UnityEngine;

namespace Tooltip.Passive
{
    public class PassiveTooltip : TooltipDisplay
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI mainText;
        
        public void SetupPassiveTooltip(PassiveData data)
        {
            title.text = data.passiveName;
            mainText.text = data.description;
        }
    }
}
