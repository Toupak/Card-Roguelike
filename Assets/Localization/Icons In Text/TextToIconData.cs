using System;
using System.Collections.Generic;
using UnityEngine;

namespace Localization.Icons_In_Text
{
    [Serializable]
    public class TextToIconRule
    {
        public string tag;
        public Sprite icon;
    }
    
    [CreateAssetMenu(fileName = "TextToIconData", menuName = "Scriptable Objects/TextToIconData")]
    public class TextToIconData : ScriptableObject
    {
        [SerializeField] public List<TextToIconRule> rules;
    }
}
