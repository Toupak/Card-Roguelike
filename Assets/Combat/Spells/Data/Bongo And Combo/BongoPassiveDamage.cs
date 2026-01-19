using ActionReaction;
using ActionReaction.Game_Actions;
using Passives;

namespace Spells.Data.Bongo_And_Combo
{
    public class BongoPassiveDamage : PassiveController
    {
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
        }
        
        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
        }

        private void DealDamageReaction(DealDamageGA dealDamageGa)
        {
            bool attackerIsNotMe = dealDamageGa.attacker != cardController;
            bool attackerIsBrother = dealDamageGa.attacker != null && dealDamageGa.attacker.passiveHolder != null && dealDamageGa.attacker.passiveHolder.GetPassive(passiveData) != null;
            bool enemyIsStillAlive = dealDamageGa.target != null && !dealDamageGa.target.cardHealth.IsDead;
            
            if (attackerIsNotMe && attackerIsBrother && enemyIsStillAlive && !dealDamageGa.isBongoAttack)
            {
                DealDamageGA attack = new DealDamageGA(dealDamageGa.amount, cardController, dealDamageGa.target);
                attack.isBongoAttack = true;
                ActionSystem.instance.AddReaction(attack);
            }
        }
    }
}
