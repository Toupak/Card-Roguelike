using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Tween_Animations;
using UnityEngine;

namespace CombatLoop
{
    public class StatusSystem : MonoBehaviour
    {
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
    }
}
