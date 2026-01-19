using System.Collections.Generic;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Battles.Data
{
    [CreateAssetMenu(fileName = "BattleData", menuName = "Scriptable Objects/BattleData")]
    public class BattleData : ScriptableObject
    {
        public enum Floor
        {
            First,
            Second,
            Third
        }
        
        public enum Difficulty
        {
            Easy,
            Hard,
            Elite,
            Boss
        }

        public List<CardData> enemyList;

        [Space]
        public Floor floor;
        public Difficulty difficulty;
    }
}
