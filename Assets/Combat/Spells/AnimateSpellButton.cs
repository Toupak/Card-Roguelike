using System;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace Combat.Spells
{
    public class AnimateSpellButton : MonoBehaviour
    {
        public enum ButtonState
        {
            Enabled,
            Disabled,
            Shiny,
            Silenced
        }
        
        [SerializeField] private Image buttonImage;
        [SerializeField] private Image silencedIcon;
        
        [Space]
        [SerializeField] private Sprite enabledSprite;
        [SerializeField] private Sprite enabledSpriteClicked;
     
        [Space]
        [SerializeField] private Sprite disabledSprite;
        [SerializeField] private Sprite disabledSpriteClicked;
        
        [Space]
        [SerializeField] private Sprite shinySprite;
        [SerializeField] private Sprite shinySpriteClicked;

        private SpellButton spellButton;
        private RectTransform iconImage;

        private bool isPlayingAnimation;

        private ButtonState currentState;

        private void Start()
        {
            spellButton = GetComponent<SpellButton>();
            iconImage = spellButton.ButtonIcon.GetComponent<RectTransform>();
            
            spellButton.OnClickSpellButton?.AddListener(OnClickButton);
        }

        private void Update()
        {
            UpdateButtonVisuals();
        }

        private void UpdateButtonVisuals()
        {
            if (spellButton.spellController == null)
                return;

            currentState = ComputeButtonState();

            if (!isPlayingAnimation)
                UpdateButtonSprite(false);
        }
        
        private ButtonState ComputeButtonState()
        {
            if (spellButton.spellController.isSilenced)
                return ButtonState.Silenced;

            if (spellButton.spellController.IsShiny)
                return ButtonState.Shiny;

            if (spellButton.spellController.CanCastSpell())
                return ButtonState.Enabled;

            return ButtonState.Disabled;
        }

        private void UpdateButtonSprite(bool displayAsClicked)
        {
            switch (currentState)
            {
                case ButtonState.Enabled:
                    buttonImage.sprite = displayAsClicked ? enabledSpriteClicked : enabledSprite;
                    silencedIcon.gameObject.SetActive(false);
                    break;
                case ButtonState.Disabled:
                    buttonImage.sprite = displayAsClicked ? disabledSpriteClicked : disabledSprite;
                    silencedIcon.gameObject.SetActive(false);
                    break;
                case ButtonState.Shiny:
                    buttonImage.sprite = displayAsClicked ? shinySpriteClicked : shinySprite;
                    silencedIcon.gameObject.SetActive(false);
                    break;
                case ButtonState.Silenced:
                    buttonImage.sprite = disabledSprite;
                    silencedIcon.gameObject.SetActive(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnClickButton()
        {
            UpdateButtonSprite(true);
            iconImage.anchoredPosition = new Vector2(0.0f, -1.1f);
            isPlayingAnimation = true;

            Sequence.Create()
                .Group(Tween.Scale(transform, new Vector3(1.1f, 0.9f, 1.0f), 0.05f, Ease.InOutBounce, 2, CycleMode.Yoyo))
                .ChainCallback(() =>
                {
                    isPlayingAnimation = false;
                    iconImage.anchoredPosition = new Vector2(0.0f, 4.1f);
                    UpdateButtonVisuals();
                });
        }
    }
}
