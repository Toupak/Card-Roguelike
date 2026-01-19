using Combat.EnemyAttack;
using Combat.Passives;
using Combat.Spells;
using Inventory.Items.Frames;
using JetBrains.Annotations;
using Run_Loop;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
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
        [SerializeField] public Image enemyIntentionIcon;
        [SerializeField] public Image enemyIntentionBackground;
        [SerializeField] public TextMeshProUGUI enemyIntentionText;
        [SerializeField] public BaseEnemyBehaviour waitingBehaviourPrefab;
        [SerializeField] public RectTransform regularTooltipPivot;
        [SerializeField] public RectTransform inspectionTooltipPivot;
        
        public RectTransform tooltipPivot => cardMovement.isInspected ? inspectionTooltipPivot : regularTooltipPivot;

        [HideInInspector] public UnityEvent OnKillCard = new UnityEvent();
        
        public RectTransform rectTransform { get; private set; }
        public Vector2 screenPosition => rectTransform.anchoredPosition;
        
        private FollowTarget followTarget;
        public CardMovement cardMovement { get;  private set; }
        public CardData cardData { get;  private set; }
        public DeckCard deckCard { get;  private set; }

        public CardHealth cardHealth { get; private set; }
        public CardStatus cardStatus { get; private set; }
        public DisplayCardEffects displayCardEffect { get; private set; }
        public FrameDisplay frameDisplay { get; private set; }
        public CardRarityDisplay cardRarityDisplay { get; private set; }
        [CanBeNull] public EnemyCardController enemyCardController { get; private set; } // is Null for Player cards
        public bool isEnemy => enemyCardController != null;

        public CardController tokenParentController { get; private set; } // is Null for regular cards, only  set for tokens
        public bool isToken => tokenParentController != null;
        
        public void SetupToken(CardMovement movement, CardData data, CardController parentCardController)
        {
            tokenParentController = parentCardController;
            SetupCard(movement, data, data.hpMax);
        }
        
        public void Setup(CardMovement movement, DeckCard cardFromDeck)
        {
            deckCard = cardFromDeck;

            SetupCard(movement, cardFromDeck.cardData, cardFromDeck.currentHealth);
        }

        private void SetupCard(CardMovement movement, CardData data, int health)
        {
            rectTransform = GetComponent<RectTransform>();
            followTarget = GetComponent<FollowTarget>();
            cardStatus = GetComponent<CardStatus>();
            frameDisplay = GetComponent<FrameDisplay>();
            cardRarityDisplay = GetComponent<CardRarityDisplay>();
            followTarget.SetTarget(movement);
            
            cardHealth = GetComponent<CardHealth>();
            cardHealth.Setup(health);
            cardHealth.OnDeath.AddListener(() => KillCard());
            
            displayCardEffect = GetComponent<DisplayCardEffects>();
            
            cardMovement = movement;
            cardData = data;

            gameObject.name = cardData.cardName;

            SetupCardBackground(cardData.rarity);
            SetArtwork(cardData.artwork);
            SetCardName(cardData.cardName);
            SetupEnemyIntention(cardMovement.IsEnemyCard);
            SetupSpells(cardMovement.IsEnemyCard);
            SetupPassives();
        }

        private void SetupCardBackground(CardData.Rarity cardRarity)
        {
            if (cardRarityDisplay != null)
                cardRarityDisplay.SetupBackground(cardRarity);
        }
        
        public void SetArtwork(Sprite newSprite)
        {
            if (artwork != null && artwork.sprite != null)
                artwork.sprite = newSprite;
        }

        public void SetCardName(string newName)
        {
            cardName.text = newName;
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
                SetupSingleButton(cardData.spellList[0]);
            else if (isDualSpell)
            {
                SetupDualButtons(cardData.spellList[0], cardData.spellList[1]);
            }
            else
            {
                singleButton.gameObject.SetActive(false);
                leftButton.gameObject.SetActive(false);
                rightButton.gameObject.SetActive(false);
            }
        }

        public void SetupSingleButton(SpellData data)
        {
            singleButton.Setup(this, data);
            
            singleButton.gameObject.SetActive(true);
            leftButton.gameObject.SetActive(false);
            rightButton.gameObject.SetActive(false);
        }
        
        public void SetupDualButtons(SpellData left, SpellData right)
        {
            leftButton.Setup(this, left);
            rightButton.Setup(this, right);
            
            singleButton.gameObject.SetActive(false);
            leftButton.gameObject.SetActive(true);
            rightButton.gameObject.SetActive(true);
        }

        public void SetupLeftSpell(SpellData left)
        {
            leftButton.Setup(this, left);
            
            singleButton.gameObject.SetActive(false);
            leftButton.gameObject.SetActive(true);
        }
        
        public void SetupRightSpell(SpellData right)
        {
            rightButton.Setup(this, right);
            
            singleButton.gameObject.SetActive(false);
            rightButton.gameObject.SetActive(true);
        }
        
        public void SetupSingleSpell(SpellData right)
        {
            singleButton.Setup(this, right);
            
            rightButton.gameObject.SetActive(false);
            leftButton.gameObject.SetActive(false);
            singleButton.gameObject.SetActive(true);
        }
        
        private void SetupPassives()
        {
            if (cardData.passiveList.Count > 0)
                passiveHolder.Setup(this, cardData);
        }
        
        public void AddFrame(FrameData frameData)
        {
            if (frameDisplay.hasFrame)
                frameDisplay.RemoveFrame();
            
            frameDisplay.SetupFrame(this, frameData);
        }

        public void KillCard(bool removeFromDeck = true)
        {
            if (removeFromDeck && !cardData.isEnemy)
                PlayerDeck.instance.RemoveCardFromDeck(deckCard);
            
            cardMovement.KillAllTokens();
            cardMovement.CurrentSlot.board.DeleteCurrentSlot(cardMovement.SlotIndex);
            OnKillCard?.Invoke();
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

        public int ComputeCurrentDamage(int damage)
        {
            int bonus = 0;

            bonus += cardStatus.GetCurrentStackCount(StatusType.BonusDamage);
            bonus += cardStatus.GetCurrentStackCount(StatusType.PermanentBonusDamage);
            bonus -= cardStatus.GetCurrentStackCount(StatusType.Weak);
            bonus -= cardStatus.IsStatusApplied(StatusType.Terror) ? 1 : 0;

            int total = damage + bonus;

            if (cardStatus.IsStatusApplied(StatusType.BerserkMode))
                total *= 2;
            
            return Mathf.Max(0, total);
        }
        
        public virtual int ComputeCurrentTargetCount(int count)
        {
            if (cardStatus.IsStatusApplied(StatusType.Fury))
                return count + cardStatus.currentStacks[StatusType.Fury];

            return count;
        }

        public bool IsTargetable()
        {
            return !cardStatus.IsStatusApplied(StatusType.Dive);
        }
    }
}
