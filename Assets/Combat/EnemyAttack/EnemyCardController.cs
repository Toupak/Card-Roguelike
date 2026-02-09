using System;
using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Combat.EnemyAttack
{
    public class EnemyCardController : MonoBehaviour
    {
        public CardController cardController { get; private set; }
        public CardData cardData { get; private set; }

        private List<BaseEnemyBehaviour> behaviours = new List<BaseEnemyBehaviour>();
        private Queue<BaseEnemyBehaviour> behaviourQueue = new Queue<BaseEnemyBehaviour>();

        public BaseEnemyBehaviour nextbehaviour => hasIntention ? behaviourQueue.Peek() : null;
        
        public bool hasIntention => behaviourQueue.Count > 0;

        public bool isWaiting => hasIntention && behaviourQueue.Peek().isWaiting;

        public void Setup(CardController controller, CardData data)
        {
            cardController = controller;
            cardData = data;
            
            cardController.cardStatus.OnUpdateStatus.AddListener((_,_) => UpdateDamageText());

            SetupBehaviours();
            SetupDisplay();
        }

        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<StartTurnGa>(PlayerStartTurnReaction, ReactionTiming.PRE);
        }
        
        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<StartTurnGa>(PlayerStartTurnReaction, ReactionTiming.PRE);
        }

        private void PlayerStartTurnReaction(StartTurnGa startTurnGa)
        {
            if (startTurnGa.starting == CombatLoop.TurnType.Player)
            {
                SetupIntentionDisplay();
                ComputeNextIntention();
                DisplayNextIntention();
            }
        }

        private void SetupBehaviours()
        {
            foreach (BaseEnemyBehaviour enemyBehaviour in cardData.enemyBehaviours)
            {
                BaseEnemyBehaviour instantiatedBehaviour = Instantiate(enemyBehaviour, transform);
                instantiatedBehaviour.Setup(this);
                behaviours.Add(instantiatedBehaviour);
            }

            if (CombatLoop.instance.turnCount > 0 && cardData.isWaitingOnSpawn)
                behaviourQueue.Enqueue(cardController.waitingBehaviourPrefab);
        }
        
        private void SetupDisplay()
        {
            cardController.rightButton.gameObject.SetActive(false);   
            cardController.leftButton.gameObject.SetActive(false);   
        }

        public IEnumerator ExecuteIntention()
        {
            if (!hasIntention)
                ComputeNextIntention();
            
            if (behaviourQueue.Count > 0)
                yield return behaviourQueue.Dequeue().ExecuteBehavior();
        }

        public void SkipIntention()
        {
            behaviourQueue.Dequeue();
        }

        public void ComputeNextIntention()
        {
            if (hasIntention)
                return;

            if (cardData.areEnemyBehavioursLooping)
            {
                for (int i = 0; i < behaviours.Count; i++)
                    behaviourQueue.Enqueue(behaviours[i]);
                return;
            }

            int total = 0;
            int[] weightArray = new int[behaviours.Count];
            for (int i = 0; i < behaviours.Count; i++)
            {
                weightArray[i] = behaviours[i].ComputeWeight();
                total += weightArray[i];
            }

            int random = Random.Range(0, total);
            for (int i = 0; i < behaviours.Count; i++)
            {
                if (random < weightArray[i])
                {
                    behaviourQueue.Enqueue(behaviours[i]);
                    break;
                }

                random -= weightArray[i];
            }
        }

        public void SetNewIntention(BaseEnemyBehaviour newBehaviour, bool resetQueue = false)
        {
            BaseEnemyBehaviour targetBehaviour = behaviours.Find((b) => b.behaviourName == newBehaviour.behaviourName);
            
            if (targetBehaviour == null)
                return;
            
            if (resetQueue)
                behaviourQueue.Clear();
            
            behaviourQueue.Enqueue(targetBehaviour);
        }
        
        private void SetupIntentionDisplay()
        {
            if (cardData.enemyBehaviours.Count <= 0)
                return;
            
            cardController.enemyIntentionIcon.gameObject.SetActive(true);   
            cardController.enemyIntentionBackground.gameObject.SetActive(true);   
            cardController.enemyIntentionText.gameObject.SetActive(true);
        }

        public void DisplayNextIntention()
        {
            if (behaviourQueue.Count <= 0)
                return;

            BaseEnemyBehaviour behaviour = behaviourQueue.Peek();
            cardController.enemyIntentionIcon.sprite = behaviour.intentionIcon;

            UpdateDamageText();
        }

        private void UpdateDamageText()
        {
            string behaviourDamage = hasIntention ? nextbehaviour.GetDamageText() : "";
            bool isDamageDisplayed = !String.IsNullOrEmpty(behaviourDamage);
            cardController.enemyIntentionText.text = isDamageDisplayed ? behaviourDamage : "";
            cardController.enemyIntentionText.gameObject.SetActive(isDamageDisplayed);
        }
    }
}
