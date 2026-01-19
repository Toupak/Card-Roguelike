using System.Collections.Generic;
using Cards.Scripts;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Character_Selection.Character
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
    public class OverWorldCharacterData : ScriptableObject
    {
        public string characterName;
        public string description;
        public Sprite displaySprite;
        public SpriteLibraryAsset spriteLibrary;
        public List<CardData> startingCards;
    }
}
