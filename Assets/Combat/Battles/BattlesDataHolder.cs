using System.Collections.Generic;
using System.Linq;
using CombatLoop.Battles.Data;
using UnityEngine;

namespace CombatLoop.Battles
{
    [CreateAssetMenu(fileName = "BattlesDataHolder", menuName = "Scriptable Objects/BattlesHolder")]
    public class BattlesDataHolder : ScriptableObject
    {
        public List<BattleData> battles;

        public BattleData ChooseRandomBattle(BattleData.Floor floor, BattleData.Difficulty difficulty)
        {
            List<BattleData> filteredList = battles.Where((b) => b.floor == floor && b.difficulty == difficulty).ToList();

            if (filteredList.Count == 0)
            {
                Debug.LogError($"Could not find Battle of type : floor : {floor}, difficulty : {difficulty}");
                return battles[0];
            }
            
            return filteredList[Random.Range(0, filteredList.Count)];
        }
    }
}
