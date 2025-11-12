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
        public static IEnumerator PlayCardAttack(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.isTargetSwitched && dealDamageGa.isDamageNegated)
                yield return PlayCardMissAttackProtected(dealDamageGa.attacker, dealDamageGa.originalTarget, dealDamageGa.target);
            else if (dealDamageGa.isDamageNegated)
                yield return PlayCardMissAttack(dealDamageGa.attacker, dealDamageGa.target);
            else if (dealDamageGa.isTargetSwitched)
                yield return PlayPhysicalAttackProtected(dealDamageGa.attacker, dealDamageGa.originalTarget, dealDamageGa.target);
            else
                yield return PlayPhysicalAttack(dealDamageGa.attacker, dealDamageGa.target);
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

        public static IEnumerator PlayCardMissAttack(CardController attacker, CardController target)
        {
            if (attacker == null || target == null)
                yield break;
            
            attacker.SetFollowState(false);
            attacker.SetSpriteAsAbove();
            target.SetFollowState(false);
            
            bool isComplete = false;

            Vector2 startingPosition = attacker.rectTransform.anchoredPosition;
            float distance = 50.0f * (attacker.cardMovement.IsEnemyCard ? 1.0f : -1.0f);
            Vector2 targetStartingPosition = target.rectTransform.anchoredPosition;
            Vector2 targetPosition = targetStartingPosition + Vector2.up * distance;
            Sequence.Create()
                .Chain(Tween.UIAnchoredPositionY(attacker.rectTransform, attacker.rectTransform.anchoredPosition.y + distance, 0.4f, Ease.OutElastic))
                .Chain(Tween.UIAnchoredPosition(attacker.rectTransform, targetPosition, 0.1f, Ease.OutBounce))
                .ChainCallback(() => DamageNumberFactory.instance.DisplayQuickMessage(targetStartingPosition, "Miss"))
                .Chain(Tween.UIAnchoredPositionY(target.rectTransform, targetStartingPosition.y - distance * 2.0f, 0.1f, Ease.OutBounce))
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
            float distance = 50.0f * (attacker.cardMovement.IsEnemyCard ? 1.0f : -1.0f);
            Vector2 targetStartingPosition = target.rectTransform.anchoredPosition;
            Vector2 targetPosition = targetStartingPosition + Vector2.up * distance;
            Sequence.Create()
                .Chain(Tween.UIAnchoredPositionY(attacker.rectTransform, attacker.rectTransform.anchoredPosition.y + distance, 0.4f, Ease.OutElastic))
                .Chain(Tween.UIAnchoredPosition(protector.rectTransform, targetPosition, 0.1f, Ease.OutBounce))
                .Chain(Tween.UIAnchoredPosition(attacker.rectTransform, targetPosition, 0.1f, Ease.OutBounce))
                .ChainCallback(() => DamageNumberFactory.instance.DisplayQuickMessage(targetStartingPosition, "Miss"))
                .Chain(Tween.UIAnchoredPositionY(protector.rectTransform, targetStartingPosition.y - distance * 2.0f, 0.1f, Ease.OutBounce))
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
