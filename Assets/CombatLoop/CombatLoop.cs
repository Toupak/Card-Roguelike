using System.Collections;
using UnityEngine;

namespace CombatLoop
{
    public class CombatLoop : MonoBehaviour
    {
        [SerializeField] private PlayerTurnController playerTurnController;
        [SerializeField] private EnemyHandController enemyHandController;
        [SerializeField] private PlayerHandController playerHandController;

        public enum TurnType
        {
            Preparation,
            Player,
            Enemy
        }

        private TurnType currentTurn;
        public TurnType CurrentTurn => currentTurn;
        
        private IEnumerator Start()
        {
            currentTurn = TurnType.Preparation;
            yield return FightIntro();
            yield return PlaceEnemyCards(); // Enemy cards appear
            yield return DrawCards(); // draw 10 cards
            yield return PlayHand(); // place a maximum of 5 cards in available slots

            currentTurn = TurnType.Player;
            while (IsMatchOver() == false)
            {
                yield return DoPlayerTurn();
                yield return PlayChangeTurnAnimation();
                yield return DoEnemyTurn();
                yield return PlayChangeTurnAnimation();
            }

            if (HasPlayerWon())
                yield return ShowVictoryScreen();
            else
                yield return ShowDefeatScreen();
        }

        private IEnumerator FightIntro()
        {
            yield break;
        }
        
        private IEnumerator PlaceEnemyCards()
        {
            yield break;
        }
        
        private IEnumerator DrawCards()
        {
            yield return playerHandController.DrawHand();
        }
        
        private IEnumerator PlayHand()
        {
            yield break;
        }

        private IEnumerator PlayChangeTurnAnimation()
        {
            if (currentTurn == TurnType.Player)
                currentTurn = TurnType.Enemy;
            else if (currentTurn == TurnType.Enemy)
                currentTurn = TurnType.Player;
            
            yield break;
        }

        private IEnumerator DoEnemyTurn()
        {
            enemyHandController.StartPlayTurn();
            yield return new WaitUntil(() => enemyHandController.IsOver);
        }

        private IEnumerator DoPlayerTurn()
        {
            playerTurnController.StartPlayTurn();
            yield return new WaitUntil(() => playerTurnController.IsOver);
        }
        
        private bool IsMatchOver()
        {
            return false;
        }

        private bool HasPlayerWon()
        {
            return true;
        }
        
        private IEnumerator ShowVictoryScreen()
        {
            Debug.Log("You Win !");
            yield break;
        }

        private IEnumerator ShowDefeatScreen()
        {
            Debug.Log("You Lose !");
            yield break;
        }
    }
}
