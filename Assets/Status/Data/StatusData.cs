using Cards.Scripts;
using UnityEngine;

namespace Status.Data
{
    [CreateAssetMenu(fileName = "StatusData", menuName = "Scriptable Objects/StatusData")]
    public class StatusData : ScriptableObject
    {
        public StatusType type;
        public string statusName;
        public string statusTag;
        [TextArea] public string statusDescription;
        public StatusEndTurnBehaviour endTurnBehaviour;
        public int maxStackCount = -1;
        
        [Space]
        [Header("Status Bar Display")]
        public Color32 barColor;
        public Color32 circleColor;
        public Sprite icon;
        
        [Space] 
        public string localizationKey;
    }
}
