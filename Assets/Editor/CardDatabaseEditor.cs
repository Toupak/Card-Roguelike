using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Scripts;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(CardDatabase))]
    public class CardDatabaseEditor : UnityEditor.Editor
    {
        private CardDatabase db;
        private Vector2 scrollPos;
        private string _rarityFilter = "All";
        private string _enemyFilter = "All";
        private string _specialSummonFilter = "All";
        private int selectedTribe = 0;
        private List<bool> foldouts = new List<bool>();
        private HashSet<int> wrongCardNumbers = new();

        private SerializedProperty allCardsProp;

        private void OnEnable()
        {
            db = (CardDatabase)target;

            serializedObject.Update();
            allCardsProp = serializedObject.FindProperty("_allCards");
            SyncFoldouts();
        }

        private void SyncFoldouts()
        {
            if (db.AllCards == null)
            {
                db.InitializeEmptyList();

                serializedObject.Update();
                allCardsProp = serializedObject.FindProperty("_allCards");
            }

            while (foldouts.Count < allCardsProp.arraySize)
                foldouts.Add(false);

            if (foldouts.Count > allCardsProp.arraySize)
                foldouts.RemoveRange(allCardsProp.arraySize, foldouts.Count - allCardsProp.arraySize);
        }

        public override void OnInspectorGUI()
        {
            if (EditorApplication.isCompiling || EditorApplication.isUpdating || Selection.activeObject != target) 
                return;

            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Card Database", EditorStyles.boldLabel);

            if (GUILayout.Button("Generate Cards From Sprites"))
            {
                CardDataGenerator.GenerateCards();
                return;
            }

            if (GUILayout.Button("Rebuild Database"))
            {
                CardDatabaseBuilder.BuildDatabase();
                return;
            }

           
            
            
            
            EditorGUILayout.Space();

            
            
            
            IEnumerable<CardData> filteredCards = db.AllCards;
            
            
            
            EditorGUILayout.BeginHorizontal();
            List<string> tribeOptions = Enum.GetNames(typeof(CardData.Tribe)).ToList();
            for (int i = 0; i < tribeOptions.Count; i++)
            {
                tribeOptions[i] = $"{tribeOptions[i]} ({filteredCards.Count(x => x.tribe == (CardData.Tribe)i)})";
            }
            tribeOptions.Insert(0, $"All ({filteredCards.Count()})");
            
            selectedTribe = EditorGUILayout.Popup("Tribe:", selectedTribe, tribeOptions.ToArray());
            EditorGUILayout.EndHorizontal();
            
            if (selectedTribe > 0)
                filteredCards = filteredCards.Where(x => x.tribe == (CardData.Tribe)selectedTribe - 1);
            
            
            
            
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Enemy:", GUILayout.Width(40));
            if (GUILayout.Toggle(_enemyFilter == "All", $"All ({filteredCards.Count()})", EditorStyles.miniButtonLeft)) _enemyFilter = "All";
            if (GUILayout.Toggle(_enemyFilter == "False", $"Player ({filteredCards.Count(x => !x.isEnemy)})", EditorStyles.miniButtonMid)) _enemyFilter = "False";
            if (GUILayout.Toggle(_enemyFilter == "True", $"Enemy ({filteredCards.Count(x => x.isEnemy)})", EditorStyles.miniButtonRight)) _enemyFilter = "True";
            EditorGUILayout.EndHorizontal();

            switch (_enemyFilter)
            {
                case "False":
                    filteredCards = filteredCards.Where(x => !x.isEnemy);
                    break;
                case "True":
                    filteredCards = filteredCards.Where(x => x.isEnemy);
                    break;
            }
            
            

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Special Summon:", GUILayout.Width(40));
            if (GUILayout.Toggle(_specialSummonFilter == "All", $"All ({filteredCards.Count()})", EditorStyles.miniButtonLeft)) _specialSummonFilter = "All";
            if (GUILayout.Toggle(_specialSummonFilter == "False", $"Regular ({filteredCards.Count(x => !x.isSpecialSummon)})", EditorStyles.miniButtonMid)) _specialSummonFilter = "False";
            if (GUILayout.Toggle(_specialSummonFilter == "True", $"Special Summon ({filteredCards.Count(x => x.isSpecialSummon)})", EditorStyles.miniButtonRight)) _specialSummonFilter = "True";
            EditorGUILayout.EndHorizontal();
            
            
            switch (_specialSummonFilter)
            {
                case "False":
                    filteredCards = filteredCards.Where(x => !x.isSpecialSummon);
                    break;
                case "True":
                    filteredCards = filteredCards.Where(x => x.isSpecialSummon);
                    break;
            }
            
            
            
            

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Rarity:", GUILayout.Width(40));
            if (GUILayout.Toggle(_rarityFilter == "All", $"All ({filteredCards.Count()})", EditorStyles.miniButtonLeft)) _rarityFilter = "All";
            if (GUILayout.Toggle(_rarityFilter == "Common", $"Common ({filteredCards.Count(x => x.rarity == CardData.Rarity.Common)})", EditorStyles.miniButtonMid)) _rarityFilter = "Common";
            if (GUILayout.Toggle(_rarityFilter == "Rare", $"Rare ({filteredCards.Count(x => x.rarity == CardData.Rarity.Rare)})", EditorStyles.miniButtonMid)) _rarityFilter = "Rare";
            if (GUILayout.Toggle(_rarityFilter == "Legendary", $"Legendary ({filteredCards.Count(x => x.rarity == CardData.Rarity.Legendary)})", EditorStyles.miniButtonMid)) _rarityFilter = "Legendary";
            if (GUILayout.Toggle(_rarityFilter == "Exotic", $"Exotic ({filteredCards.Count(x => x.rarity == CardData.Rarity.Exotic)})", EditorStyles.miniButtonRight)) _rarityFilter = "Exotic";
            EditorGUILayout.EndHorizontal();
            
            /*
            switch (_rarityFilter)
            {
                case "Common":
                    filteredCards = filteredCards.Where(x => x.rarity == CardData.Rarity.Common);
                    break;
                case "Rare":
                    filteredCards = filteredCards.Where(x => x.rarity == CardData.Rarity.Rare);
                    break;
                case "Legendary":
                    filteredCards = filteredCards.Where(x => x.rarity == CardData.Rarity.Legendary);
                    break;
                case "Exotic":
                    filteredCards = filteredCards.Where(x => x.rarity == CardData.Rarity.Exotic);
                    break;
            }
            */
            
            
            
            
            EditorGUILayout.Space();
            
            
            
            

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            SyncFoldouts();

            for (int i = 0; i < allCardsProp.arraySize; i++)
            {
                SerializedProperty cardProp = allCardsProp.GetArrayElementAtIndex(i);
                if (cardProp == null || cardProp.objectReferenceValue == null) 
                    continue;

                CardData card = (CardData)cardProp.objectReferenceValue;
                if (card == null) 
                    continue;

                if (EditorApplication.isCompiling || EditorApplication.isUpdating)
                {
                    EditorGUILayout.LabelField("Loading...");
                    continue;
                }

                SerializedObject cardSO = new SerializedObject(card);
                SerializedProperty propCardName = cardSO.FindProperty("cardName");
                SerializedProperty propDescription = cardSO.FindProperty("description");
                SerializedProperty propTribe = cardSO.FindProperty("tribe");
                SerializedProperty propRarity = cardSO.FindProperty("rarity");
                SerializedProperty propHpMax = cardSO.FindProperty("hpMax");
                SerializedProperty propSpecialSummon = cardSO.FindProperty("isSpecialSummon");
                SerializedProperty propArtwork = cardSO.FindProperty("artwork");
                //SerializedProperty propAlternatePrefab = cardSO.FindProperty("_alternatePrefab");

                cardSO.Update();
                
                if (_rarityFilter != "All" && propRarity.enumNames[propRarity.enumValueIndex] != _rarityFilter && !foldouts[i]) 
                    continue;
                if (_enemyFilter != "All" && card.isEnemy.ToString() != _enemyFilter && !foldouts[i]) 
                    continue;
                if (_specialSummonFilter != "All" && card.isSpecialSummon.ToString() != _specialSummonFilter && !foldouts[i]) 
                    continue;
                if (selectedTribe > 0 && card.tribe != (CardData.Tribe)selectedTribe - 1)
                    continue;

                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.BeginHorizontal();

                // Récupération du Sprite (toujours depuis SerializedProperty, safe pour l'editor)
                Sprite sprite = (Sprite)propArtwork.objectReferenceValue;

                // Affichage dans un ObjectField pour pouvoir changer le sprite
                Sprite newSprite = (Sprite)EditorGUILayout.ObjectField(sprite, typeof(Sprite), false, GUILayout.Width(60), GUILayout.Height(60));

                // Appliquer le changement si modifié
                if (newSprite != sprite)
                {
                    propArtwork.objectReferenceValue = newSprite;
                }


                // ---- Nom et warning ----
                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(propCardName, GUIContent.none);
                if (string.IsNullOrEmpty(propCardName.stringValue) || string.IsNullOrEmpty(propDescription.stringValue))
                {
                    GUILayout.Label("⚠️ Incomplete", GUILayout.Width(100));
                }
                EditorGUILayout.EndHorizontal();
                
                // ---- Flags, lien et foldout ----
                EditorGUILayout.BeginHorizontal();
                
                GUILayout.Label("Hp", GUILayout.Width(25));
                EditorGUILayout.PropertyField(propHpMax, GUIContent.none, GUILayout.Width(80));

                if (GUILayout.Button("🔗", GUILayout.Width(25)))
                {
                    Selection.activeObject = card;
                    EditorGUIUtility.PingObject(card);
                }

                GUILayout.FlexibleSpace();

                // Foldout
                foldouts[i] = EditorGUILayout.Foldout(foldouts[i], "", true);
                
                EditorGUILayout.EndHorizontal(); // end flags + link + foldout
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(propRarity, GUIContent.none, GUILayout.Width(80));
                EditorGUILayout.PropertyField(propTribe, GUIContent.none, GUILayout.Width(80));
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Is Special Summon", GUILayout.Width(120));
                EditorGUILayout.PropertyField(propSpecialSummon, GUIContent.none, GUILayout.Width(80));
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.EndVertical();   // end vertical nom + warning + flags
                EditorGUILayout.EndHorizontal(); // end horizontal header

                if (foldouts[i])
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.LabelField("Description:");
                    propDescription.stringValue = EditorGUILayout.TextArea(propDescription.stringValue, GUILayout.Height(60));

                    //EditorGUILayout.PropertyField(propAlternatePrefab);

                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.EndVertical();   // end box

                EditorGUILayout.Space();

                if (cardSO.hasModifiedProperties)
                {
                    Undo.RecordObject(card, "Edit CardData");
                    cardSO.ApplyModifiedProperties();
                    EditorUtility.SetDirty(card);
                }
            }

            EditorGUILayout.EndScrollView();

            if (serializedObject.hasModifiedProperties)
                serializedObject.ApplyModifiedProperties();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(db);
            }
        }
    }
}
