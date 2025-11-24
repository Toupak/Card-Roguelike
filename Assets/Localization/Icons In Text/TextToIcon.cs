using System.Linq;
using UnityEngine;

namespace Localization.Icons_In_Text
{
    public class TextToIcon : MonoBehaviour
    {
        [SerializeField] private TextToIconData textToIconData;

        public string CheckForIcons(string message)
        {
            string finalString = message;
            string[] chunks = message.Split('$');
            
            foreach (string chunk in chunks)
            {
                if (textToIconData.rules.Where((r) => r.tag.Equals(chunk)).ToList().Count > 0)
                {
                    TextToIconRule rule = textToIconData.rules.First((r) => r.tag.Equals(chunk));
                    finalString = finalString.Replace($"${rule.tag}$", $"<sprite name=\"{rule.icon.name}\">");
                }
            }
            
            return finalString;
        }
    }
}
