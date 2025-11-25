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
        private const string spellTablePath = "Assets/Localization/Tables/Spells/Spells_en.asset";
        private const string passiveTablePath = "Assets/Localization/Tables/Passives/Passives_en.asset";

        public TMP_SpriteAsset spriteAsset;
        public TextToIconData textToIconData;

        public static LocalizationSystem instance;
        public TextToIcon textToIcon { get; private set; }

        private StringTable spellTable;
        private StringTable passiveTable;
        
        private void Awake()
        {
            instance = this;
            
            textToIcon = GetComponent<TextToIcon>();
            spellTable = AssetDatabase.LoadAssetAtPath<StringTable>(spellTablePath);
            passiveTable = AssetDatabase.LoadAssetAtPath<StringTable>(passiveTablePath);
            
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
    }
}
