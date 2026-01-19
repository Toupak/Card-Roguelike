using System.Collections.Generic;
using UnityEngine;

namespace Character_Selection.Character.Dialog
{
    [CreateAssetMenu(fileName = "DialogData", menuName = "Scriptable Objects/DialogData")]
    public class DialogData : ScriptableObject
    {
        [SerializeField, TextArea(1, 3)] List<string> dialogTexts;
        public List<string> DialogTexts => dialogTexts;
    }
}
