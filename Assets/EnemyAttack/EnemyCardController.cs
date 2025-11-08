using System.Collections;
using System.Collections.Generic;
using Cards.Scripts;
using Tooltip;
using UnityEngine;

namespace EnemyAttack
{
    public class EnemyCardController : MonoBehaviour
    {
        public CardController cardController { get; private set; }
        public CardData cardData { get; private set; }

        private List<BaseEnemyBehaviour> behaviours = new List<BaseEnemyBehaviour>();
        private Queue<BaseEnemyBehaviour> behaviourQueue = new Queue<BaseEnemyBehaviour>();

        public bool hasIntention => behaviourQueue.Count > 0;

        private DisplayTooltipOnHover displayTooltipOnHover;
        public bool isWaiting => hasIntention && behaviourQueue.Peek().isWaiting;

        public void Setup(CardController controller, CardData data)
        {
            cardController = controller;
            cardData = data;

            SetupBehaviours();
            SetupDisplay();
            ComputeNextIntention();
            DisplayNextIntention();
        }

        private void SetupBehaviours()
        {
            foreach (BaseEnemyBehaviour enemyBehaviour in cardData.enemyBehaviours)
            {
                Debug.Log($"Enemy Behaviour : {enemyBehaviour.behaviourName}");
                BaseEnemyBehaviour instantiatedBehaviour = Instantiate(enemyBehaviour, transform);
                instantiatedBehaviour.Setup(this);
                behaviours.Add(instantiatedBehaviour);
            }
        }
        
        private void SetupDisplay()
        {
            cardController.rightButton.gameObject.SetActive(false);   
            cardController.leftButton.gameObject.SetActive(false);   
            cardController.EnemyIntentionIcon.gameObject.SetActive(true);   
            cardController.EnemyIntentionBackground.gameObject.SetActive(true);
            displayTooltipOnHover = cardController.EnemyIntentionBackground.GetComponent<DisplayTooltipOnHover>();
        }

        public IEnumerator ExecuteIntention()
        {
            if (!hasIntention)
                ComputeNextIntention();
            
            yield return behaviourQueue.Dequeue().ExecuteBehavior();
        }

        public void ComputeNextIntention()
        {
            if (hasIntention)
                return;

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

        public void DisplayNextIntention()
        {
            BaseEnemyBehaviour behaviour = behaviourQueue.Peek();
            cardController.EnemyIntentionIcon.sprite = behaviour.intentionIcon;
            displayTooltipOnHover.SetTextToDisplay(behaviour.behaviourName, behaviour.description, TooltipDisplay.TooltipType.Spell);
        }
    }
}
