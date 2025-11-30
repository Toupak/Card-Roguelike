using System.Collections.Generic;
using Battles.Data;
using UnityEngine;

namespace CombatLoop.Battles
{
    [CreateAssetMenu(fileName = "BattlesDataHolder", menuName = "Scriptable Objects/BattlesHolder")]
    public class BattlesDataHolder : ScriptableObject
    {
        public List<BattleData> battles;
    }
}
