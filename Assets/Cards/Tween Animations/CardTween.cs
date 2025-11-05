using System.Collections;
using BoomLib.Tools;
using Cards.Scripts;
using PrimeTween;
using UnityEngine;

//Go Watch : https://www.youtube.com/watch?v=DEkgi2ufrUA&t=4s
namespace Cards.Tween_Animations
{
    public static class CardTween
    {
        public static IEnumerator PlayCardAttack(CardController attacker, CardController target)
        {
            if (attacker == null || target == null)
                yield break;
            
            bool isComplete = false;

            Vector2 startingPosition = attacker.rectTransform.anchoredPosition;
            float distance = 50.0f * (attacker.cardMovement.IsEnemyCard ? 1.0f : -1.0f);
            Sequence.Create()
                .Chain(Tween.UIAnchoredPositionY(attacker.rectTransform, attacker.rectTransform.anchoredPosition.y + distance, 0.4f, Ease.OutElastic))
                .Chain(Tween.UIAnchoredPosition(attacker.rectTransform, target.rectTransform.anchoredPosition, 0.1f, Ease.OutBounce))
                .Group(Tween.Scale(target.transform, Vector3.one * 0.8f, 0.1f, Ease.InOutBounce, 2, CycleMode.Yoyo))
                .Chain(Tween.UIAnchoredPosition(attacker.rectTransform, startingPosition, 0.2f, Ease.OutElastic))
                .ChainCallback(() => isComplete = true);

            yield return new WaitUntil(() => isComplete);
        }

        public static IEnumerator PlayCardIsStun(CardController card)
        {
            bool isComplete = false;
            
            Sequence.Create()
                .Chain(Tween.ShakeLocalPosition(card.rectTransform, (Vector2.one * 15.0f).ToVector3(), 0.2f))
                .ChainDelay(0.5f)
                .ChainCallback(() => isComplete = true);
            
            yield return new WaitUntil(() => isComplete);
        }
    }
}
