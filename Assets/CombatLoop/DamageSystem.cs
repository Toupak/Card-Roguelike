using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Tween_Animations;
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
            yield return CardTween.PlayCardAttack(dealDamageGa.attacker, dealDamageGa.target);
            dealDamageGa.target.cardHealth.TakeDamage(dealDamageGa.amount);
        }
    }
}
