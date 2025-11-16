using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Passives;

namespace Spells.Data.Bongo_And_Combo
{
    public class BongoPassiveStatus : PassiveController
    {
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<ApplyStatusGa>(ApplyStatusReaction, ReactionTiming.POST);
        }
        
        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<ApplyStatusGa>(ApplyStatusReaction, ReactionTiming.POST);
        }

        private void ApplyStatusReaction(ApplyStatusGa applyStatusGa)
        {
            bool targetIsNotMe = applyStatusGa.target != cardController;
            bool targetIsBrother = applyStatusGa.target != null && applyStatusGa.target.passiveHolder != null && applyStatusGa.target.passiveHolder.GetPassive(passiveData) != null;
            bool isStatusCaptured = applyStatusGa.type == StatusType.Captured;
            
            if (targetIsNotMe && targetIsBrother && !applyStatusGa.isBongoStatus && !isStatusCaptured)
            {
                ApplyStatusGa status = new ApplyStatusGa(applyStatusGa.type, applyStatusGa.amount, cardController, cardController);
                status.isBongoStatus = true;
                ActionSystem.instance.AddReaction(status);
            }
        }
    }
}
