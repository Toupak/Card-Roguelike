using System;
using System.Collections;
using ActionReaction.Game_Actions;
using BoomLib.Tools;
using Cards.Scripts;
using PrimeTween;
using UI.Damage_Numbers;
using UnityEngine;

//Go Watch : https://www.youtube.com/watch?v=DEkgi2ufrUA&t=4s
namespace Cards.Tween_Animations
{
    public static class CardTween
    {
        public static IEnumerator PlayCardStatusApply(ApplyStatusGa applyStatusGa)
        {
            if (applyStatusGa.isTargetSwitched && applyStatusGa.isEffectNegated)
                yield return PlayCardMissAttackProtected(applyStatusGa.attacker, applyStatusGa.originalTarget, applyStatusGa.target);
            else if (applyStatusGa.isEffectNegated)
                yield return PlayCardMissAttack(applyStatusGa.attacker, applyStatusGa.target);
            else if (applyStatusGa.isTargetSwitched)
                yield return PlayPhysicalAttackProtected(applyStatusGa.attacker, applyStatusGa.originalTarget, applyStatusGa.target);
            else
                yield return PlayPhysicalAttack(applyStatusGa.attacker, applyStatusGa.target);
        }

        public static Sequence NewPlayAttackAnimation(CardController attacker, Vector2 targetPosition, Sequence damageSequence)
        {
            if (attacker == null)
                return Sequence.Create();
            
            attacker.SetFollowState(false);
            attacker.SetSpriteAsAbove();
            
            Vector2 startingPosition = attacker.rectTransform.anchoredPosition;
            float distance = 50.0f * (attacker.cardMovement.IsEnemyCard ? 1.0f : -1.0f);
            targetPosition += Vector2.up * distance;
            
            Sequence sequence = Sequence.Create()
                .Chain(Tween.UIAnchoredPositionY(attacker.rectTransform, attacker.rectTransform.anchoredPosition.y + distance, 0.4f, Ease.OutElastic))
                .Chain(Tween.UIAnchoredPosition(attacker.rectTransform, targetPosition, 0.1f, Ease.OutBounce))
                .Group(damageSequence)
                .Chain(Tween.UIAnchoredPosition(attacker.rectTransform, startingPosition, 0.2f, Ease.OutElastic))
                .ChainCallback(() =>
                {
                    attacker.SetFollowState(true);
                    attacker.ResetSpriteOrder();
                });

            return sequence;
        }
        
        public static Sequence NewPlayCleaveAttackAnimation(CardController attacker, Vector2 targetPosition, Sequence damageSequence)
        {
            if (attacker == null)
                return Sequence.Create();
            
            attacker.SetFollowState(false);
            attacker.SetSpriteAsAbove();
            
            Vector2 startingPosition = attacker.rectTransform.anchoredPosition;
            float distance = 50.0f * (attacker.cardMovement.IsEnemyCard ? 1.0f : -1.0f);
            targetPosition += Vector2.up * distance;
            
            Sequence sequence = Sequence.Create()
                .Chain(Tween.UIAnchoredPositionY(attacker.rectTransform, attacker.rectTransform.anchoredPosition.y + distance, 0.4f, Ease.OutElastic))
                .Chain(Tween.UIAnchoredPosition(attacker.rectTransform, targetPosition, 0.1f, Ease.OutBounce))
                .Group(damageSequence)
                .Chain(Tween.UIAnchoredPosition(attacker.rectTransform, startingPosition, 0.2f, Ease.OutElastic))
                .ChainCallback(() =>
                {
                    attacker.SetFollowState(true);
                    attacker.ResetSpriteOrder();
                });

            return sequence;
        }

        public static Sequence NewPlayDamageAnimation(CardController target, Action callback)
        {
            if (target == null)
                return Sequence.Create();
            
            target.SetFollowState(false);

            Sequence sequence = Sequence.Create()
                .ChainCallback(callback)
                .Group(Tween.Scale(target.transform, Vector3.one * 0.7f, 0.1f, Ease.InOutBounce, 2, CycleMode.Yoyo))
                .ChainCallback(() =>
                {
                    target.SetFollowState(true);
                });

            return sequence;
        }
        
        public static Sequence NewPlayMissedDamageAnimation(CardController target, Action callback)
        {
            if (target == null)
                return Sequence.Create();
            
            target.SetFollowState(false);
            
            float targetDistance = 50.0f * (target.cardMovement.IsEnemyCard ? 1.0f : -1.0f);
            Vector2 targetStartingPosition = target.rectTransform.anchoredPosition;
            Sequence sequence = Sequence.Create()
                .ChainCallback(() => DamageNumberFactory.instance.DisplayQuickMessage(targetStartingPosition, "Miss"))
                .ChainCallback(callback)
                .Chain(Tween.UIAnchoredPositionY(target.rectTransform, targetStartingPosition.y + targetDistance * 2.0f, 0.1f, Ease.OutBounce))
                .Chain(Tween.UIAnchoredPosition(target.rectTransform, targetStartingPosition, 0.1f, Ease.OutElastic))
                .ChainCallback(() =>
                {
                    target.SetFollowState(true);
                });

            return sequence;
        }

        public static Sequence NewPlayDamageProtectedAnimation(CardController protector, Vector2 targetPosition, bool isAttackerAnEnemy, Sequence damageSequence)
        {
            if (protector == null)
                return Sequence.Create();
            
            protector.SetFollowState(false);
            protector.SetSpriteAsAbove();
            
            Vector2 protectorStartingPosition = protector.rectTransform.anchoredPosition;
            float distance = 50.0f * (isAttackerAnEnemy ? 1.0f : -1.0f);
            targetPosition += Vector2.up * distance;
            Sequence sequence = Sequence.Create()
                .Chain(Tween.UIAnchoredPosition(protector.rectTransform, targetPosition, 0.1f))
                .Chain(damageSequence)
                .Chain(Tween.UIAnchoredPosition(protector.rectTransform, protectorStartingPosition, 0.2f, Ease.OutElastic))
                .ChainCallback(() =>
                {
                    protector.SetFollowState(true);
                    protector.ResetSpriteOrder();
                });

            return sequence;
        }

        public static IEnumerator PlayPhysicalAttack(CardController attacker, CardController target)
        {
            if (attacker == null || target == null)
                yield break;
            
            attacker.SetFollowState(false);
            attacker.SetSpriteAsAbove();
            target.SetFollowState(false);
            
            bool isComplete = false;

            Vector2 startingPosition = attacker.rectTransform.anchoredPosition;
            float distance = 50.0f * (attacker.cardMovement.IsEnemyCard ? 1.0f : -1.0f);
            Vector2 targetPosition = target.rectTransform.anchoredPosition + Vector2.up * distance;
            Sequence.Create()
                .Chain(Tween.UIAnchoredPositionY(attacker.rectTransform, attacker.rectTransform.anchoredPosition.y + distance, 0.4f, Ease.OutElastic))
                .Chain(Tween.UIAnchoredPosition(attacker.rectTransform, targetPosition, 0.1f, Ease.OutBounce))
                .Group(Tween.Scale(target.transform, Vector3.one * 0.8f, 0.1f, Ease.InOutBounce, 2, CycleMode.Yoyo))
                .Chain(Tween.UIAnchoredPosition(attacker.rectTransform, startingPosition, 0.2f, Ease.OutElastic))
                .ChainCallback(() => isComplete = true);

            yield return new WaitUntil(() => isComplete);
            
            attacker.SetFollowState(true);
            attacker.ResetSpriteOrder();
            target.SetFollowState(true);
        }
        
        public static IEnumerator PlayPhysicalAttackProtected(CardController attacker, CardController target, CardController protector)
        {
            if (attacker == null || target == null)
                yield break;
            
            attacker.SetFollowState(false);
            attacker.SetSpriteAsAbove();
            protector.SetFollowState(false);
            protector.SetSpriteAsAbove();
            target.SetFollowState(false);
            
            bool isComplete = false;

            Vector2 startingPosition = attacker.rectTransform.anchoredPosition;
            Vector2 protectorStartingPosition = protector.rectTransform.anchoredPosition;
            float distance = 50.0f * (attacker.cardMovement.IsEnemyCard ? 1.0f : -1.0f);
            Vector2 targetPosition = target.rectTransform.anchoredPosition + Vector2.up * distance;
            Sequence.Create()
                .Chain(Tween.UIAnchoredPositionY(attacker.rectTransform, attacker.rectTransform.anchoredPosition.y + distance, 0.4f, Ease.OutElastic))
                .Chain(Tween.UIAnchoredPosition(protector.rectTransform, targetPosition, 0.1f, Ease.OutBounce))
                .Chain(Tween.UIAnchoredPosition(attacker.rectTransform, targetPosition, 0.1f, Ease.OutBounce))
                .Chain(Tween.Scale(protector.transform, Vector3.one * 0.8f, 0.1f, Ease.InOutBounce, 2, CycleMode.Yoyo))
                .Chain(Tween.UIAnchoredPosition(attacker.rectTransform, startingPosition, 0.2f, Ease.OutElastic))
                .Group(Tween.UIAnchoredPosition(protector.rectTransform, protectorStartingPosition, 0.2f, Ease.OutElastic))
                .ChainCallback(() => isComplete = true);

            yield return new WaitUntil(() => isComplete);
            
            attacker.SetFollowState(true);
            attacker.ResetSpriteOrder();
            protector.SetFollowState(true);
            protector.ResetSpriteOrder();
            target.SetFollowState(true);
        }

        public static IEnumerator PlayCardIsStun(CardController card)
        {
            bool isComplete = false;
            
            card.SetFollowState(false);
            
            Sequence.Create()
                .Chain(Tween.ShakeLocalPosition(card.rectTransform, (Vector2.one * 15.0f).ToVector3(), 0.2f))
                .ChainDelay(0.5f)
                .ChainCallback(() => isComplete = true);
            
            yield return new WaitUntil(() => isComplete);
            
            card.SetFollowState(true);
        }

        public static Sequence NewPlaySelfAction(CardController card)
        {
            card.SetFollowState(false);

            Sequence sequence = Sequence.Create()
                .Chain(Tween.Scale(card.transform, Vector3.one * 0.8f, 0.1f, Ease.InOutBounce, 2, CycleMode.Yoyo))
                .ChainCallback(() => card.SetFollowState(true));

            return sequence;
        }

        public static IEnumerator PlayCardMissAttack(CardController attacker, CardController target)
        {
            if (attacker == null || target == null)
                yield break;
            
            attacker.SetFollowState(false);
            attacker.SetSpriteAsAbove();
            target.SetFollowState(false);
            
            bool isComplete = false;

            Vector2 startingPosition = attacker.rectTransform.anchoredPosition;
            float attackerDistance = 50.0f * (attacker.cardMovement.IsEnemyCard ? 1.0f : -1.0f);
            float targetDistance = 50.0f * (target.cardMovement.IsEnemyCard ? 1.0f : -1.0f);
            Vector2 targetStartingPosition = target.rectTransform.anchoredPosition;
            Vector2 targetPosition = targetStartingPosition + Vector2.up * targetDistance;
            Sequence.Create()
                .Chain(Tween.UIAnchoredPositionY(attacker.rectTransform, attacker.rectTransform.anchoredPosition.y + attackerDistance, 0.4f, Ease.OutElastic))
                .Chain(Tween.UIAnchoredPosition(attacker.rectTransform, targetPosition, 0.1f, Ease.OutBounce))
                .ChainCallback(() => DamageNumberFactory.instance.DisplayQuickMessage(targetStartingPosition, "Miss"))
                .Chain(Tween.UIAnchoredPositionY(target.rectTransform, targetStartingPosition.y + targetDistance * 2.0f, 0.1f, Ease.OutBounce))
                .Chain(Tween.UIAnchoredPosition(attacker.rectTransform, startingPosition, 0.2f, Ease.OutElastic))
                .Chain(Tween.UIAnchoredPosition(target.rectTransform, targetStartingPosition, 0.1f, Ease.OutElastic))
                .ChainCallback(() => isComplete = true);

            yield return new WaitUntil(() => isComplete);
            
            attacker.SetFollowState(true);
            attacker.ResetSpriteOrder();
            target.SetFollowState(true);
        }
        
        public static IEnumerator PlayCardMissAttackProtected(CardController attacker, CardController target, CardController protector)
        {
            if (attacker == null || target == null)
                yield break;
            
            attacker.SetFollowState(false);
            attacker.SetSpriteAsAbove();
            protector.SetFollowState(false);
            protector.SetSpriteAsAbove();
            target.SetFollowState(false);
            
            bool isComplete = false;

            Vector2 startingPosition = attacker.rectTransform.anchoredPosition;
            Vector2 protectorStartingPosition = protector.rectTransform.anchoredPosition;
            float attackerDistance = 50.0f * (attacker.cardMovement.IsEnemyCard ? 1.0f : -1.0f);
            float targetDistance = 50.0f * (target.cardMovement.IsEnemyCard ? 1.0f : -1.0f);
            Vector2 targetStartingPosition = target.rectTransform.anchoredPosition;
            Vector2 targetPosition = targetStartingPosition + Vector2.up * targetDistance;
            Sequence.Create()
                .Chain(Tween.UIAnchoredPositionY(attacker.rectTransform, attacker.rectTransform.anchoredPosition.y + attackerDistance, 0.4f, Ease.OutElastic))
                .Chain(Tween.UIAnchoredPosition(protector.rectTransform, targetPosition, 0.1f, Ease.OutBounce))
                .Chain(Tween.UIAnchoredPosition(attacker.rectTransform, targetPosition, 0.1f, Ease.OutBounce))
                .ChainCallback(() => DamageNumberFactory.instance.DisplayQuickMessage(targetStartingPosition, "Miss"))
                .Chain(Tween.UIAnchoredPositionY(protector.rectTransform, targetStartingPosition.y + targetDistance * 2.0f, 0.1f, Ease.OutBounce))
                .Chain(Tween.UIAnchoredPosition(attacker.rectTransform, startingPosition, 0.2f, Ease.OutElastic))
                .Chain(Tween.UIAnchoredPosition(protector.rectTransform, protectorStartingPosition, 0.1f, Ease.OutElastic))
                .ChainCallback(() => isComplete = true);

            yield return new WaitUntil(() => isComplete);
            
            attacker.SetFollowState(true);
            attacker.ResetSpriteOrder();
            protector.SetFollowState(true);
            protector.ResetSpriteOrder();
            target.SetFollowState(true);
        }
    }
}
