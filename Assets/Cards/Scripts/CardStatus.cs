using ActionReaction;
using ActionReaction.Game_Actions;
using UnityEngine;
using UnityEngine.UI;

namespace Cards.Scripts
{
    public class CardStatus : MonoBehaviour
    {
        [SerializeField] private Image stunEffect;

        private CardController cardController;

        private int stunStacks;
        public bool IsStun => stunStacks > 0;
        
        private void Start()
        {
            cardController = GetComponent<CardController>();
            stunEffect.gameObject.SetActive(false);
        }
        
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.POST);
        }
        
        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.POST);
        }

        public void StartTurnReaction(StartTurnGa loadBarkBulletGa)
        {
            RemoveOneStackOfEachStatus();
        }

        private void RemoveOneStackOfEachStatus()
        {
            stunStacks = Mathf.Max(stunStacks - 1, 0);
            UpdateStunEffect();
        }

        public void ApplyStunStacks(int stacksCount)
        {
            stunStacks += stacksCount;
            UpdateStunEffect();
        }

        private void UpdateStunEffect()
        {
            stunEffect.gameObject.SetActive(IsStun);
        }
    }
}
