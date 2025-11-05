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
        [CanBeNull] public SpellData leftSpell;
        [CanBeNull] public SpellData rightSpell;
        public Sprite artwork;
        public string description;
        public CardController alternativeCardPrefab;
        [CanBeNull] public List<BaseEnemyBehaviour> enemyBehaviours;
    }
}
