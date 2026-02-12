using System.Linq;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;

namespace Combat.Spells.Data.Xordan
{
    public class XordanPassive : PassiveController
    {
        private bool isBulletGainedThisTurn;
        
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.POST);
            ActionSystem.SubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.PRE);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.POST);
            ActionSystem.UnsubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.PRE);
        }

        private void StartTurnReaction(StartTurnGa startTurnGa)
        {
            if (startTurnGa.starting == CombatLoop.TurnType.Player)
                isBulletGainedThisTurn = false;
        }

        private void DealDamageReaction(DealDamageGA dealDamageGa)
        {
            if (!isBulletGainedThisTurn && dealDamageGa.attacker != null && dealDamageGa.attacker == cardController && dealDamageGa.packages.Where((p) => p.target.cardStatus.IsStatusApplied(StatusType.Marker)).ToList().Count > 0)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.BulletAmmo, 1, cardController, cardController);
                ActionSystem.instance.AddReaction(applyStatusGa);
                isBulletGainedThisTurn = true;
            }
        }
    }
}
