using BoomLib.Tools;
using Cards.Scripts;
using Data;
using Localization.Icons_In_Text;
using Passives;
using Spells;
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

        public TMP_SpriteAsset spriteAsset;
        public TextToIconData textToIconData;

        public static LocalizationSystem instance;
        public TextToIcon textToIcon { get; private set; }

        private StringTable spellTable;
        private StringTable passiveTable;
        private StringTable combatTable;
        
        private void Awake()
        {
            instance = this;
            
            textToIcon = GetComponent<TextToIcon>();
            spellTable = AssetDatabase.LoadAssetAtPath<StringTable>(spellTablePath);
            passiveTable = AssetDatabase.LoadAssetAtPath<StringTable>(passiveTablePath);
            combatTable = AssetDatabase.LoadAssetAtPath<StringTable>(combatTablePath);
            
            //UpdateGlyphs();
        }

        public string GetSpellDescription(string cardKey, string spellKey)
        {
            StringTableEntry entry = spellTable.GetEntry($"{cardKey}_{spellKey}");
            
            return entry != null ? entry.Value : "";
        }
        
        public string GetPassiveDescription(string cardKey, string passiveKey)
        {
            StringTableEntry entry = passiveTable.GetEntry($"{cardKey}_{passiveKey}");
            
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

        private static string ComputeLocalizationKey(string cardDataCardName)
        {
            return cardDataCardName.ToLower().RemoveWhitespace();
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
