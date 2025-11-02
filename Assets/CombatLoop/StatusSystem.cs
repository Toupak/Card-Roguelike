using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Cards.Tween_Animations;
using Spells;
using Spells.Targeting;
using UnityEngine;

namespace CombatLoop
{
    public class StatusSystem : MonoBehaviour
    {
        public static StatusSystem instance;

        private void Awake()
        {
            instance = this;
        }

        private void OnEnable()
        {
            ActionSystem.AttachPerformer<ApplyStatusGa>(ApplyStunPerformer);
            ActionSystem.AttachPerformer<ConsumeStacksGa>(ConsumeStacksPerformer);
        }

        private void OnDisable()
        {
            ActionSystem.DetachPerformer<ApplyStatusGa>();
            ActionSystem.DetachPerformer<ConsumeStacksGa>();
        }

        private IEnumerator ApplyStunPerformer(ApplyStatusGa applyStatusGa)
        {
            yield return CardTween.PlayCardAttack(applyStatusGa.attacker, applyStatusGa.target);
            applyStatusGa.target.cardStatus.ApplyStatusStacks(applyStatusGa.type, applyStatusGa.amount);
        }
        
        private IEnumerator ConsumeStacksPerformer(ConsumeStacksGa consumeStacksGa)
        {
            consumeStacksGa.target.cardStatus.ConsumeStacks(consumeStacksGa.type, consumeStacksGa.amount);
            yield break;
        }

        public List<CardMovement> GetListOfCardsAfflictedByStatus(TargetType targetType, StatusType statusType)
        {
            return TargetingSystem.instance.RetrieveBoard(targetType).Where((c) => IsCardAfflictedByStatus(c.cardController, statusType)).ToList();
        }

        public bool IsCardAfflictedByStatus(CardController cardController, StatusType statusType)
        {
            return cardController.cardStatus.currentStacks.ContainsKey(statusType) &&
                   cardController.cardStatus.currentStacks[statusType] > 0;
        }
    }
}
