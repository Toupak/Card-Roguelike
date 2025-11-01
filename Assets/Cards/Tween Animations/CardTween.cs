using System.Collections;
using Cards.Scripts;
using PrimeTween;
using UnityEngine;

namespace Cards.Tween_Animations
{
    public static class CardTween
    {
        public static IEnumerator PlayCardAttack(CardController attacker, CardController target)
        {
            bool isComplete = false;

            Vector2 startingPosition = attacker.rectTransform.anchoredPosition;
            Sequence.Create()
                .Chain(Tween.UIAnchoredPositionY(attacker.rectTransform, attacker.rectTransform.anchoredPosition.y - 50.0f, 0.4f, Ease.OutElastic))
                .Chain(Tween.UIAnchoredPosition(attacker.rectTransform, target.rectTransform.anchoredPosition, 0.1f, Ease.OutBounce))
                .Chain(Tween.ShakeLocalPosition(attacker.rectTransform, Vector3.one, 0.15f))
                .Group(Tween.PunchLocalPosition(target.transform, Vector3.one, 0.2f))
                .Chain(Tween.UIAnchoredPosition(attacker.rectTransform, startingPosition, 0.2f, Ease.OutElastic))
                .ChainCallback(() => isComplete = true);

            yield return new WaitUntil(() => isComplete);
        }
    }
}
