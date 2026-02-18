using System.Collections.Generic;
using Combat.Card_Container.Script;
using Combat.EnemyAttack;
using Combat.Passives;
using Combat.Spells;
using UnityEngine;

namespace Cards.Scripts
{
    [CreateAssetMenu(fileName = "Card", menuName = "ScriptableObject/Card")]
    public class CardData : ScriptableObject
    {
        public enum Tribe
        {
            None,
            AquaForce,
            SealTeamX,
            Tank,
            DPS,
            Support
        }

        public enum Rarity
        {
            Common,
            Rare,
            Legendary,
            Exotic
        }

        public string cardName;
        public Tribe tribe;
        public Rarity rarity;
        public int hpMax;
        public bool isInvincible;
        public Sprite artwork;
        public string description;
        public bool isSpecialSummon;
        public CardController alternativeCardPrefab;
        
        [Space]
        public List<SpellData> spellList;

        [Space]
        public List<PassiveData> passiveList;

        [Space]
        [Header("Enemy Stuff")]
        public List<BaseEnemyBehaviour> enemyBehaviours;
        public bool areEnemyBehavioursLooping;
        public bool isMainBoss;
        public bool isWaitingOnSpawn;
        public CardContainer.PreferredPosition preferredPosition;
        public bool isEnemySummon;
        
        [Space]
        [Header("SFX")]
        public List<AudioClip> pickupAudioClip;
        public List<AudioClip> dropAudioClip;
        public List<AudioClip> inspectAudioClip;
        public List<AudioClip> stopInspectAudioClip;
        public List<AudioClip> spawnAudioClip;
        public List<AudioClip> takeDamageAudioClip;
        public List<AudioClip> deathAudioClip;

        [Space] 
        [Header("Localization")]
        public string localizationKey;
        
        public bool isIncomplete => (passiveList == null || passiveList.Count < 1) && (spellList == null || spellList.Count < 1) && !isEnemy;
        public bool isEnemy => (enemyBehaviours != null && enemyBehaviours.Count > 0) || isEnemySummon;
        public bool canBeDrawn => !isIncomplete && !isSpecialSummon && !isEnemy;
        public bool isLowRarity => rarity == Rarity.Common || rarity == Rarity.Rare;
    }
}
