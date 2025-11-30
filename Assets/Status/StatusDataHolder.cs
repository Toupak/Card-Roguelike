using System.Collections.Generic;
using Status.Data;
using UnityEngine;

namespace Status
{
    [CreateAssetMenu(fileName = "StatusDataHolder", menuName = "Scriptable Objects/StatusDataHolder")]
    public class StatusDataHolder : ScriptableObject
    {
        public List<StatusData> data;
    }
}
