using System.Collections.Generic;
using Cards.Scripts;
using UnityEngine;

namespace Battles.Data
{
    [CreateAssetMenu(fileName = "BattleData", menuName = "Scriptable Objects/BattleData")]
    public class BattleData : ScriptableObject
    {
        public List<CardData> enemyList;
    }
}
