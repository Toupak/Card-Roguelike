using System.IO;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class DeleteSaveTool
    {
        [MenuItem("Tools/Delete Save")]
        static void ClearPP()
        {
            string path = Application.persistentDataPath + "/Save/" + "SaveFile_1";
            File.Delete(path);
            
            Debug.Log("Save Deleted");
        }
    }
}
