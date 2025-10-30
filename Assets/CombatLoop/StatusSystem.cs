using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
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
            stunGa.target.cardStatus.ApplyStunStacks(stunGa.amount);
            yield break;
        }
    }
}
