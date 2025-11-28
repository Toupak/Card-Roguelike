using BoomLib.Tools;
using Cards.Scripts;
using Data;
using EnemyAttack;
using Passives;
using Spells;
using UnityEditor;
using UnityEngine;
using UnityEngine.Localization.Tables;

namespace Editor
{
    public static class LocalizationTools
    {
        private const string cardDBPath = "Assets/Data/CardDatabase.asset";
        private const string spellTablePath = "Assets/Localization/Tables/Spells/Spells_en.asset";
        private const string passiveTablePath = "Assets/Localization/Tables/Passives/Passives_en.asset";
        private const string combatTablePath = "Assets/Localization/Tables/Combat/Combat_en.asset";
        private const string statusTablePath = "Assets/Localization/Tables/Status/Status_en.asset";
        private const string enemiesTablePath = "Assets/Localization/Tables/Enemies/Enemies_en.asset";

        [MenuItem("Tools/Add New Card To Trad File")]
        private static void AddNewCardToTradFile()
        {
            CardDatabase db = AssetDatabase.LoadAssetAtPath<CardDatabase>(cardDBPath);
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
        
        [MenuItem("Tools/Add Enemies To Trad File")]
        private static void AddEnemiesToTradFile()
        {
            CardDatabase db = AssetDatabase.LoadAssetAtPath<CardDatabase>(cardDBPath);
            StringTable enemies = AssetDatabase.LoadAssetAtPath<StringTable>(enemiesTablePath);

            foreach (CardData cardData in db.AllCards)
            {
                if (cardData.isEnemy && string.IsNullOrEmpty(cardData.localizationKey))
                {
                    cardData.localizationKey = ComputeLocalizationKey(cardData.cardName);
                    Debug.Log($"Added Card : {ComputeLocalizationKey(cardData.cardName)}");
                    
                    foreach (BaseEnemyBehaviour data in cardData.enemyBehaviours)
                    {
                        data.localizationKey = ComputeLocalizationKey(data.name);
                        EditorUtility.SetDirty(data);

                        enemies.AddEntry($"{cardData.localizationKey}_{data.localizationKey}_title", data.behaviourName);
                        enemies.AddEntry($"{cardData.localizationKey}_{data.localizationKey}", data.description);
                        Debug.Log($"Added Spell : {ComputeLocalizationKey(data.name)}");
                    }
                    
                    foreach (PassiveData data in cardData.passiveList)
                    {
                        data.localizationKey = ComputeLocalizationKey(data.name);
                        EditorUtility.SetDirty(data);
                        
                        enemies.AddEntry($"{cardData.localizationKey}_{data.localizationKey}_title", data.passiveName);
                        enemies.AddEntry($"{cardData.localizationKey}_{data.localizationKey}", data.description);
                        Debug.Log($"Added Passive : {ComputeLocalizationKey(data.name)}");
                    }
                    
                    EditorUtility.SetDirty(cardData);
                }
            }
            
            EditorUtility.SetDirty(enemies);
        }
        

        [MenuItem("Tools/SetAllDataAsDirty")]
        private static void SetAllDataAsDirty()
        {
            CardDatabase db = AssetDatabase.LoadAssetAtPath<CardDatabase>(cardDBPath);
            
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
        
        private static string ComputeLocalizationKey(string cardDataCardName)
        {
            return cardDataCardName.ToLower().RemoveWhitespace();
        }
    }
}
