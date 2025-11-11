using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Cards.Tween_Animations;
using Spells;
using Spells.Targeting;
using Status.Data;
using UnityEngine;

namespace Status
{
    public class StatusSystem : MonoBehaviour
    {
        [SerializeField] private List<StatusData> statusData;

        public static StatusSystem instance;

        private void Awake()
        {
            instance = this;
        }

        private void OnEnable()
        {
            ActionSystem.AttachPerformer<ApplyStatusGa>(ApplyStatusPerformer);
            ActionSystem.AttachPerformer<ConsumeStacksGa>(ConsumeStacksPerformer);
            ActionSystem.AttachPerformer<ApplyPassiveGa>(ApplyPassivePerformer);
            ActionSystem.AttachPerformer<RemovePassiveGa>(RemovePassivePerformer);
        }

        private void OnDisable()
        {
            ActionSystem.DetachPerformer<ApplyStatusGa>();
            ActionSystem.DetachPerformer<ConsumeStacksGa>();
            ActionSystem.DetachPerformer<ApplyPassiveGa>();
            ActionSystem.DetachPerformer<RemovePassiveGa>();
        }

        private IEnumerator ApplyStatusPerformer(ApplyStatusGa applyStatusGa)
        {
            if (applyStatusGa.target != null)
            {
                yield return CardTween.PlayCardAttack(applyStatusGa.attacker, applyStatusGa.target);
                applyStatusGa.target.cardStatus.ApplyStatusStacks(applyStatusGa.type, applyStatusGa.amount);
            }
        }
        
        private IEnumerator ConsumeStacksPerformer(ConsumeStacksGa consumeStacksGa)
        {
            if (consumeStacksGa.target != null)
                consumeStacksGa.wasConsumed = consumeStacksGa.target.cardStatus.ConsumeStacks(consumeStacksGa.type, consumeStacksGa.amount);
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

        public StatusData GetStatusData(StatusType type)
        {
            if (type == StatusType.None)
                return null;
            
            return statusData.Where((s) => s.type == type).ToList().First();
        }
        
        private IEnumerator ApplyPassivePerformer(ApplyPassiveGa applyPassiveGa)
        {
            if (applyPassiveGa.target != null)
                applyPassiveGa.target.passiveHolder.AddPassive(applyPassiveGa.passive);
            
            yield break;
        }
        
        private IEnumerator RemovePassivePerformer(RemovePassiveGa removePassiveGa)
        {
            if (removePassiveGa.target != null)
                removePassiveGa.target.passiveHolder.RemovePassive(removePassiveGa.passive);
            
            yield break;
        }
    }
}
