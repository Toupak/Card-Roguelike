using System.Collections.Generic;
using Cards.Scripts;
using UnityEngine;

namespace Character_Selection
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
    public class CharacterData : ScriptableObject
    {
        public string characterName;
        public string description;
        public Sprite displaySprite;
        public GameObject characterPrefab;
        public List<CardData> startingCards;
    }
}
