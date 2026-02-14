using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Cards.Tween_Animations;
using PrimeTween;
using UI.Damage_Numbers;
using UnityEngine;
using static ActionReaction.Game_Actions.DealDamageGA;
using Random = UnityEngine.Random;

namespace Combat
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
            if (dealDamageGa.packages.Count == 1)
                yield return DealDamageToSingleTarget(dealDamageGa);
            else if (dealDamageGa.packages.Count > 1)
                yield return DealDamageToMultipleTargets(dealDamageGa);
        }

        private IEnumerator DealDamageToSingleTarget(DealDamageGA dealDamageGa)
        {
            bool isArmored = false;
                
            DamagePackage package = dealDamageGa.packages[0];
         
            if (package.target.cardHealth.IsDead || package.originalTarget == null)
                yield break;
            
            if (package.target.cardStatus.IsStatusApplied(StatusType.Marker))
                package.amount += 1;
                
            if (IsTargetDodging(package.target))
                dealDamageGa.NegateDamage(package.target);
            else if (!dealDamageGa.bypassArmor && package.target.cardStatus.IsStatusApplied(StatusType.Armor))
            {
                isArmored = true;
                package.amount = ComputeArmorReduction(dealDamageGa.attacker, package.target, package.amount);
            } 
                
            Sequence damageSequence = ComputeDamageSequence(package, dealDamageGa.attacker, () =>
            {
                if (isArmored)
                    DamageNumberFactory.instance.DisplayQuickMessage(package.target.screenPosition, "Armored");
                
                package.target.cardHealth.TakeDamage(package.amount, dealDamageGa.attacker);
            });
            Sequence attackSequence = CardTween.NewPlayAttackAnimation(dealDamageGa.attacker, package.originalTarget.screenPosition, damageSequence);
            
            yield return new WaitWhile(() => attackSequence.isAlive);
        }

        private Sequence ComputeDamageSequence(DamagePackage damagePackage, CardController attacker, Action callback)
        {
            Sequence damageSequence = damagePackage.isDamageNegated ? CardTween.NewPlayMissedDamageAnimation(damagePackage.target, callback) : CardTween.NewPlayDamageAnimation(damagePackage.target, callback);

            if (damagePackage.isTargetSwitched)
                return CardTween.NewPlayDamageProtectedAnimation(damagePackage.target, damagePackage.originalTarget.screenPosition, attacker.isEnemy, damageSequence);
            
            return damageSequence;
        }
        
        private IEnumerator DealDamageToMultipleTargets(DealDamageGA dealDamageGa)
        {
            Sequence multiDamageSequence = Sequence.Create();

            Vector2 position = Vector2.zero;
            foreach (DamagePackage package in dealDamageGa.packages)
            {
                if (package.target.cardHealth.IsDead)
                    continue;
                
                bool isArmored = false;
                int damage = package.amount;
                
                if (package.target.cardStatus.IsStatusApplied(StatusType.Marker))
                    damage += 1;
                
                if (package.isDamageNegated || IsTargetDodging(package.target))
                {
                    damage = 0;
                }
                else if (!dealDamageGa.bypassArmor && package.target.cardStatus.IsStatusApplied(StatusType.Armor))
                {
                    isArmored = true;
                    damage = ComputeArmorReduction(null, package.target, damage);
                }            
                
                Sequence damageSequence = ComputeDamageSequence(package, dealDamageGa.attacker, () =>
                {
                    if (isArmored)
                        DamageNumberFactory.instance.DisplayQuickMessage(package.target.screenPosition, "Armored");
                
                    package.target.cardHealth.TakeDamage(damage, dealDamageGa.attacker);
                });

                multiDamageSequence.Group(damageSequence);
                position += package.target.screenPosition;
            }
            
            Sequence attackSequence = CardTween.NewPlayCleaveAttackAnimation(dealDamageGa.attacker, position / dealDamageGa.packages.Count, multiDamageSequence);
            
            yield return new WaitWhile(() => attackSequence.isAlive);
        }

        private int ComputeArmorReduction(CardController attacker, CardController target, int damage)
        {
            int armorStacks = target.cardStatus.GetCurrentStackCount(StatusType.Armor);

            int finalArmorStacks = Mathf.Max(0, armorStacks - damage);
            damage = Mathf.Max(0, damage - armorStacks);

            if (finalArmorStacks != armorStacks)
            {
                ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Armor, armorStacks - finalArmorStacks, attacker, target);
                ActionSystem.instance.AddReaction(consumeStacksGa);
            }
            
            return damage;
        }

        private bool IsTargetDodging(CardController target)
        {
            if (!target.cardStatus.IsStatusApplied(StatusType.Dodge))
                return false;

            int dodgeStacks = target.cardStatus.GetCurrentStackCount(StatusType.Dodge);

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
            if (!deathGa.isDeathPrevented)
                deathGa.target.KillCard();
            else
            {
                Sequence fakeDeathAnimation = CardTween.NewPlayFakeDeathAnimation(deathGa.target);
                yield return new WaitWhile(() => fakeDeathAnimation.isAlive);
            }
            yield break;
        }
        
        private void DealDamageReactionPost(DealDamageGA dealDamageGa)
        {
            List<DamagePackage> targets = dealDamageGa.packages.Where((t) => t.target.cardStatus.IsStatusApplied(StatusType.ReturnDamage)).ToList();

            foreach (DamagePackage package in targets)
            {
                if (dealDamageGa.attacker != null)
                {
                    DealDamageGA damageGa = new DealDamageGA(1, package.target, dealDamageGa.attacker);
                    ActionSystem.instance.AddReaction(damageGa);
                }
            }
        }
    }
}
