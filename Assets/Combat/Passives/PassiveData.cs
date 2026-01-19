using UnityEngine;

namespace Combat.Passives
{
    [CreateAssetMenu(fileName = "PassiveData", menuName = "Scriptable Objects/PassiveData")]
    public class PassiveData : ScriptableObject
    {
        public string passiveName;
        [TextArea] public string description;
        public Sprite icon;
        public Color32 backgroundColor;
    
        [Space]
        public PassiveController passiveController;
        
        [Space] 
        public string localizationKey;
    }
}
