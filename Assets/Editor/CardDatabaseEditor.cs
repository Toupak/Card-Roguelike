using System.Collections.Generic;
using System.Linq;
using Cards.Scripts;
using Data;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(CardDatabase))]
    public class CardDatabaseEditor : UnityEditor.Editor
    {
        private CardDatabase db;
        private Vector2 scrollPos;
        private string _globalFilter = "All";
        private string _rarityFilter = "All";
        private string _enemyFilter = "All";
        private string _specialSummonFilter = "All";
        private List<bool> foldouts = new List<bool>();
        private HashSet<int> wrongCardNumbers = new();
        private int _lowestMissingNumber = -1;
        private int _highestNumber = 0;

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

        
        private void SyncNumbers()
        {
            List<CardData> orderedCards = new List<CardData>(db.AllCards);
            orderedCards.Sort((a, b) => a.cardNumber.CompareTo(b.cardNumber));
            
            _lowestMissingNumber = -1;
            _highestNumber = 0;
            int expectedNumber = 1;
            
            wrongCardNumbers.Clear();
            foreach (var card in orderedCards)
            {
                if (card == null || card.cardNumber <= 0) 
                    continue;
                if (card.cardNumber < expectedNumber)
                {
                    wrongCardNumbers.Add(card.cardNumber);
                }
                else if (card.cardNumber == expectedNumber)
                {
                    _highestNumber = Mathf.Max(_highestNumber, expectedNumber);
                    expectedNumber++;
                }
                else
                {
                    if (_lowestMissingNumber == -1) 
                        _lowestMissingNumber = expectedNumber;
                    expectedNumber++;
                }
            }
            if (_lowestMissingNumber == -1) 
                _lowestMissingNumber = expectedNumber;
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

            if (GUILayout.Button("Reset ids"))
            {
                foreach (var card in db.AllCards)
                {
                    if (card != null)
                    {
                        SerializedObject cardSO = new SerializedObject(card);
                        SerializedProperty propNumber = cardSO.FindProperty("cardNumber");
                        cardSO.Update();
                        propNumber.intValue = 0;
                        cardSO.ApplyModifiedProperties();
                        EditorUtility.SetDirty(card);
                    }
                }
                return;
            }

            /*
            if (GUILayout.Button("Set ids by rarity"))
            {
                // order by rarity
                var orderedCards = new List<CardData>(db.AllCards);
                orderedCards.Sort((a, b) => a.Rarity.CompareTo(b.Rarity));
                int id = 1;
                foreach (var card in orderedCards)
                {
                    if (card != null)
                    {
                        SerializedObject cardSO = new SerializedObject(card);
                        SerializedProperty propNumber = cardSO.FindProperty("_number");
                        cardSO.Update();
                        propNumber.intValue = id;
                        cardSO.ApplyModifiedProperties();
                        EditorUtility.SetDirty(card);
                        id++;
                    }
                }
                return;
            }
            */

            
            if (GUILayout.Button("Order Cards By Number"))
            {
                db.Sort((a, b) =>
                {
                    int A = a.cardNumber;
                    int B = b.cardNumber;

                    int groupA = A > 0 ? 0 : (A == 0 ? 1 : 2);
                    int groupB = B > 0 ? 0 : (B == 0 ? 1 : 2);

                    if (groupA != groupB)
                        return groupA.CompareTo(groupB);

                    if (groupA == 0)
                        return A.CompareTo(B);
                    else if (groupA == 1)   // zéros → tous égaux
                        return 0;
                    else                    // négatifs → décroissant
                        return B.CompareTo(A);
                });
                foldouts.Clear();
                SyncFoldouts();
                EditorUtility.SetDirty(db);
                return;
            }
            

            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Lowest Missing Number:", _lowestMissingNumber.ToString(), EditorStyles.helpBox);
            EditorGUILayout.LabelField("Highest Number:", _highestNumber.ToString(), EditorStyles.helpBox);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Filter:", GUILayout.Width(40));
            if (GUILayout.Toggle(_globalFilter == "All", $"All ({db.AllCards.Count})", EditorStyles.miniButtonLeft)) _globalFilter = "All";
            if (GUILayout.Toggle(_globalFilter == "Incomplete", $"Incomplete ({db.AllCards.Count(x => IsIncomplete(x, db.AllCards.ToList().IndexOf(x)))})", EditorStyles.miniButtonMid)) _globalFilter = "Incomplete";
            if (GUILayout.Toggle(_globalFilter == "WrongNumber", $"Wrong Number ({db.AllCards.Count(x => wrongCardNumbers.Contains(x.cardNumber))})", EditorStyles.miniButtonRight)) _globalFilter = "WrongNumber";
            EditorGUILayout.EndHorizontal();


            // Applique le filtre global avant de compter les raretés
            
            IEnumerable<CardData> filteredCards = db.AllCards;
            switch (_globalFilter)
            {
                case "Incomplete":
                    filteredCards = filteredCards.Where(x => IsIncomplete(x));
                    break;
                case "WrongNumber":
                    filteredCards = filteredCards.Where(x => wrongCardNumbers.Contains(x.cardNumber));
                    break;
            }
            
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
            
            EditorGUILayout.Space();

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            SyncFoldouts();
            SyncNumbers();

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
                SerializedProperty propNumber = cardSO.FindProperty("cardNumber");
                SerializedProperty propArtwork = cardSO.FindProperty("artwork");
                SerializedProperty propEnemyDifficulty = cardSO.FindProperty("enemyDifficulty");
                SerializedProperty propEnemyFloor = cardSO.FindProperty("enemyFloor");
                //SerializedProperty propAlternatePrefab = cardSO.FindProperty("_alternatePrefab");

                cardSO.Update();

                bool isIncomplete = IsIncomplete(card);

                if (_globalFilter == "Incomplete" && !isIncomplete && !foldouts[i]) 
                    continue;
                if (_globalFilter == "WrongNumber" && !foldouts[i] && !wrongCardNumbers.Contains(propNumber.intValue)) 
                    continue;
                if (_rarityFilter != "All" && propRarity.enumNames[propRarity.enumValueIndex] != _rarityFilter && !foldouts[i]) 
                    continue;
                if (_enemyFilter != "All" && card.isEnemy.ToString() != _enemyFilter && !foldouts[i]) 
                    continue;
                if (_specialSummonFilter != "All" && card.isSpecialSummon.ToString() != _specialSummonFilter && !foldouts[i]) 
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

                // Petit label fixe
                GUILayout.Label("#", GUILayout.Width(12));
                // Champ numérique sans label
                EditorGUILayout.PropertyField(propNumber, GUIContent.none, GUILayout.Width(40));

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

        private bool IsIncomplete(CardData card, int indexOf = -42)
        {
            //if (indexOf != -42)
              //  Debug.Log($"Wtf : {indexOf}");
            
            return string.IsNullOrEmpty(card.cardName) ||
                   string.IsNullOrEmpty(card.description) ||
                   card.cardNumber == 0;
            
        }
    }
}
