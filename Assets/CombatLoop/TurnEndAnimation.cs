using System;
using System.Collections;
using Localization;
using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CombatLoop
{
    public class TurnEndAnimation : MonoBehaviour
    {
        [SerializeField] private Image background;
        [SerializeField] private Image botCorner;
        [SerializeField] private Image topCorner;
        [SerializeField] private Image banner;
        [SerializeField] private TextMeshProUGUI text;
        
        private void Start()
        {
            SetDisplayState(false);
        }

        public IEnumerator PlayAnimation(CombatLoop.TurnType currentTurn)
        {
            bool isAnimationOver = false;

            SetDisplayState(true);

            text.text = ComputeBannerText(currentTurn);
            text.rectTransform.anchoredPosition = Vector2.zero;

            Sequence.Create()
                .Group(Tween.Alpha(background, 0.5f, 0.3f))
                .Group(Tween.UIAnchoredPositionX(botCorner.rectTransform, 0.0f, 0.1f, Ease.OutBounce))
                .Group(Tween.UIAnchoredPositionX(topCorner.rectTransform, 0.0f, 0.1f, Ease.OutBounce))
                .Chain(Tween.UIAnchoredPositionX(banner.rectTransform, 0.0f, 0.1f, Ease.OutBounce))
                .Chain(Tween.UIAnchoredPositionX(text.rectTransform, -50, 0.5f, Ease.InElastic))
                .Chain(Tween.UIAnchoredPositionX(banner.rectTransform, 2250.0f, 0.1f, Ease.OutBounce))
                .Group(Tween.UIAnchoredPositionX(botCorner.rectTransform, 2250.0f, 0.1f, Ease.OutBounce))
                .Group(Tween.UIAnchoredPositionX(topCorner.rectTransform, -2250.0f, 0.1f, Ease.OutBounce))
                .Group(Tween.Alpha(background, 0.0f, 0.1f))
                .ChainCallback(() => isAnimationOver = true);
            
            yield return new WaitUntil(() => isAnimationOver);
            
            SetDisplayState(false);
        }

        private string ComputeBannerText(CombatLoop.TurnType currentTurn)
        {
            switch (currentTurn)
            {
                case CombatLoop.TurnType.Preparation:
                    return "Preparation";
                case CombatLoop.TurnType.SetupOver:
                    return "Battle";
                case CombatLoop.TurnType.Player:
                    return LocalizationSystem.instance.GetCombatString("start_turn_message");
                case CombatLoop.TurnType.Enemy:
                    return LocalizationSystem.instance.GetCombatString("end_turn_message");
                default:
                    throw new ArgumentOutOfRangeException(nameof(currentTurn), currentTurn, null);
            }
        }

        private void SetDisplayState(bool state)
        {
            background.gameObject.SetActive(state);
            botCorner.gameObject.SetActive(state);
            topCorner.gameObject.SetActive(state);
            banner.gameObject.SetActive(state);
        }
    }
}
