using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Cards.Tween_Animations;
using UI.Damage_Numbers;
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
            
            ActionSystem.SubscribeReaction<DealDamageGA>(DealDamageReactionPost, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.DetachPerformer<DealDamageGA>();
            ActionSystem.DetachPerformer<HealGa>();
            ActionSystem.DetachPerformer<DeathGA>();
            
            ActionSystem.UnsubscribeReaction<DealDamageGA>(DealDamageReactionPost, ReactionTiming.POST);
        }

        private IEnumerator DealDamagePerformer(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.target != null)
            {
                bool isArmored = false;
                
                if (IsTargetDodging(dealDamageGa))
                {
                    dealDamageGa.NegateDamage();
                }
                else if (dealDamageGa.target.cardStatus.IsStatusApplied(StatusType.Armor))
                {
                    isArmored = ComputeArmorReduction(dealDamageGa);
                }
                
                yield return CardTween.PlayCardAttack(dealDamageGa);
                
                if (isArmored)
                    DamageNumberFactory.instance.DisplayQuickMessage(dealDamageGa.target.screenPosition, "Armored");
                
                dealDamageGa.target.cardHealth.TakeDamage(dealDamageGa.amount, dealDamageGa.attacker);
            }
        }

        private bool ComputeArmorReduction(DealDamageGA dealDamageGa)
        {
            int armorStacks = dealDamageGa.target.cardStatus.GetCurrentStackCount(StatusType.Armor);
            int damage = dealDamageGa.amount;

            int finalArmorStacks = Mathf.Max(0, armorStacks - damage);
            dealDamageGa.amount = Mathf.Max(0, damage - armorStacks);

            if (finalArmorStacks != armorStacks)
            {
                ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Armor, armorStacks - finalArmorStacks, dealDamageGa.attacker, dealDamageGa.target);
                ActionSystem.instance.AddReaction(consumeStacksGa);

                return true;
            }
            
            return false;
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
        
        private void DealDamageReactionPost(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.target != null && dealDamageGa.target.cardStatus.IsStatusApplied(StatusType.ReturnDamage))
            {
                DealDamageGA damageGa = new DealDamageGA(1, dealDamageGa.target, dealDamageGa.attacker);
                ActionSystem.instance.AddReaction(damageGa);
            }   
        }
    }
}
