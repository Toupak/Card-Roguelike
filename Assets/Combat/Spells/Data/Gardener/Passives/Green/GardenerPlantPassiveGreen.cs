using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;
using UnityEngine;

namespace Combat.Spells.Data.Gardener.Passives.Green
{
    public class GardenerPlantPassiveGreen : PassiveController
    {
        [SerializeField] private int armorGained;
        [SerializeField] private int damageReturned;
        
        private void OnEnable()
        {
            if (armorGained > 0)
                ActionSystem.SubscribeReaction<EndTurnGA>(EndTurnReaction, ReactionTiming.PRE);
            if (damageReturned > 0)
                ActionSystem.SubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.PRE);
        }

        private void OnDisable()
        {
            if (armorGained > 0)
                ActionSystem.UnsubscribeReaction<EndTurnGA>(EndTurnReaction, ReactionTiming.PRE);
            if (damageReturned > 0)
                ActionSystem.UnsubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.PRE);
        }

        private void DealDamageReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.attacker != null && dealDamageGa.GetPackageFromTarget(cardController.tokenParentController) != null)
            {
                DealDamageGA returnDamage = new DealDamageGA(damageReturned, cardController, dealDamageGa.attacker);
                ActionSystem.instance.AddReaction(returnDamage);
            }
        }

        private void EndTurnReaction(EndTurnGA endTurnGa)
        {
            if (endTurnGa.ending == CombatLoop.TurnType.Player)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Armor, armorGained, cardController, cardController.tokenParentController);
                ActionSystem.instance.AddReaction(applyStatusGa);
            }
        }
    }
}
