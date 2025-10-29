using System.Collections;
using Board.Script;
using UnityEngine;

namespace CombatLoop
{
    public class CombatLoop : MonoBehaviour
    {
        [SerializeField] private EnemyHandController enemyHandController;
        [SerializeField] private PlayerHandController playerHandController;
        [SerializeField] private CardContainer playerBoard;
        [SerializeField] private Animator canvasAnimator;
        
        [SerializeField] private GameObject endPreparationButton;
        [SerializeField] private GameObject endPlayerTurnButton;
        
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
            endPreparationButton.SetActive(false);
            endPlayerTurnButton.SetActive(false);
            
            currentTurn = TurnType.Preparation;
            yield return FightIntro();
            yield return PlaceEnemyCards();
            yield return DrawCards();
            yield return WaitForAtLeastOneCardOnPlayerBoard();
            yield return ActivateEndPreparationButton();
            yield return PlayHand();
            yield return DeactivateEndPreparationButton();
            yield return TransformBattleground();
            yield return ActivatePlayerEnergyDisplay();
            
            currentTurn = TurnType.Player;
            while (IsMatchOver() == false)
            {
                yield return ActivatePlayerEndTurnButton();
                yield return DoPlayerTurn();
                yield return DeactivatePlayerEndTurnButton();
                yield return PlayChangeTurnAnimation();
                yield return DoEnemyTurn();
                yield return PlayChangeTurnAnimation();
                yield return RefreshPlayerEnergyCount();
            }

            if (HasPlayerWon())
                yield return ShowVictoryScreen();
            else
                yield return ShowDefeatScreen();
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
        
        private IEnumerator WaitForAtLeastOneCardOnPlayerBoard()
        {
            yield return new WaitUntil(() => playerBoard.Slots.Count > 0);
        }
        
        private IEnumerator ActivateEndPreparationButton()
        {
            endPreparationButton.SetActive(true);
            yield break;
        }
        
        private IEnumerator DeactivateEndPreparationButton()
        {
            endPreparationButton.SetActive(false);
            yield break;
        }
        
        private IEnumerator ActivatePlayerEndTurnButton()
        {
            endPlayerTurnButton.SetActive(true);
            yield break;
        }
        
        private IEnumerator DeactivatePlayerEndTurnButton()
        {
            endPlayerTurnButton.SetActive(false);
            yield break;
        }
        
        private IEnumerator PlayHand()
        {
            yield return new WaitWhile(() => currentTurn == TurnType.Preparation);
        }

        public void OnEndSetupPhase()
        {
            currentTurn = TurnType.SetupOver;
        }
        
        private IEnumerator TransformBattleground()
        {
            canvasAnimator.Play("GoToBattle");
            yield return new WaitForSeconds(0.5f);
        }
        
        private IEnumerator ActivatePlayerEnergyDisplay()
        {
            EnergyController.instance.Initialize();
            yield return null;
        }

        private IEnumerator RefreshPlayerEnergyCount()
        {
            EnergyController.instance.RefreshOnTurnStart();
            yield return null;
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
            yield return enemyHandController.PlayTurn();
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
            return playerBoard.Slots.Count == 0 || enemyHandController.container.Slots.Count == 0;
        }

        private bool HasPlayerWon()
        {
            return playerBoard.Slots.Count > enemyHandController.container.Slots.Count;
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
