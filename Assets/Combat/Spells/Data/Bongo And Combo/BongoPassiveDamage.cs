using System.Linq;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;

namespace Combat.Spells.Data.Bongo_And_Combo
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
            
            if (attackerIsNotMe && attackerIsBrother && !dealDamageGa.isBongoAttack)
            {
                foreach (DealDamageGA.DamagePackage package in dealDamageGa.packages)
                {
                    if (!package.target.cardHealth.IsDead)
                    {
                        DealDamageGA attack = new DealDamageGA(package.amount, cardController, package.target);
                        attack.isBongoAttack = true;
                        ActionSystem.instance.AddReaction(attack);
                    }
                }
            }
        }
    }
}
