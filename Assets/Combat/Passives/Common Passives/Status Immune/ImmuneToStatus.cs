using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UI.Damage_Numbers;
using UnityEngine;

namespace Combat.Passives.Common_Passives.Status_Immune
{
    public class ImmuneToStatus : PassiveController
    {
        [SerializeField] private StatusType statusType;
        [SerializeField] private bool immuneToAllStatuses;
        
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<ApplyStatusGa>(ImmuneToStatusReaction, ReactionTiming.PRE);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<ApplyStatusGa>(ImmuneToStatusReaction, ReactionTiming.PRE);
        }
        
        private void ImmuneToStatusReaction(ApplyStatusGa applyStatusGa)
        {
            if (applyStatusGa.target == cardController && immuneToAllStatuses == true)
            {
                applyStatusGa.NegateEffect();
                DamageNumberFactory.instance.DisplayQuickMessage(cardController.rectTransform.anchoredPosition, "Immune");
                return;
            }

            if (applyStatusGa.target == cardController && applyStatusGa.type == statusType)
            {
                applyStatusGa.NegateEffect();
                DamageNumberFactory.instance.DisplayQuickMessage(cardController.rectTransform.anchoredPosition, "Immune");
            }
        }    
    }
}
