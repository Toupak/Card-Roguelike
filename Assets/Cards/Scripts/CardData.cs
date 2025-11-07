using System.Collections.Generic;
using EnemyAttack;
using JetBrains.Annotations;
using Spells;
using UnityEngine;

namespace Cards.Scripts
{
    [CreateAssetMenu(fileName = "Card", menuName = "ScriptableObject/Card")]
    public class CardData : ScriptableObject
    {
        public enum Tribe
        {
            InanimateObjects,
            WeaponizedAnimals,
            Fungi,
            SentientGeometricalFigures,
            MedievalFantasyRobot,
            Food,
            Alien
        }

        public int cardNumber;
        public string cardName;
        public Tribe tribe;
        public int hpMax;
        public Sprite artwork;
        public string description;
        public CardController alternativeCardPrefab;
        
        [Space]
        [CanBeNull] public List<SpellData> spellList;

        [Space]
        [CanBeNull] public List<PassiveData> passiveList;
        
        [Space]
        [CanBeNull] public List<BaseEnemyBehaviour> enemyBehaviours;

        public bool isIncomplete => (passiveList == null || passiveList.Count < 1) && (spellList == null || spellList.Count < 1) && !isEnemy;
        public bool isEnemy => enemyBehaviours != null && enemyBehaviours.Count > 0;
    }
}
