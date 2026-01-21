using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.EnemyAttack.Goboking_Fight.Bomber
{
    public class GoboChargeUp : BaseEnemyBehaviour
    {
        [SerializeField] public int turnsToChargeUp;
    
        public override void Setup(EnemyCardController controller)
        {
            base.Setup(controller);

            ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.GobombCharging, turnsToChargeUp, enemyCardController.cardController, enemyCardController.cardController);
            
            if (ActionSystem.instance.IsPerforming)
                ActionSystem.instance.AddReaction(applyStatusGa);
            else
                ActionSystem.instance.Perform(applyStatusGa);
        }
        
        public override IEnumerator ExecuteBehavior()
        {
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
        }
        
        public override int ComputeWeight()
        {
            bool hasNoStacks = !enemyCardController.cardController.cardStatus.IsStatusApplied(StatusType.GobombCharging);
            bool hadStacksEarlier = enemyCardController.cardController.cardStatus.currentStacks.ContainsKey(StatusType.GobombCharging);
            
            if (hasNoStacks && hadStacksEarlier)
                return 0;
            
            return weight;
        }
    }
}