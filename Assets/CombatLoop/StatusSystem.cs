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
            ActionSystem.AttachPerformer<StunGa>(ApplyStunPerformer);
        }

        private void OnDisable()
        {
            ActionSystem.DetachPerformer<StunGa>();
        }

        private IEnumerator ApplyStunPerformer(StunGa stunGa)
        {
            yield return CardTween.PlayCardAttack(stunGa.attacker, stunGa.target);
            stunGa.target.cardStatus.ApplyStunStacks(stunGa.amount);
        }
    }
}
