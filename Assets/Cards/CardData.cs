using UnityEngine;

namespace Cards
{
    [CreateAssetMenu(fileName = "Card", menuName = "ScriptableObject/Card")]
    public class CardData : ScriptableObject
    {
        public string cardName;
        public int cardNumber;
    }
}
