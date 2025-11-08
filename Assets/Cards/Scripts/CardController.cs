using EnemyAttack;
using JetBrains.Annotations;
using Passives;
using Run_Loop;
using Spells;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cards.Scripts
{
    public class CardController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI cardName;
        [SerializeField] private Image artwork;
        [SerializeField] public SpellButton singleButton;
        [SerializeField] public SpellButton leftButton;
        [SerializeField] public SpellButton rightButton;
        [SerializeField] public PassiveHolder passiveHolder;
        [SerializeField] public Image EnemyIntentionBackground;
        [SerializeField] public Image EnemyIntentionIcon;

        public RectTransform rectTransform { get; private set; }
        public Vector2 screenPosition => rectTransform.anchoredPosition;
        
        private FollowTarget followTarget;
        public CardMovement cardMovement { get;  private set; }
        public CardData cardData { get;  private set; }
        public DeckCard deckCard { get;  private set; }

        public CardHealth cardHealth { get; private set; }
        public CardStatus cardStatus { get; private set; }
        public DisplayCardEffects displayCardEffect { get; private set; }
        [CanBeNull] public EnemyCardController enemyCardController { get; private set; } // is Null for Player cards

        public void Setup(CardMovement movement, DeckCard cardFromDeck)
        {
            rectTransform = GetComponent<RectTransform>();
            followTarget = GetComponent<FollowTarget>();
            cardStatus = GetComponent<CardStatus>();
            followTarget.SetTarget(movement);
            
            cardHealth = GetComponent<CardHealth>();
            cardHealth.Setup(cardFromDeck);
            cardHealth.OnDeath.AddListener(KillCard);
            
            displayCardEffect = GetComponent<DisplayCardEffects>();
            
            cardMovement = movement;
            cardData = cardFromDeck.cardData;
            deckCard = cardFromDeck;

            cardName.text = cardData.cardName;
            
            if (artwork != null)
                SetArtwork(cardData.artwork);
            gameObject.name = cardData.cardName;
            
            SetupEnemyIntention(cardMovement.IsEnemyCard);
            SetupSpells(cardMovement.IsEnemyCard);
            SetupPassives();
        }

        private void SetupEnemyIntention(bool isEnemyCard)
        {
            if (isEnemyCard)
            {
                enemyCardController = gameObject.AddComponent<EnemyCardController>();
                enemyCardController!.Setup(this, cardData);
            }
        }

        private void SetupSpells(bool isEnemyCard)
        {
            bool isSingleSpell = !isEnemyCard && cardData.spellList.Count == 1;
            bool isDualSpell = !isEnemyCard && cardData.spellList.Count == 2;

            if (isSingleSpell)
                singleButton.Setup(this, cardData.spellList[0]);
            else if (isDualSpell)
            {
                leftButton.Setup(this, cardData.spellList[0]);
                rightButton.Setup(this, cardData.spellList[1]);
            }
            
            singleButton.gameObject.SetActive(isSingleSpell);
            leftButton.gameObject.SetActive(isDualSpell);
            rightButton.gameObject.SetActive(isDualSpell);
        }
        
        private void SetupPassives()
        {
            if (cardData.passiveList.Count > 0)
                passiveHolder.Setup(this, cardData);
        }

        public void SetArtwork(Sprite newSprite)
        {
            artwork.sprite = newSprite;
        }

        public void KillCard()
        {
            if (!cardMovement.IsEnemyCard && !cardData.isEnemy)
                PlayerDeck.instance.RemoveCardFromDeck(deckCard);
            
            cardMovement.CurrentSlot.board.DeleteCurrentSlot(cardMovement.SlotIndex);
            Destroy(gameObject);
        }

        public void SetFollowState(bool state)
        {
            followTarget.SetFollowState(state);
        }

        public void SetSpriteAsAbove()
        {
            followTarget.SetSortingOrderAsAbove();
        }

        public void ResetSpriteOrder()
        {
            followTarget.ResetSortingOrder();
        }
    }
}
