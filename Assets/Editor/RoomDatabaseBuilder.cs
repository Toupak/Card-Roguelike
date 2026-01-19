using System.Collections.Generic;
using Map.Rooms;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public abstract class RoomDatabaseBuilder
    {
        private const string databasePath = "Assets/MapMaker/Rooms/RoomDataBase.asset";

        [MenuItem("Tools/RoomDatabase/Build Room Database")]
        public static void BuildDatabase()
        {
            string[] guids = AssetDatabase.FindAssets("t:RoomData", new[] { "Assets/MapMaker/Rooms/Data" });
            List<RoomData> allRooms = new List<RoomData>();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                RoomData room = AssetDatabase.LoadAssetAtPath<RoomData>(path);
                if (room != null)
                    allRooms.Add(room);
            }

            RoomDataBase db = AssetDatabase.LoadAssetAtPath<RoomDataBase>(databasePath);

            if (db == null)
            {
                db = ScriptableObject.CreateInstance<RoomDataBase>();
                AssetDatabase.CreateAsset(db, databasePath);
                Debug.Log("CardDatabase asset created.");
            }

            SerializedObject dbSO = new SerializedObject(db);
            SerializedProperty allRoomsProperty = dbSO.FindProperty("allRooms");

            allRoomsProperty.ClearArray();

            for (int i = 0; i < allRooms.Count; i++)
            {
                allRoomsProperty.InsertArrayElementAtIndex(i);
                allRoomsProperty.GetArrayElementAtIndex(i).objectReferenceValue = allRooms[i];
            }

            dbSO.ApplyModifiedProperties();

            EditorUtility.SetDirty(db);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"Database built with {allRooms.Count} rooms");
        }
    }
}
