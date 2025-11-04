using System;
using System.Collections;
using PrimeTween;
using UnityEngine;
using UnityEngine.UI;

namespace Spells
{
    public class AnimateSpellButton : MonoBehaviour
    {
        [SerializeField] private Sprite enabledSprite;
        [SerializeField] private Sprite disabledSprite;
        
        [SerializeField] private Sprite enabledSpriteClicked;
        [SerializeField] private Sprite disabledSpriteClicked;
        
        private SpellButton spellButton;
        
        private Image buttonImage;
        private RectTransform iconImage;

        private bool isPlayingAnimation;
        private bool canBeClicked;

        private void Start()
        {
            spellButton = GetComponent<SpellButton>();
            buttonImage = GetComponent<Image>();
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
            
            canBeClicked = spellButton.spellController.CanCastSpell(spellButton.spellData);

            if (!isPlayingAnimation)
                UpdateButtonSprite();
        }

        private void UpdateButtonSprite()
        {
            buttonImage.sprite = canBeClicked ? enabledSprite : disabledSprite;
        }

        private void OnClickButton()
        {
            buttonImage.sprite = canBeClicked ? enabledSpriteClicked : disabledSpriteClicked;
            iconImage.anchoredPosition = new Vector2(0.0f, -2.0f);
            isPlayingAnimation = true;

            Sequence.Create()
                .Group(Tween.Scale(transform, new Vector3(1.1f, 0.9f, 1.0f), 0.1f, Ease.InOutBounce, 2, CycleMode.Yoyo))
                .ChainCallback(() =>
                {
                    buttonImage.sprite = canBeClicked ? enabledSprite : disabledSprite;
                    iconImage.anchoredPosition = new Vector2(0.0f, 6.0f);
                    isPlayingAnimation = false;
                    UpdateButtonVisuals();
                });
        }
    }
}
