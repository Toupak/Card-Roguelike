using System.Collections;
using UnityEngine;

namespace CombatLoop
{
    public class CombatLoop : MonoBehaviour
    {
        [SerializeField] private EnemyHandController enemyHandController;
        [SerializeField] private PlayerHandController playerHandController;
        [SerializeField] private Animator canvasAnimator;
        
        public enum TurnType
        {
            Preparation,
            SetupOver,
            Player,
            Enemy
        }

        private TurnType currentTurn;
        public TurnType CurrentTurn => currentTurn;

        public static CombatLoop instance;

        private void Awake()
        {
            instance = this;
        }

        private IEnumerator Start()
        {
            currentTurn = TurnType.Preparation;
            yield return FightIntro();
            yield return PlaceEnemyCards(); // Enemy cards appear
            yield return DrawCards();
            yield return PlayHand(); // place a maximum of 5 cards in available slots

            yield return TransformBattleground();
            
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

        private IEnumerator TransformBattleground()
        {
            canvasAnimator.Play("GoToBattle");
            yield return new WaitForSeconds(0.5f);
        }

        private IEnumerator FightIntro()
        {
            yield return new WaitForSeconds(1.0f);
        }
        
        private IEnumerator PlaceEnemyCards()
        {
            enemyHandController.DrawCard();
            yield return new WaitForSeconds(0.1f);
            enemyHandController.DrawCard();
            yield return new WaitForSeconds(0.1f);
            enemyHandController.DrawCard();
            yield return new WaitForSeconds(0.1f);
        }
        
        private IEnumerator DrawCards()
        {
            yield return playerHandController.DrawHand();
        }
        
        private IEnumerator PlayHand()
        {
            yield return new WaitWhile(() => currentTurn == TurnType.Preparation);
        }

        public void OnEndSetupPhase()
        {
            currentTurn = TurnType.SetupOver;
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

        private bool isPlayerPlaying;
        private IEnumerator DoPlayerTurn()
        {
            isPlayerPlaying = true;
            yield return new WaitWhile(() => isPlayerPlaying);
        }
        
        public void OnEndPlayerTurn()
        {
            isPlayerPlaying = false;
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
