using BoomLib.Tools;
using Cards.Scripts;
using Data;
using Localization.Icons_In_Text;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.TextCore;

namespace Localization
{
    public class LocalizationSystem : MonoBehaviour
    {
        private const string databasePath = "Assets/Data/CardDatabase.asset";
        private const string tablePath = "Assets/Localization/Tables/SpellsAndPassives_en.asset";

        public TMP_SpriteAsset spriteAsset;
        public TextToIconData textToIconData;

        public static LocalizationSystem instance;
        public TextToIcon textToIcon { get; private set; }

        private CardDatabase db;
        private StringTable table;
        
        private void Awake()
        {
            instance = this;
            
            textToIcon = GetComponent<TextToIcon>();
            db = AssetDatabase.LoadAssetAtPath<CardDatabase>(databasePath);
            table = AssetDatabase.LoadAssetAtPath<StringTable>(tablePath);
            
            //UpdateGlyphs();
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

        //[MenuItem("Tools/From Google Sheet to CardData")]
        private static void UpdateCardDataWithLocalization()
        {
            CardDatabase db = AssetDatabase.LoadAssetAtPath<CardDatabase>(databasePath);
            StringTable table = AssetDatabase.LoadAssetAtPath<StringTable>(tablePath);
            
            foreach (CardData cardData in db.AllCards)
            {
                if (IsCardValidForLocalization(cardData))
                    UpdateCardDescription(table, cardData);
            }
        }
        
        [MenuItem("Tools/Split Locals")]
        private static void SplitLocals()
        {
            CardDatabase db = AssetDatabase.LoadAssetAtPath<CardDatabase>(databasePath);
            StringTable spellAndPassive = AssetDatabase.LoadAssetAtPath<StringTable>(tablePath);
            StringTable spell = AssetDatabase.LoadAssetAtPath<StringTable>("Assets/Localization/Tables/Spells/Spells_en.asset");
            StringTable passive = AssetDatabase.LoadAssetAtPath<StringTable>("Assets/Localization/Tables/Passives/Passives_en.asset");
            
            foreach (CardData cardData in db.AllCards)
            {
                if (!cardData.isEnemy)
                {
                    string localizationKey = ComputeLocalizationKey(cardData.cardName);
                    Debug.Log($"[{nameof(LocalizationSystem)}] : Added new Entry : {localizationKey}");

                    for (int i = 0; i < cardData.spellList.Count; i++)
                    {
                        string oldKey = $"{localizationKey}_spell_{i}";
                        string spellKey = ComputeLocalizationKey(cardData.spellList[i].name);
                        string description = spellAndPassive.GetEntry(oldKey) == null ? cardData.spellList[i].description : spellAndPassive.GetEntry(oldKey).Value; 
                        cardData.spellList[i].localizationKey = spellKey;
                        AddKeyToTable(spell, $"{localizationKey}_{spellKey}", description);
                        Debug.Log($"[{nameof(LocalizationSystem)}] : Added new Spell : {spellKey} : {description}");
                    }

                    for (int i = 0; i < cardData.passiveList.Count; i++)
                    {
                        string oldKey = $"{localizationKey}_passive_{i}";
                        string passiveKey = ComputeLocalizationKey(cardData.passiveList[i].name);
                        string description = spellAndPassive.GetEntry(oldKey) == null ? cardData.passiveList[i].description : spellAndPassive.GetEntry(oldKey).Value; 
                        cardData.passiveList[i].localizationKey = passiveKey;
                        AddKeyToTable(passive, $"{localizationKey}_{passiveKey}", description);
                        Debug.Log($"[{nameof(LocalizationSystem)}] : Added new Passive : {passiveKey} : {description}");
                    }

                    cardData.localizationKey = localizationKey;
                }
            }
        }
        
        
        private void UpdateGlyphs()
        {
            Debug.Log($"{spriteAsset.name}");
            
            foreach (TMP_SpriteGlyph glyph in spriteAsset.spriteGlyphTable)
            {
                glyph.scale = 1.75f;
                glyph.metrics = new GlyphMetrics(32, 32, -5, 24, 24);
            }
        }

        private static void UpdateCardDescription(StringTable table, CardData cardData)
        {
            string localizationKey = ComputeLocalizationKey(cardData.cardName);

            for (int i = 0; i < cardData.spellList.Count; i++)
            {
                StringTableEntry entry = table.GetEntry($"{localizationKey}_spell_{i}");
                cardData.spellList[i].description = entry != null ? entry.Value : "";
            }
            
            for (int i = 0; i < cardData.passiveList.Count; i++)
            {
                StringTableEntry entry = table.GetEntry($"{localizationKey}_passive_{i}");
                cardData.passiveList[i].description = entry != null ? entry.Value : "";
            }
            
            cardData.localizationKey = localizationKey;
        }

        [MenuItem("Tools/From CardData to Google Sheet")]
        private static void FillLocalizationFile()
        {
            CardDatabase db = AssetDatabase.LoadAssetAtPath<CardDatabase>(databasePath);
            StringTable table = AssetDatabase.LoadAssetAtPath<StringTable>(tablePath);
            
            foreach (CardData cardData in db.AllCards)
            {
                if (IsCardValidForLocalization(cardData))
                    AddNewEntryInTable(table, cardData);
            }
        }

        private static bool IsCardValidForLocalization(CardData cardData)
        {
            return !cardData.isEnemy && string.IsNullOrEmpty(cardData.localizationKey);
        }

        private static void AddNewEntryInTable(StringTable table, CardData cardData)
        {
            string localizationKey = ComputeLocalizationKey(cardData.cardName);

            for (int i = 0; i < cardData.spellList.Count; i++)
                AddKeyToTable(table, $"{localizationKey}_spell_{i}", cardData.spellList[i].description);
            
            for (int i = 0; i < cardData.passiveList.Count; i++)
                AddKeyToTable(table, $"{localizationKey}_passive_{i}", cardData.passiveList[i].description);

            cardData.localizationKey = localizationKey;
            
            Debug.Log($"[{nameof(LocalizationSystem)}] : Added new Entry : {localizationKey}");
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
