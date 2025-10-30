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
            MedievalFantasyRobot
        }

        public int cardNumber;
        public string cardName;
        public Tribe tribe;
        public int hpMax;
        public SpellData leftSpell;
        public SpellData rightSpell;
        public Sprite artwork;
        public string description;
    }
}
