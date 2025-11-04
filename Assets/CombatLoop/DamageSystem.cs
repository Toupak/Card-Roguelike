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
            ActionSystem.AttachPerformer<HealGa>(HealPerformer);
            ActionSystem.AttachPerformer<DeathGA>(DeathPerformer);

        }

        private void OnDisable()
        {
            ActionSystem.DetachPerformer<DealDamageGA>();
            ActionSystem.DetachPerformer<HealGa>();
            ActionSystem.DetachPerformer<DeathGA>();
        }

        private IEnumerator DealDamagePerformer(DealDamageGA dealDamageGa)
        {
            yield return CardTween.PlayCardAttack(dealDamageGa.attacker, dealDamageGa.target);
            dealDamageGa.target.cardHealth.TakeDamage(dealDamageGa.amount);
        }

        private IEnumerator HealPerformer(HealGa HealGa)
        {
            yield return CardTween.PlayCardAttack(HealGa.attacker, HealGa.target);
            HealGa.target.cardHealth.Heal(HealGa.amount);
        }

        private IEnumerator DeathPerformer(DeathGA deathGa)
        {
            deathGa.target.cardHealth.Dies();
            yield break;
        }
    }
}
