using Spells.Data;
using UnityEngine;

namespace Data
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
        public SpellData rightSpell;
        public SpellData leftSpell;
        public Sprite artwork;
        public string description;
    }
}
