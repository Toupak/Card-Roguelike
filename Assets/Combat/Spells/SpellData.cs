using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells
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
        
        [Space]
        public TargetType targetType;
        public TargetingMode targetingMode;
        public int targetCount = 1;
        public bool targetTokens;
        
        [Space]
        public int energyCost;
        public bool hasNoCooldown;
        public int damage;
        public StatusType inflictStatus;
        public int statusStacksApplied;
        
        [Space]
        public SpellController spellController;
        
        [Space] 
        public string localizationKey;
    }
}
