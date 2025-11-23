using EnemyAttack;
using TMPro;
using UnityEngine;

namespace Tooltip.Enemy_Intention
{
    public class IntentionTooltip : TooltipDisplay
    {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI mainText;
        
        public void SetupIntentionTooltip(EnemyCardController enemyCardController)
        {
            title.text = $"{enemyCardController.nextbehaviour.behaviourName}";
            mainText.text = ComputeMainText(enemyCardController.nextbehaviour.description, enemyCardController.nextbehaviour.GetDamageText());
        }

        private string ComputeMainText(string description, string damage)
        {
            return description.Replace("%d%", $"{damage}");
        }
    }
}
