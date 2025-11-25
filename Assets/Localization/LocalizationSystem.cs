using System;
using BoomLib.Tools;
using Cards.Scripts;
using Data;
using Localization.Icons_In_Text;
using Passives;
using Spells;
using Status;
using Status.Data;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.TextCore;

namespace Localization
{
    public class LocalizationSystem : MonoBehaviour
    {
        private const string spellTablePath = "Assets/Localization/Tables/Spells/Spells_en.asset";
        private const string passiveTablePath = "Assets/Localization/Tables/Passives/Passives_en.asset";
        private const string combatTablePath = "Assets/Localization/Tables/Combat/Combat_en.asset";
        private const string statusTablePath = "Assets/Localization/Tables/Status/Status_en.asset";

        public TMP_SpriteAsset spriteAsset;
        public TextToIconData textToIconData;

        public static LocalizationSystem instance;
        public TextToIcon textToIcon { get; private set; }

        private StringTable spellTable;
        private StringTable passiveTable;
        private StringTable combatTable;
        private StringTable statusTable;
        
        private void Awake()
        {
            instance = this;
            
            textToIcon = GetComponent<TextToIcon>();
            spellTable = AssetDatabase.LoadAssetAtPath<StringTable>(spellTablePath);
            passiveTable = AssetDatabase.LoadAssetAtPath<StringTable>(passiveTablePath);
            combatTable = AssetDatabase.LoadAssetAtPath<StringTable>(combatTablePath);
            statusTable = AssetDatabase.LoadAssetAtPath<StringTable>(statusTablePath);
            
            //UpdateGlyphs();
        }

        private void Start()
        {
            //UpdateStatus();
        }
        
        public string GetSpellTitle(string cardKey, string spellKey)
        {
            StringTableEntry entry = spellTable.GetEntry($"{cardKey}_{spellKey}_title");
            
            return entry != null ? entry.Value : "";
        }
        
        public string GetSpellDescription(string cardKey, string spellKey)
        {
            StringTableEntry entry = spellTable.GetEntry($"{cardKey}_{spellKey}");
            
            return entry != null ? entry.Value : "";
        }
        
        public string GetPassiveTitle(string cardKey, string passiveKey)
        {
            StringTableEntry entry = passiveTable.GetEntry($"{cardKey}_{passiveKey}_title");
            
            return entry != null ? entry.Value : "";
        }
        
        public string GetPassiveDescription(string cardKey, string passiveKey)
        {
            StringTableEntry entry = passiveTable.GetEntry($"{cardKey}_{passiveKey}");
            
            return entry != null ? entry.Value : "";
        }
        
        public string GetStatusTitle(string statusKey)
        {
            StringTableEntry entry = statusTable.GetEntry($"{statusKey}_title");
            
            return entry != null ? entry.Value : "";
        }
        
        public string GetStatusDescription(string statusKey)
        {
            StringTableEntry entry = statusTable.GetEntry($"{statusKey}");
            
            return entry != null ? entry.Value : "";
        }

        public string GetCombatString(string key)
        {
            StringTableEntry entry = combatTable.GetEntry(key);
            
            return entry != null ? entry.Value : "";
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

        private void UpdateStatus()
        {
            foreach (StatusData data in StatusSystem.instance.statusData)
            {
                string key = ComputeLocalizationKey(data.name);

                data.localizationKey = key;
                EditorUtility.SetDirty(data);
                
                statusTable.AddEntry($"{key}_title", data.statusName);
                statusTable.AddEntry($"{key}", data.statusDescription);
                
                //Debug.Log($"Create new Status : {key} => {data.statusName} / {data.statusDescription}");
            }
        }

        private static string ComputeLocalizationKey(string cardDataCardName)
        {
            return cardDataCardName.ToLower().RemoveWhitespace();
        }
        
        [MenuItem("Tools/Add New Card To Trad File")]
        private static void AddNewCardToTradFile()
        {
            CardDatabase db = AssetDatabase.LoadAssetAtPath<CardDatabase>("Assets/Data/CardDatabase.asset");
            StringTable spell = AssetDatabase.LoadAssetAtPath<StringTable>(spellTablePath);
            StringTable passive = AssetDatabase.LoadAssetAtPath<StringTable>(passiveTablePath);
            
            foreach (CardData cardData in db.AllCards)
            {
                if (!cardData.isEnemy && string.IsNullOrEmpty(cardData.localizationKey))
                {
                    cardData.localizationKey = ComputeLocalizationKey(cardData.cardName);
                    Debug.Log($"Added Card : {cardData.localizationKey}");
                    
                    foreach (SpellData data in cardData.spellList)
                    {
                        data.localizationKey = ComputeLocalizationKey(data.name);
                        EditorUtility.SetDirty(data);

                        spell.AddEntry($"{cardData.localizationKey}_{data.localizationKey}_title", data.spellName);
                        spell.AddEntry($"{cardData.localizationKey}_{data.localizationKey}", data.description);
                        Debug.Log($"Added Spell : {data.localizationKey}");
                    }
                    
                    foreach (PassiveData data in cardData.passiveList)
                    {
                        data.localizationKey = ComputeLocalizationKey(data.name);
                        EditorUtility.SetDirty(data);
                        
                        passive.AddEntry($"{cardData.localizationKey}_{data.localizationKey}_title", data.passiveName);
                        passive.AddEntry($"{cardData.localizationKey}_{data.localizationKey}", data.description);
                        Debug.Log($"Added Passive : {data.localizationKey}");
                    }
                    
                    EditorUtility.SetDirty(cardData);
                }
            }
            
            EditorUtility.SetDirty(spell);
            EditorUtility.SetDirty(passive);
        }
        

        [MenuItem("Tools/SetAllDataAsDirty")]
        private static void SetAllDataAsDirty()
        {
            CardDatabase db = AssetDatabase.LoadAssetAtPath<CardDatabase>("Assets/Data/CardDatabase.asset");
            
            foreach (CardData cardData in db.AllCards)
            {
                if (!cardData.isEnemy)
                {
                    foreach (SpellData data in cardData.spellList)
                        EditorUtility.SetDirty(data);
                    
                    foreach (PassiveData data in cardData.passiveList)
                        EditorUtility.SetDirty(data);
                }
                
                EditorUtility.SetDirty(cardData);
            }
        }
    }
}
