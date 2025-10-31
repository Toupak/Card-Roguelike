using System.Collections;
using System.Collections.Generic;
using Cards.Scripts;
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

        private int totalWeight;

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

                totalWeight += instantiatedBehaviour.weight;
            }
        }
        
        private void SetupDisplay()
        {
            cardController.rightButton.gameObject.SetActive(false);   
            cardController.leftButton.gameObject.SetActive(false);   
            cardController.EnemyIntentionBackground.gameObject.SetActive(true);   
            cardController.EnemyIntentionIcon.gameObject.SetActive(true);   
        }

        public IEnumerator ExecuteIntention()
        {
            if (!hasIntention)
                ComputeNextIntention();
            
            yield return behaviourQueue.Dequeue().Execute();
        }

        public void ComputeNextIntention()
        {
            if (hasIntention)
                return;

            int random = Random.Range(0, totalWeight);
            foreach (BaseEnemyBehaviour enemyBehaviour in behaviours)
            {
                if (random < enemyBehaviour.weight)
                {
                    behaviourQueue.Enqueue(enemyBehaviour);
                    break;
                }

                random -= enemyBehaviour.weight;
            }
        }

        public void DisplayNextIntention()
        {
            cardController.EnemyIntentionIcon.sprite = behaviourQueue.Peek().intentionIcon;
        }
    }
}
