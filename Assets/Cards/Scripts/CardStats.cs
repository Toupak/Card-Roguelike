using System.Collections.Generic;
using UnityEngine;

namespace Cards.Scripts
{
    public class CardStats : MonoBehaviour
    {
        public enum Stats
        {
            Strength,
            Constitution
        }

        private Dictionary<Stats, int> statDictionary = new Dictionary<Stats, int>();

        public void IncreaseStat(Stats stat, int amount)
        {
            if (!statDictionary.ContainsKey(stat))
                statDictionary.Add(stat, amount);
            else
                statDictionary[stat] += amount;
        }

        public void DecreaseStat(Stats stat, int amount)
        {
            if (!statDictionary.ContainsKey(stat))
                statDictionary.Add(stat, -amount);
            else
                statDictionary[stat] -= amount;
        }

        public int GetStatValue(Stats stat)
        {
            if (statDictionary.ContainsKey(stat))
                return statDictionary[stat];

            return 0;
        }
    }
}
