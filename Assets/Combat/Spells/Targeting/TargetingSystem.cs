using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards.Scripts;
using Combat.Card_Container.CardSlot;
using Combat.Card_Container.Script;
using Cursor.Script;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;

namespace Combat.Spells.Targeting
{
    public class TargetingSystem : MonoBehaviour
    {
        [SerializeField] private CardContainer playerBoard;
        [SerializeField] private CardContainer enemyBoard;
        [SerializeField] private TargetingCursor targetingCursorPrefab;

        public static TargetingSystem instance;
        
        private TargetingCursor currentTargetingCursor;
        private List<TargetingCursor> previousCursors;

        private List<CardMovement> potentialTargets = new List<CardMovement>();
        private List<CardMovement> currentTargets = new List<CardMovement>();
        public List<CardMovement> Targets => currentTargets;

        private bool isCanceled;
        public bool IsCanceled => isCanceled;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            SpellController.OnCancelSpell.AddListener(StopTargeting);
        }

        public IEnumerator SelectTargets(CardMovement castingCard, Transform startingPosition, TargetType targetType, TargetingMode targetingMode, int targetCount = 1, bool targetToken = false)
        {
            Debug.Log("Start Targeting");

            isCanceled = false;
            currentTargets = new List<CardMovement>();
            previousCursors = new List<TargetingCursor>();

            if (targetCount > 1 && targetType != TargetType.Self && targetingMode == TargetingMode.Single)
                targetingMode = TargetingMode.Multi;
            
            potentialTargets = ComputeTargetAllList(castingCard, targetType, targetToken);

            if (targetingMode == TargetingMode.All)
            {
                currentTargets = potentialTargets;
                yield break;
            }
            
            StartTargeting();

            int maxTargetCount = ComputeMaxAmountOfTargets(potentialTargets.Count, targetingMode, targetCount);
            Debug.Log($"Target Count : {maxTargetCount}");

            HighlightTargets(potentialTargets);

            yield return null;//skip a frame to prevent targeting the card that just started the spell
            while (!isCanceled && currentTargets.Count < maxTargetCount)
            {
                if (currentTargetingCursor == null)
                    SpawnTargetingCursor(startingPosition);
                
                UpdateTargetingCursorPosition();
                if (Mouse.current.leftButton.wasPressedThisFrame)
                    CheckForTarget(targetType, targetToken);
                if (Mouse.current.rightButton.wasPressedThisFrame)
                    isCanceled = true;
                yield return null;
            }

            if (isCanceled)
                currentTargets = new List<CardMovement>();
            else
                yield return new WaitForSeconds(0.3f);
            
            StopTargeting();
        }

        private List<CardMovement> ComputeTargetAllList(CardMovement current, TargetType targetType, bool targetToken)
        {
            List<CardMovement> list = new List<CardMovement>();
            switch (targetType)
            {
                case TargetType.Ally:
                    list = RetrieveBoard(playerBoard, false, targetToken);
                    break;
                case TargetType.Enemy:
                    list = RetrieveBoard(enemyBoard, false, targetToken);
                    break;
                case TargetType.Self:
                    if (targetToken)
                        list.AddRange(RetrieveCardTokens(current));
                    else
                        list.Add(current);
                    break;
                case TargetType.None:
                    list = RetrieveBoard(playerBoard, false, targetToken);
                    list.AddRange(RetrieveBoard(enemyBoard, false, targetToken));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetType), targetType, null);
            }

            return list;
        }

        public CardMovement RetrieveCard(CardData cardData, TargetType targetType = TargetType.None)
        {
            switch (targetType)
            {
                case TargetType.Ally:
                    return RetrieveCard(cardData, RetrieveBoard(playerBoard));
                case TargetType.Enemy:
                    return RetrieveCard(cardData, RetrieveBoard(enemyBoard));
                case TargetType.None:
                    return RetrieveCard(cardData, RetrieveBoard(playerBoard).Concat(RetrieveBoard(enemyBoard)).ToList());
                case TargetType.Self:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetType), targetType, null);
            }
        }

        private CardMovement RetrieveCard(CardData cardData, List<CardMovement> cards)
        {
            foreach (CardMovement card in cards)
            {
                if (card.cardController.cardData.cardName == cardData.cardName)
                    return card;
            }

            return null;
        }

        private List<CardMovement> RetrieveCardTokens(CardMovement cardMovement)
        {
            List<CardMovement> tokens = new List<CardMovement>();
            foreach (Slot slot in cardMovement.tokenContainer.Slots)
            {
                tokens.Add(slot.CurrentCard);
            }

            return tokens;
        }
        
        public List<CardMovement> RetrieveBoard(TargetType targetType, bool includeUnTargetable = false)
        {
            switch (targetType)
            {
                case TargetType.Ally:
                    return RetrieveBoard(playerBoard, includeUnTargetable);
                case TargetType.Enemy:
                    return RetrieveBoard(enemyBoard, includeUnTargetable);
                case TargetType.None:
                    return RetrieveBoard(playerBoard, includeUnTargetable).Concat(RetrieveBoard(enemyBoard, includeUnTargetable)).ToList();
                case TargetType.Self:
                    return null;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetType), targetType, null);
            }
        }

        private List<CardMovement> RetrieveBoard(CardContainer container, bool includeUnTargetable = false, bool targetToken = false)
        {
            List<CardMovement> list = new List<CardMovement>();
            foreach (Slot slot in container.Slots)
            {
                if (!slot.IsEmpty)
                {
                    CardMovement cardMovement = slot.transform.GetChild(0).GetComponent<CardMovement>();

                    if (targetToken)
                    {
                        list.AddRange(RetrieveCardTokens(cardMovement));
                    }
                    else
                    {
                        if (cardMovement.cardController.IsTargetable() || includeUnTargetable)
                            list.Add(cardMovement);
                    }
                }
            }

            return list;
        }

        public CardMovement RetrieveRandomCard(TargetType targetType, bool includeUnTargetable = false)
        {
            List<CardMovement> cards = RetrieveBoard(targetType, includeUnTargetable);

            if (cards.Count < 1)
                return null;
            
            return cards[Random.Range(0, cards.Count)];
        }

        private int ComputeMaxAmountOfTargets(int potentialTargetsCount, TargetingMode targetingMode, int targetCount)
        {
            switch (targetingMode)
            {
                case TargetingMode.Single:
                    return 1;
                case TargetingMode.Multi:
                    return Math.Min(targetCount, potentialTargetsCount);
                case TargetingMode.All:
                    Debug.LogError("error : TargetingMode.All should have been handled by ComputeTargetAllList()");
                    return 0;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetingMode), targetingMode, null);
            }
        }

        private void CheckForTarget(TargetType targetType, bool targetToken)
        {
            Debug.Log($"Targeting : Check for targets : {CursorInfo.instance.currentCardMovement}");

            CardMovement card = CursorInfo.instance.currentCardMovement;
            
            if (card != null && !currentTargets.Contains(card) && IsSelectedTargetValid(card, targetType, targetToken))
                ValidateTarget(card);
        }

        private void ValidateTarget(CardMovement card)
        {
            card.cardController.displayCardEffect.SetTargetState(true);
            currentTargets.Add(card);
            previousCursors.Add(currentTargetingCursor);
            currentTargetingCursor = null;
        }

        private bool IsSelectedTargetValid(CardMovement card, TargetType targetType, bool targetToken)
        {
            switch (targetType)
            {
                case TargetType.Ally:
                    return card.IsEnemyCard == false && card.cardController.isToken == targetToken;
                case TargetType.Enemy:
                    return card.IsEnemyCard && card.cardController.isToken == targetToken;
                case TargetType.Self:
                    return false;
                case TargetType.None:
                    return card.cardController.isToken == targetToken;
                default:
                    throw new ArgumentOutOfRangeException(nameof(targetType), targetType, null);
            }
        }

        private void SpawnTargetingCursor(Transform startPosition)
        {
            currentTargetingCursor = Instantiate(targetingCursorPrefab, startPosition.position, Quaternion.identity, transform);
            currentTargetingCursor.Setup(startPosition);
        }

        private void UpdateTargetingCursorPosition()
        {
            currentTargetingCursor.UpdatePosition();
        }

        private void StartTargeting()
        {
            CursorInfo.instance.SetCursorMode(CursorInfo.CursorMode.Targeting);
        }
        
        private void StopTargeting()
        {
            DestroyAllTargetingCursor();
            StopHighlightTargets(potentialTargets);
            CursorInfo.instance.SetCursorMode(CursorInfo.CursorMode.Free);
        }

        private void DestroyAllTargetingCursor()
        {
            if (previousCursors.Count > 0)
                foreach (TargetingCursor cursor in previousCursors)
                    cursor.DestroyCursor();

            if (currentTargetingCursor != null)
            {
                currentTargetingCursor.DestroyCursor();
                currentTargetingCursor = null;
            }
        }
        
        private void HighlightTargets(List<CardMovement> targets)
        {
            foreach (CardMovement cardMovement in targets)
            {
                cardMovement.cardController.displayCardEffect.SetPotentialTargetState(true);
            }
        }

        private void StopHighlightTargets(List<CardMovement> targets)
        {
            foreach (CardMovement cardMovement in targets)
            {
                if (cardMovement != null)
                {
                    cardMovement.cardController.displayCardEffect.SetPotentialTargetState(false);
                    cardMovement.cardController.displayCardEffect.SetTargetState(false);
                }
            }
        }
    }
}
