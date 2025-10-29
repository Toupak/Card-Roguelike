using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
using UnityEngine;

namespace CombatLoop
{
    public class DamageSystem : MonoBehaviour
    {
        private void OnEnable()
        {
            ActionSystem.AttachPerformer<DealDamageGA>(DealDamagePerformer);
        }

        private void OnDisable()
        {
            ActionSystem.DetachPerformer<DealDamageGA>();
        }

        private IEnumerator DealDamagePerformer(DealDamageGA dealDamageGa)
        {
            dealDamageGa.target.cardHealth.TakeDamage(dealDamageGa.amount);
            yield break;
        }
    }
}
