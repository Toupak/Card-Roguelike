using Spells.Data;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "Card", menuName = "ScriptableObject/Card")]
    public class CardData : ScriptableObject
    {
        public string cardName;
        public int cardNumber;
        public Sprite artwork;
        public SpellData leftSpell;
        public SpellData rightSpell;
    }
}
