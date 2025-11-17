using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Spells.Targeting;
using UnityEngine;

namespace Spells.Data.TheCount
{
    public class CountAttack : SpellController
    {
        private Dictionary<CardController, int> friends = new Dictionary<CardController, int>();
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            
            foreach (CardMovement target in targets)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                
                friends.Add(target.cardController, 0);
                DealDamageGA dealDamageGa = new DealDamageGA(spellData.damage, cardController, target.cardController);
                ActionSystem.instance.Perform(dealDamageGa);
            }
        }
        
        protected override void SubscribeReactions()
        {
            base.SubscribeReactions();
            ActionSystem.SubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
            ActionSystem.SubscribeReaction<StartTurnGa>(StartTurnCountReaction, ReactionTiming.POST);
        }

        protected override void UnsubscribeReactions()
        {
            base.UnsubscribeReactions();
            ActionSystem.UnsubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
            ActionSystem.UnsubscribeReaction<StartTurnGa>(StartTurnCountReaction, ReactionTiming.POST);
        }

        private void DealDamageReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.attacker == cardController && friends.ContainsKey(dealDamageGa.target))
            {
                friends[dealDamageGa.target] = dealDamageGa.amount;

                if (dealDamageGa.amount > 0)
                {
                    ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Blood, dealDamageGa.amount, dealDamageGa.target, cardController);
                    ActionSystem.instance.AddReaction(applyStatusGa);
                    
                    List<CardMovement> enemies = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);
                    DealDamageGA attackRandomEnemy = new DealDamageGA(dealDamageGa.amount, cardController, enemies[Random.Range(0, enemies.Count)].cardController);
                    ActionSystem.instance.AddReaction(attackRandomEnemy);
                }
            }
        }

        private void StartTurnCountReaction(StartTurnGa startTurnGa)
        {
            if (startTurnGa.starting == CombatLoop.CombatLoop.TurnType.Player)
            {
                foreach (KeyValuePair<CardController,int> friend in friends)
                {
                    int stacks = cardController.cardStatus.GetCurrentStackCount(StatusType.Blood);
                    int toBeHealed = Mathf.Min(friend.Value, stacks);
                    
                    HealGa healGa = new HealGa(toBeHealed, cardController, friend.Key);
                    ActionSystem.instance.AddReaction(healGa);

                    ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Blood, toBeHealed, cardController, cardController);
                    ActionSystem.instance.AddReaction(consumeStacksGa);
                }

                friends = new Dictionary<CardController, int>();
            }
        }
    }
}
