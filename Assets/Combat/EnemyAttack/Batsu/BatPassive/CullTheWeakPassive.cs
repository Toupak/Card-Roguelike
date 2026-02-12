using ActionReaction;
using ActionReaction.Game_Actions;
using Combat.Passives;
using UnityEngine;

namespace Combat.EnemyAttack.Batsu.BatPassive
{
    public class CullTheWeakPassive : PassiveController
    {
        [SerializeField] private int damage;

        private bool isThisReaction;

        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DealDamageGA>(ExecutePassiveBehaviour, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DealDamageGA>(ExecutePassiveBehaviour, ReactionTiming.POST);
        }

        private void ExecutePassiveBehaviour(DealDamageGA gA)
        {
            DealDamageGA.DamagePackage package = gA.GetPackageFromTarget(cardController);
            
            if (package != null && package.amount > 0)
            {
                DealDamageGA damageGa = new DealDamageGA(cardController.ComputeCurrentDamage(damage), cardController, gA.attacker);
                ActionSystem.instance.AddReaction(damageGa);

                isThisReaction = true;
            }

            if (gA.attacker == cardController && isThisReaction)
            {
                foreach (DealDamageGA.DamagePackage damagePackage in gA.packages)
                {
                    HealGa healGa = new HealGa(damagePackage.amount, cardController, cardController);
                    ActionSystem.instance.AddReaction(healGa);
                }

                isThisReaction = false;
            }
        }
    }
}
