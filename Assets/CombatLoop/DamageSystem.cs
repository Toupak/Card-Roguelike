using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Cards.Tween_Animations;
using UnityEngine;

namespace CombatLoop
{
    public class DamageSystem : MonoBehaviour
    {
        public enum DamageType
        {
            Physical,
            Heal,
            Crit
        }        
        
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
            if (dealDamageGa.target != null)
            {
                if (IsTargetDodging(dealDamageGa))
                    dealDamageGa.NegateDamage();
                
                yield return CardTween.PlayCardAttack(dealDamageGa);
                
                dealDamageGa.target.cardHealth.TakeDamage(dealDamageGa.amount, dealDamageGa.attacker);
            }
        }

        private bool IsTargetDodging(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.isDamageNegated)
                return true;
            
            if (!dealDamageGa.target.cardStatus.IsStatusApplied(StatusType.Dodge))
                return false;

            int dodgeStacks = dealDamageGa.target.cardStatus.currentStacks[StatusType.Dodge];

            return Random.Range(0.0f, 100.0f) < dodgeStacks * 30.0f;
        }

        private IEnumerator HealPerformer(HealGa HealGa)
        {
            if (HealGa.target != null)
            {
                yield return CardTween.PlayPhysicalAttack(HealGa.attacker, HealGa.target);
                HealGa.target.cardHealth.Heal(HealGa.amount);
            }
        }

        private IEnumerator DeathPerformer(DeathGA deathGa)
        {
            deathGa.target.cardHealth.Dies();
            yield break;
        }
    }
}
