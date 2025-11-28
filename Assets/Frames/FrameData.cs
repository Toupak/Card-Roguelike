using UnityEngine;

namespace Frames
{
    [CreateAssetMenu(fileName = "FrameData", menuName = "Scriptable Objects/FrameData")]
    public class FrameData : ScriptableObject
    {
        public string frameName;
        public string frameDescription;
        
        [Space] 
        public Material material;
        
        [Space] 
        public FrameController frameController;
        
        [Space] 
        public string localizationKey;
    }
}
