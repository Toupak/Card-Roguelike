using Cards.Scripts;
using UnityEngine;

namespace Spells
{
    public enum TargetType
    {
        Ally,
        Enemy,
        Self,
        None
    }

    public enum TargetingMode
    {
        Single,
        Multi,
        All
    }
    
    [CreateAssetMenu(fileName = "SpellData", menuName = "Scriptable Objects/SpellData")]
    public class SpellData : ScriptableObject
    {
        public string spellName;
        [TextArea] public string description;
        public Sprite icon;
        public TargetType targetType;
        public TargetingMode targetingMode;
        [Tooltip("Optional : only needed when TargetingMode is set to Multi")] public int targetCount = 1;
        public int energyCost;
        public int damage;
        public StatusType inflictStatus;
        public int statusStacksApplied;
        public SpellController spellController;
    }
}
