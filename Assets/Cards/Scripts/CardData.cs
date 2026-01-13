using System.Collections.Generic;
using Board.Script;
using EnemyAttack;
using Frames;
using Passives;
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

        public enum Rarity
        {
            Common,
            Rare,
            Legendary,
            Exotic
        }

        public enum EnemyDifficulty
        {
            Easy,
            Hard
        }

        public enum EnemyFloor
        {
            _1,
            _2,
            _3
        }

        public int cardNumber;
        public string cardName;
        public Tribe tribe;
        public Rarity rarity;
        public int hpMax;
        public Sprite artwork;
        public string description;
        public bool isSpecialSummon;
        public CardController alternativeCardPrefab;
        public FrameData frameData;
        
        [Space]
        public List<SpellData> spellList;

        [Space]
        public List<PassiveData> passiveList;

        [Space]
        public List<BaseEnemyBehaviour> enemyBehaviours;
        public bool areEnemyBehavioursLooping;
        public bool isMainBoss;
        public bool isWaitingOnSpawn;
        public CardContainer.PreferredPosition preferredPosition;
        public EnemyDifficulty enemyDifficulty;
        public EnemyFloor enemyFloor;
        
        [Space] 
        public string localizationKey;
        
        public bool isIncomplete => (passiveList == null || passiveList.Count < 1) && (spellList == null || spellList.Count < 1) && !isEnemy;
        public bool isEnemy => enemyBehaviours != null && enemyBehaviours.Count > 0;
        public bool canBeDrawn => !isIncomplete && !isSpecialSummon && !isEnemy;
    }
}
