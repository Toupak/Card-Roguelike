using System.Collections.Generic;
using System.IO;
using Map.Rooms;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public abstract class RoomDataGenerator
    {
        [MenuItem("Tools/RoomDatabase/Generate Rooms From Scenes")]
        public static void GenerateRooms()
        {
            string roomPath = "Assets/MapMaker/Rooms/Data";
            if (!System.IO.Directory.Exists(roomPath))
                System.IO.Directory.CreateDirectory(roomPath);

            string[] guids = AssetDatabase.FindAssets("t:Scene", new[] { "Assets/Scenes/Rooms" });

            List<string> created = new List<string>();
            
            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string sceneName = Path.GetFileName(path).Split('.')[0];
                
                if (sceneName.Equals("RoomTemplate"))
                    continue;

                string assetPath = Path.Combine(roomPath, $"{sceneName}.asset");

                RoomData room = AssetDatabase.LoadAssetAtPath<RoomData>(assetPath);
                if (room == null)
                {
                    room = ScriptableObject.CreateInstance<RoomData>();

                    SerializedObject so = new SerializedObject(room);
                    so.FindProperty("roomName").stringValue = sceneName;
                    so.FindProperty("scenePath").stringValue = path;
                    so.ApplyModifiedProperties();

                    AssetDatabase.CreateAsset(room, assetPath);
                    created.Add(path);
                    Debug.Log($"Created room : {room.roomName}");
                }
            }

            EditorBuildSettingsScene[] original = EditorBuildSettings.scenes;

            for (int i = 0; i < original.Length; i++)
            {
                if (created.Contains(original[i].path))
                    created.RemoveAt(created.IndexOf(original[i].path));
            }
            
            EditorBuildSettingsScene[] newSettings = new EditorBuildSettingsScene[original.Length + created.Count]; 
            System.Array.Copy(original, newSettings, original.Length);
            
            int index = original.Length;
            for (int i = 0; i < created.Count; i ++)
            {
                EditorBuildSettingsScene sceneToAdd = new EditorBuildSettingsScene(created[i], true); 
                newSettings[index] = sceneToAdd;
			
                index ++;
            }
            
            EditorBuildSettings.scenes = newSettings;

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
