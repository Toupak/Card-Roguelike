using Cards.Scripts;
using Status;
using System.Collections;
using System.Collections.Generic;
using Spells;
using Spells.Targeting;
using UnityEngine;

namespace EnemyAttack
{
    public abstract class BaseEnemyBehaviour : MonoBehaviour, IEnemyBehaviour
    {
        public string behaviourName;
        [TextArea] public string description;
        public int weight;
        public Sprite intentionIcon;

        protected EnemyCardController enemyCardController;

        public virtual void Setup(EnemyCardController controller)
        {
            enemyCardController = controller;
        }

        protected virtual int ComputeCurrentDamage(int spellDamage)
        {
            int bonus = 0;

            if (StatusSystem.instance.IsCardAfflictedByStatus(enemyCardController.cardController, StatusType.BonusDamage))
                bonus += enemyCardController.cardController.cardStatus.currentStacks[StatusType.BonusDamage];

            if (StatusSystem.instance.IsCardAfflictedByStatus(enemyCardController.cardController, StatusType.PermanentBonusDamage))
                bonus += enemyCardController.cardController.cardStatus.currentStacks[StatusType.PermanentBonusDamage];

            return spellDamage + bonus;
        }

        protected virtual CardController ComputeTarget(bool canBeTaunted = false)
        {
            List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Ally);

            if (canBeTaunted)
            {
                foreach (CardMovement cardMovement in targets)
                {
                    if (StatusSystem.instance.IsCardAfflictedByStatus(cardMovement.cardController, StatusType.Taunt))
                        return cardMovement.cardController;
                }
            }
            
            int randomTarget = Random.Range(0, targets.Count);

            return targets[randomTarget].cardController;
        }

        public virtual bool CanBeSelected()
        {
            return true;
        }

        public abstract IEnumerator ExecuteBehavior();
    }
}
