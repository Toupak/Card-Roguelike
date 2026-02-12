using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
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
            if (dealDamageGa.attacker == cardController.tokenParentController)
            {
                foreach (DealDamageGA.DamagePackage package in dealDamageGa.packages)
                {
                    if (!package.target.cardHealth.IsDead)
                    {
                        DealDamageGA followAttack = new DealDamageGA(1, cardController, package.target);
                        ActionSystem.instance.AddReaction(followAttack);

                        DeathGA deathGa = new DeathGA(cardController, cardController);
                        ActionSystem.instance.AddReaction(deathGa);
                        
                        break;
                    }
                }
            }
        }
    }
}
