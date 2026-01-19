using ActionReaction;
using ActionReaction.Game_Actions;
using Combat.Passives;

namespace Combat.Spells.Data.Snuk
{
    public class SnukBabyAttack : PassiveController
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
            if (dealDamageGa.attacker == cardController.tokenParentController && dealDamageGa.target != null && !dealDamageGa.target.cardHealth.IsDead)
            {
                DealDamageGA followAttack = new DealDamageGA(1, cardController, dealDamageGa.target);
                ActionSystem.instance.AddReaction(followAttack);

                DeathGA deathGa = new DeathGA(cardController, cardController);
                ActionSystem.instance.AddReaction(deathGa);
            }
        }
    }
}
