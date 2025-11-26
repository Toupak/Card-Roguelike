using Cards.Scripts;
using Status;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CombatLoop;
using Spells;
using Spells.Targeting;
using UnityEngine;

namespace EnemyAttack
{
    public abstract class BaseEnemyBehaviour : MonoBehaviour, IEnemyBehaviour
    {
        public string behaviourName;
        [TextArea] public string description;
        [SerializeField] protected int weight;
        public Sprite intentionIcon;
        public bool isWaiting;

        public string localizationKey;

        protected EnemyCardController enemyCardController;

        public virtual void Setup(EnemyCardController controller)
        {
            enemyCardController = controller;
        }

        protected virtual int ComputeCurrentDamage(int spellDamage, CardController target)
        {
            int damage = enemyCardController.cardController.ComputeCurrentDamage(spellDamage);

            if (target != null && target.cardStatus.IsStatusApplied(StatusType.BerserkMode))
                damage *= 2;

            return damage;
        }

        protected virtual CardController ComputeTarget()
        {
            List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Ally);
            
            if (targets.Count < 1)
                return null;

            foreach (CardMovement cardMovement in targets)
            {
                if (StatusSystem.instance.IsCardAfflictedByStatus(cardMovement.cardController, StatusType.Taunt))
                    return cardMovement.cardController;
            }

            int randomTarget = Random.Range(0, targets.Count);

            return targets[randomTarget].cardController;
        }

        public virtual int ComputeWeight()
        {
            return weight;
        }

        public virtual bool CanBeSelected()
        {
            return true;
        }

        public virtual CardController GetSpecificCard(CardData cardData)
        {
            List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(TargetType.Enemy);

            foreach (CardMovement cardMovement in targets)
            {
                if (cardMovement.cardController != null && cardMovement.cardController.cardData.cardName == cardData.cardName)
                    return cardMovement.cardController;
            }

            return null;
        }

        protected virtual List<CardMovement> ComputeTargetList(bool isTargetEnemy, bool canSelfTarget)
        {
            List<CardMovement> targets = TargetingSystem.instance.RetrieveBoard(isTargetEnemy ? TargetType.Enemy : TargetType.Ally);

            if (!canSelfTarget)
                return targets.Where((c) => c.cardController != enemyCardController.cardController).ToList();

            return targets;
        }

        public virtual string GetDamageText()
        {
            return "";
        }

        public virtual DamageSystem.DamageType GetDamageType()
        {
            return DamageSystem.DamageType.Physical;
        }

        public abstract IEnumerator ExecuteBehavior();
    }
}
