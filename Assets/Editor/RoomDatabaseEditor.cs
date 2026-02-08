using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cards.Scripts;
using Codice.Client.BaseCommands.Merge.ApplyMergeOperations;
using Map.Rooms;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Editor
{
    [CustomEditor(typeof(RoomDataBase))]
    public class RoomDatabaseEditor : UnityEditor.Editor
    {
        private RoomDataBase db;
        private Vector2 scrollPos;
        private string _globalfilter = "All";
        private string _doorCountfilter = "All";
        private List<bool> foldouts = new List<bool>();
        private List<RoomData> invalidRooms = new();

        private SerializedProperty allRoomsProperty;

        private void OnEnable()
        {
            db = (RoomDataBase)target;

            serializedObject.Update();
            allRoomsProperty = serializedObject.FindProperty("allRooms");
            SyncFoldouts();
        }

        private void SyncFoldouts()
        {
            if (db.AllRooms == null)
            {
                db.InitializeEmptyList();

                serializedObject.Update();
                allRoomsProperty = serializedObject.FindProperty("allRooms");
            }

            while (foldouts.Count < allRoomsProperty.arraySize)
                foldouts.Add(false);

            if (foldouts.Count > allRoomsProperty.arraySize)
                foldouts.RemoveRange(allRoomsProperty.arraySize, foldouts.Count - allRoomsProperty.arraySize);
        }
        
        private void SyncDoorNumbers()
        {
            List<RoomData> allRooms = new List<RoomData>(db.AllRooms);
            
            invalidRooms.Clear();
            foreach (RoomData room in allRooms)
            {
                if (room == null || !room.isIncomplete)
                    continue;

                invalidRooms.Add(room);
            }
        }

        public override void OnInspectorGUI()
        {
            if (EditorApplication.isCompiling || EditorApplication.isUpdating || Selection.activeObject != target) 
                return;

            serializedObject.Update();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Room Database", EditorStyles.boldLabel);

            if (GUILayout.Button("Generate Rooms From Scenes"))
            {
                RoomDataGenerator.GenerateRooms();
                return;
            }

            if (GUILayout.Button("Rebuild Database"))
            {
                RoomDatabaseBuilder.BuildDatabase();
                return;
            }

            /*
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
            !/

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

            if (GUILayout.Button("Order Cards By Door Number"))
            {
                db.Sort((a, b) =>
                {
                    int A = a.DoorCount();
                    int B = b.DoorCount();

                    int groupA = A > 0 ? 0 : (A == 0 ? 1 : 2);
                    int groupB = B > 0 ? 0 : (B == 0 ? 1 : 2);

                    if (groupA != groupB)
                        return groupA.CompareTo(groupB);

                    if (groupA == 0)
                        return A.CompareTo(B);
                    else if (groupA == 1)   // zéros ? tous égaux
                        return 0;
                    else                    // négatifs ? décroissant
                        return B.CompareTo(A);
                });
                foldouts.Clear();
                SyncFoldouts();
                EditorUtility.SetDirty(db);
                return;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Filter:", GUILayout.Width(40));
            if (GUILayout.Toggle(_globalfilter == "All", $"All ({db.AllRooms.Count})", EditorStyles.miniButtonLeft)) 
                _globalfilter = "All";
            if (GUILayout.Toggle(_globalfilter == "Incomplete", $"Incomplete ({db.AllRooms.Count(x => IsIncomplete(x, db.AllRooms.ToList().IndexOf(x)))})", EditorStyles.miniButtonMid)) 
                _globalfilter = "Incomplete";
            EditorGUILayout.EndHorizontal();
            
            // Applique le filtre global
            IEnumerable<RoomData> filteredCards = db.AllRooms;
            switch (_globalfilter)
            {
                case "Incomplete":
                    filteredCards = filteredCards.Where(x => IsIncomplete(x));
                    break;
            }

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Rarity:", GUILayout.Width(40));
            if (GUILayout.Toggle(_doorCountfilter == "All", $"All ({filteredCards.Count()})", EditorStyles.miniButtonLeft)) 
                _doorCountfilter = "All";
            if (GUILayout.Toggle(_doorCountfilter == "1", $"1 Door ({filteredCards.Count(x => x.DoorCount() == 1)})", EditorStyles.miniButtonMid)) 
                _doorCountfilter = "1";
            if (GUILayout.Toggle(_doorCountfilter == "2", $"2 Doors ({filteredCards.Count(x => x.DoorCount() == 2)})", EditorStyles.miniButtonMid)) 
                _doorCountfilter = "2";
            if (GUILayout.Toggle(_doorCountfilter == "3", $"3 Doors ({filteredCards.Count(x => x.DoorCount() == 3)})", EditorStyles.miniButtonRight)) 
                _doorCountfilter = "3";
            if (GUILayout.Toggle(_doorCountfilter == "4", $"4 Doors ({filteredCards.Count(x => x.DoorCount() == 4)})", EditorStyles.miniButtonRight)) 
                _doorCountfilter = "4";
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space();

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            SyncFoldouts();
            SyncDoorNumbers();

            for (int i = 0; i < allRoomsProperty.arraySize; i++)
            {
                SerializedProperty roomProperty = allRoomsProperty.GetArrayElementAtIndex(i);
                if (roomProperty == null || roomProperty.objectReferenceValue == null) 
                    continue;

                RoomData room = (RoomData)roomProperty.objectReferenceValue;
                if (room == null) 
                    continue;

                if (EditorApplication.isCompiling || EditorApplication.isUpdating)
                {
                    EditorGUILayout.LabelField("Loading...");
                    continue;
                }

                SerializedObject roomSO = new SerializedObject(room);
                SerializedProperty roomNameProperty = roomSO.FindProperty("roomName");
                SerializedProperty descriptionProperty = roomSO.FindProperty("description");
                SerializedProperty screenShotProperty = roomSO.FindProperty("screenShot");
                SerializedProperty topDoor = roomSO.FindProperty("hasTopDoor");
                SerializedProperty botDoor = roomSO.FindProperty("hasBotDoor");
                SerializedProperty rightDoor = roomSO.FindProperty("hasRightDoor");
                SerializedProperty leftDoor = roomSO.FindProperty("hasLeftDoor");
                int doorCount = room.DoorCount();

                roomSO.Update();

                bool isIncomplete = IsIncomplete(room);

                if (_globalfilter == "Incomplete" && !isIncomplete && !foldouts[i]) 
                    continue;
                if (_doorCountfilter != "All" && doorCount != int.Parse(_doorCountfilter)  && !foldouts[i]) 
                    continue;

                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.BeginHorizontal();

                
                
                // Récupération du Sprite (toujours depuis SerializedProperty, safe pour l'editor)
                Sprite sprite = (Sprite)screenShotProperty.objectReferenceValue;

                // Affichage dans un ObjectField pour pouvoir changer le sprite
                Sprite newSprite = (Sprite)EditorGUILayout.ObjectField(sprite, typeof(Sprite), false, GUILayout.Width(192), GUILayout.Height(108));

                // Appliquer le changement si modifié
                if (newSprite != sprite)
                {
                    screenShotProperty.objectReferenceValue = newSprite;
                }
                


                // ---- Nom et warning ----
                EditorGUILayout.BeginVertical();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(roomNameProperty, GUIContent.none);
                if (string.IsNullOrEmpty(roomNameProperty.stringValue))
                {
                    GUILayout.Label("?? Incomplete", GUILayout.Width(100));
                }
                EditorGUILayout.EndHorizontal();
                
                // ---- Flags, lien et foldout ----
                EditorGUILayout.BeginHorizontal();
                
                // Petit label fixe
                GUILayout.Label($"Door Count : {room.DoorCount()}", GUILayout.Width(100));
                
                GUILayout.FlexibleSpace();

                // Foldout
                foldouts[i] = EditorGUILayout.Foldout(foldouts[i], "", true);
                
                EditorGUILayout.EndHorizontal(); // end flags + link + foldout
                
                EditorGUILayout.BeginHorizontal(); // Buttons

                if (GUILayout.Button("Asset", GUILayout.Width(50)))
                {
                    Selection.activeObject = room;
                    EditorGUIUtility.PingObject(room);
                }
                
                if (GUILayout.Button("Open", GUILayout.Width(50)))
                {
                    EditorSceneManager.SaveOpenScenes();
                    EditorSceneManager.OpenScene(room.scenePath, OpenSceneMode.Single);
                    break;
                }
                
                if (GUILayout.Button("ScreenShot", GUILayout.Width(80)))
                {
                    string filepath = Application.dataPath + $"/Resources/Screenshots/{room.roomName}.png";
                    string metapath = Application.dataPath + $"/Resources/Screenshots/{room.roomName}.png.meta";
                    
                    if (File.Exists(filepath))
                    {
                        File.Delete(filepath);
                        File.Delete(metapath);
                        Debug.Log("Old Snapshot deleted !");
                        AssetDatabase.Refresh();
                    }
                    
                    ScreenCapture.CaptureScreenshot(filepath);
                    Debug.Log("Screenshot Taken, stored at " + filepath);
                }
                
                if (GUILayout.Button("Update", GUILayout.Width(80)))
                {
                    string filepath = $"Assets/Resources/Screenshots/{room.roomName}.png";
                    
                    AssetDatabase.Refresh();
                    
                    Debug.Log($"Update : {filepath}");
                    
                    TextureImporter importer = TextureImporter.GetAtPath(filepath) as TextureImporter;
                    importer.textureType = TextureImporterType.Sprite;
                    importer.spriteImportMode = SpriteImportMode.Single;

                    Object asset = AssetDatabase.LoadAssetAtPath(importer.assetPath, typeof(Texture2D));
                    AssetDatabase.ImportAsset(importer.assetPath, ImportAssetOptions.ForceUpdate);
                    Debug.Log("Texture Configuration Complete !");

                    Object[] assets = AssetDatabase.LoadAllAssetRepresentationsAtPath(filepath);
                    
                    foreach (var obj in assets)
                    {
                        if (obj is Sprite screen)
                        {
                            screenShotProperty.objectReferenceValue = screen;
                        }
                    }
                }

                EditorGUILayout.EndHorizontal(); // Buttons
                
                if (foldouts[i])
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.LabelField("Description:");
                    descriptionProperty.stringValue = EditorGUILayout.TextArea(descriptionProperty.stringValue, GUILayout.Height(60));

                    EditorGUI.indentLevel--;
                }
                
                EditorGUILayout.Separator();
                EditorGUILayout.BeginVertical();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Top Door : ", GUILayout.Width(150));
                EditorGUILayout.PropertyField(topDoor, GUIContent.none, GUILayout.Width(150));
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Right Door : ", GUILayout.Width(150));
                EditorGUILayout.PropertyField(rightDoor, GUIContent.none, GUILayout.Width(150));
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Bot Door : ", GUILayout.Width(150));
                EditorGUILayout.PropertyField(botDoor, GUIContent.none, GUILayout.Width(150));
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Left Door : ", GUILayout.Width(150));
                EditorGUILayout.PropertyField(leftDoor, GUIContent.none, GUILayout.Width(150));
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.EndVertical();
                
                /*
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Is Special Summon", GUILayout.Width(120));
                EditorGUILayout.PropertyField(propSpecialSummon, GUIContent.none, GUILayout.Width(80));
                EditorGUILayout.EndHorizontal();
                */
                
                EditorGUILayout.EndVertical();   // end vertical nom + warning + flags
                EditorGUILayout.EndHorizontal(); // end horizontal header

                EditorGUILayout.EndVertical();   // end box

                EditorGUILayout.Space();

                if (roomSO.hasModifiedProperties)
                {
                    Undo.RecordObject(room, "Edit RoomData");
                    roomSO.ApplyModifiedProperties();
                    EditorUtility.SetDirty(room);
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

        private bool IsIncomplete(RoomData room, int indexOf = -42)
        {
            return string.IsNullOrEmpty(room.roomName) || room.isIncomplete;
        }
    }
}
