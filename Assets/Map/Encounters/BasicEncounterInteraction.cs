using System.Collections;
using BoomLib.BoomTween;
using BoomLib.Dialog_System;
using Character_Selection.Character;
using Character_Selection.Character.Dialog;
using Combat.Card_Container.Script;
using Map.MiniMap;
using PrimeTween;
using Run_Loop;
using UnityEngine;
using UnityEngine.UI;

namespace Map.Encounters
{
    public class BasicEncounterInteraction : DialogStarter
    {
        [SerializeField] private Canvas backgroundCanvas;
        [SerializeField] private Canvas cardsCanvas;
        [SerializeField] private Image background;
        [SerializeField] private Image blackScreen;
        
        [Space] 
        [SerializeField] protected CardContainer handContainer;

        private bool isScreenOpen;
        private Vector2 backgroundSize;
        protected bool isSelectionValidated;
        
        protected override void Start()
        {
            base.Start();
            Setup();
        }

        protected virtual void Setup()
        {
            backgroundCanvas.gameObject.SetActive(false);
            cardsCanvas.gameObject.SetActive(false);
            backgroundSize = background.rectTransform.sizeDelta;
        }

        protected override void StartDialog()
        {
            DialogManager.instance.StartDialog(dialogData.DialogTexts, dialogPosition, OpenScreen);
        }

        private void OpenScreen()
        {
            if (!isScreenOpen)
                StartCoroutine(OpenScreenCoroutine());
        }

        private IEnumerator OpenScreenCoroutine()
        {
            isScreenOpen = true;
            CharacterSingleton.instance.LockPlayer();
            MinimapBuilder.instance.SetMinimapState(false);
            isSelectionValidated = false;

            yield return AnimateOpening();
            cardsCanvas.gameObject.SetActive(true);

            yield return DrawCards();

            yield return DoStuffPreValidation();
            yield return new WaitUntil(() => isSelectionValidated);
            yield return DoStuffPostValidation();
            
            yield return AnimateClosing();

            DoStuffPostClosing();
            
            CharacterSingleton.instance.UnlockPlayer();
            MinimapBuilder.instance.SetMinimapState(true);
            isScreenOpen = false;
        }

        protected virtual IEnumerator DoStuffPreValidation()
        {
            yield break;
        }
        
        protected virtual IEnumerator DoStuffPostValidation()
        {
            yield break;
        }
        
        protected virtual void DoStuffPostClosing()
        {
            
        }

        private IEnumerator AnimateOpening()
        {
            backgroundCanvas.worldCamera = Camera.main;
            backgroundCanvas.gameObject.SetActive(true);
            
            background.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0.0f);

            blackScreen.MakeTransparent();

            Sequence sequence = Sequence.Create()
                .Group(Tween.UISizeDelta(background.rectTransform, backgroundSize, 0.3f))
                .Group(Tween.Alpha(blackScreen, 0.6f, 0.3f));

            yield return new WaitWhile(() => sequence.isAlive);
        }
        
        private IEnumerator DrawCards()
        {
            foreach (DeckCard card in PlayerDeck.instance.deck)
            {
                RunLoop.instance.DrawCardToContainer(card, handContainer);
                yield return new WaitForSeconds(0.05f);
            }
        }
        
        private IEnumerator AnimateClosing()
        {
            cardsCanvas.gameObject.SetActive(false);
            Sequence closingSequence = Sequence.Create()
                .Group(Tween.UISizeDelta(background.rectTransform, Vector2.zero, 0.3f))
                .Group(Tween.Alpha(blackScreen, 0.0f, 0.3f));
            
            yield return new WaitWhile(() => closingSequence.isAlive);
            
            backgroundCanvas.gameObject.SetActive(false);
        }

        public void ValidateSelection()
        {
            isSelectionValidated = true;
        }

        protected void ClearHand()
        {
            for (int i = handContainer.Slots.Count - 1; i >= 0; i--)
            {
                handContainer.Slots[i].CurrentCard.cardController.KillCard(false);
            }
        }
    }
}