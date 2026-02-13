using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Items.Frames
{
    [CreateAssetMenu(fileName = "FrameDatabase", menuName = "Scriptable Objects/FrameDatabase")]
    public class FrameDatabase : ScriptableObject
    {
        public List<FrameData> frames;
    }
}
