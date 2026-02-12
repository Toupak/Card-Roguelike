using Cards.Scripts;
using UnityEngine;

namespace Combat.Status.Data
{
    [CreateAssetMenu(fileName = "StatusData", menuName = "Scriptable Objects/StatusData")]
    public class StatusData : ScriptableObject
    {
        public enum EffectType
        {
            Neutral,
            Positive,
            Negative
        }
        
        public StatusType type;
        public string statusName;
        public string statusTag;
        [TextArea] public string statusDescription;
        public StatusBehaviour behaviour;
        public StatusBehaviourTimings behaviourTiming;
        public EffectType effectType;
        public int maxStackCount = -1;
        public bool neverRemoveTab;

        [Space] 
        public StatusController statusController;
        
        [Space]
        [Header("Status Bar Display")]
        public Color32 barColor;
        public Color32 circleColor;
        public Sprite icon;
        
        [Space] 
        public string localizationKey;
    }
}
