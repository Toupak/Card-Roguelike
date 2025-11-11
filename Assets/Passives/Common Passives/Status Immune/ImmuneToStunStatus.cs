using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UI.Damage_Numbers;
using UnityEngine;

namespace Passives.Common_Passives.Status_Immune
{
    public class ImmuneToStunStatus : PassiveController
    {
        [SerializeField] private StatusType statusType;
        
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
            if (applyStatusGa.target == cardController && applyStatusGa.type == statusType)
            {
                applyStatusGa.NegateEffect();
                DamageNumberFactory.instance.DisplayQuickMessage(cardController.rectTransform.anchoredPosition, "Immune");
            }
        }    
    }
}
