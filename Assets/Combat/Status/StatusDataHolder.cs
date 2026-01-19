using System.Collections.Generic;
using Combat.Status.Data;
using UnityEngine;

namespace Combat.Status
{
    [CreateAssetMenu(fileName = "StatusDataHolder", menuName = "Scriptable Objects/StatusDataHolder")]
    public class StatusDataHolder : ScriptableObject
    {
        public List<StatusData> data;
    }
}
