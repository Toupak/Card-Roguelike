using System;
using BoomLib.Tools;
using Cards.Scripts;
using Data;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Tables;

namespace Localization
{
    public class LocalizationSystem : MonoBehaviour
    {
        private const string databasePath = "Assets/Data/CardDatabase.asset";
        private const string tablePath = "Assets/Localization/Tables/SpellsAndPassives_en.asset";

        public static LocalizationSystem instance;

        private CardDatabase db;
        private StringTable table;
        
        private void Awake()
        {
            instance = this;
            
            db = AssetDatabase.LoadAssetAtPath<CardDatabase>(databasePath);
            table = AssetDatabase.LoadAssetAtPath<StringTable>(tablePath);
        }

        public string GetSpellDescription(string localizationKey, int spellIndex)
        {
            StringTableEntry entry = table.GetEntry($"{localizationKey}_spell_{spellIndex}");
            
            return entry != null ? entry.Value : "";
        }
        
        public string GetPassiveDescription(string localizationKey, int passiveIndex)
        {
            StringTableEntry entry = table.GetEntry($"{localizationKey}_passive_{passiveIndex}");
            
            return entry != null ? entry.Value : "";
        }

        [MenuItem("Tools/Fill Localization File")]
        private static void Test()
        {
            CardDatabase db = AssetDatabase.LoadAssetAtPath<CardDatabase>(databasePath);
            StringTable table = AssetDatabase.LoadAssetAtPath<StringTable>(tablePath);
            
            Debug.Log($"Table : {table.TableCollectionName}");
            
            foreach (CardData cardData in db.AllCards)
            {
                if (string.IsNullOrEmpty(cardData.localizationKey) && IsCardValidForLocalization(cardData))
                    AddNewEntryInTable(table, cardData);
            }
        }

        private static bool IsCardValidForLocalization(CardData cardData)
        {
            return !cardData.isEnemy && !cardData.isSpecialSummon;
        }

        private static void AddNewEntryInTable(StringTable table, CardData cardData)
        {
            string localizationKey = ComputeLocalizationKey(cardData.cardName);

            for (int i = 0; i < cardData.spellList.Count; i++)
                AddKeyToTable(table, $"{localizationKey}_spell_{i}", cardData.spellList[i].description);
            
            for (int i = 0; i < cardData.passiveList.Count; i++)
                AddKeyToTable(table, $"{localizationKey}_passive_{i}", cardData.passiveList[i].description);

            cardData.localizationKey = localizationKey;
        }

        private static void AddKeyToTable(StringTable table, string key, string value)
        {
            //Debug.Log($"TEST : {key} / {value}");
            table.AddEntry(key, value);
        }

        private static string ComputeLocalizationKey(string cardDataCardName)
        {
            return cardDataCardName.ToLower().RemoveWhitespace();
        }
    }
}
