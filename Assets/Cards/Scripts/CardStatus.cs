using ActionReaction;
using ActionReaction.Game_Actions;
using UnityEngine;
using UnityEngine.UI;
using static CombatLoop.CombatLoop;

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
            ActionSystem.SubscribeReaction<EndTurnGA>(EndTurnReaction, ReactionTiming.PRE);
        }
        
        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<EndTurnGA>(EndTurnReaction, ReactionTiming.PRE);
        }

        private void EndTurnReaction(EndTurnGA endTurnGa)
        {
            if (IsCorrectTurn(endTurnGa.ending))
                RemoveOneStackOfEachStatus();
        }

        private bool IsCorrectTurn(TurnType startingTurn)
        {
            bool isPlayer = !cardController.cardMovement.IsEnemyCard;

            if (startingTurn == TurnType.Player && isPlayer)
                return true;

            if (startingTurn == TurnType.Enemy && !isPlayer)
                return true;

            return false;
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
