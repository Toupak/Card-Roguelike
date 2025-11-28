using BoomLib.Tools;
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
        private const string enemiesTablePath = "Assets/Localization/Tables/Enemies/Enemies_en.asset";

        public TMP_SpriteAsset spriteAsset;

        public static LocalizationSystem instance;

        [SerializeField] private StringTable spellTable;
        [SerializeField] private StringTable passiveTable;
        [SerializeField] private StringTable combatTable;
        [SerializeField] private StringTable statusTable;
        [SerializeField] private StringTable enemiesTable;
        
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            //UpdateGlyphs();
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
        
        public string GetEnemyPassiveTitle(string cardKey, string passiveKey)
        {
            StringTableEntry entry = enemiesTable.GetEntry($"{cardKey}_{passiveKey}_title");
            
            return entry != null ? entry.Value : "";
        }
        
        public string GetEnemyPassiveDescription(string cardKey, string passiveKey)
        {
            StringTableEntry entry = enemiesTable.GetEntry($"{cardKey}_{passiveKey}");
            
            return entry != null ? entry.Value : "";
        }
        
        public string GetEnemyBehaviourTitle(string cardKey, string behaviourKey)
        {
            StringTableEntry entry = enemiesTable.GetEntry($"{cardKey}_{behaviourKey}_title");
            
            return entry != null ? entry.Value : "";
        }
        
        public string GetEnemyBehaviourDescription(string cardKey, string behaviourKey)
        {
            StringTableEntry entry = enemiesTable.GetEntry($"{cardKey}_{behaviourKey}");
            
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
                
                Debug.Log($"Create new Status : {key} => {data.statusName} / {data.statusDescription}");
            }
        }

        private static string ComputeLocalizationKey(string cardDataCardName)
        {
            return cardDataCardName.ToLower().RemoveWhitespace();
        }
    }
}
