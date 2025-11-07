using UnityEngine;

namespace Spells
{
    [CreateAssetMenu(fileName = "PassiveData", menuName = "Scriptable Objects/PassiveData")]
    public class PassiveData : ScriptableObject
    {
        public string passiveName;
        [TextArea] public string description;
        public Sprite icon;
    
        [Space]
        public PassiveController passiveController;
    }
}
