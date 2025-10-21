using System.Collections;
using System.Collections.Generic;
using BoomLib.BoomTween;
using UnityEngine;

namespace CombatLoop
{
    public class PlayerHandController : MonoBehaviour
    {
        [SerializeField] private Transform pivot;

        [Space] 
        [SerializeField] private float distanceBetweenCards;
        [SerializeField] private float drawDuration;
        
        private List<CardController> cards;

        public IEnumerator DrawHand()
        {
            cards = Deck.instance.DrawCards();
            
            for (int i = 0; i < cards.Count; i++)
            {
                yield return PlaceCardInHand(cards[i], i);
            }
        }

        private IEnumerator PlaceCardInHand(CardController card, int cardOffset)
        {
            Vector3 targetPosition = pivot.position + (Vector3.right * (distanceBetweenCards * cardOffset));

            yield return BTween.TweenPosition(card.transform, targetPosition, drawDuration);
        }
    }
}
