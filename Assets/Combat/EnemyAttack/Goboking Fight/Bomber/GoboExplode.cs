using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.EnemyAttack.Goboking_Fight.Bomber
{
    public class GoboExplode : BaseEnemyBehaviour
    {
        [SerializeField] private int damage;
        [SerializeField] public int selfDamage;
    
        public override IEnumerator ExecuteBehavior()
        {
            Debug.Log("Deal Damage Behaviour");

            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            CardController target = ComputeTarget();
            DealDamageGA damageGa = new DealDamageGA(ComputeCurrentDamage(damage, target), enemyCardController.cardController, target);
            ActionSystem.instance.Perform(damageGa);

            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            DealDamageGA selfDamageGa = new DealDamageGA(selfDamage, enemyCardController.cardController, enemyCardController.cardController);
            ActionSystem.instance.Perform(selfDamageGa);
        }
        
        public override int ComputeWeight()
        {
            bool hasMoreThanOneStack =
                enemyCardController.cardController.cardStatus.IsStatusApplied(StatusType.GobombCharging) &&
                enemyCardController.cardController.cardStatus.currentStacks[StatusType.GobombCharging] >= 1;

            bool hasNeverHadStacks = !enemyCardController.cardController.cardStatus.currentStacks.ContainsKey(StatusType.GobombCharging);
            
            if (hasMoreThanOneStack || hasNeverHadStacks)
                return 0;
            
            return weight;
        }
        
        public override string GetDamageText()
        {
            return $"{ComputeCurrentDamage(damage, null)}";
        }

        public override DamageSystem.DamageType GetDamageType()
        {
            return DamageSystem.DamageType.Crit;
        }
    }
}
