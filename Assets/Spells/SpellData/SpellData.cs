using UnityEngine;

namespace Spells.Data
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
        public string description;
        public Sprite icon;
        public TargetType targetType;
        public TargetingMode targetingMode;
        [Tooltip("Optional : only needed when TargetingMode is set to Multi")] public int targetCount = 1;
        public SpellController spellController;
    }
}
