using UnityEngine;

namespace Inventory.Drop_Rates
{
    [CreateAssetMenu(fileName = "DropRateData", menuName = "Scriptable Objects/DropRateData")]
    public class DropRateData : ScriptableObject
    {
        public float startingValue;
        public float valueIncreaseOnFail;
        public float valueIncreaseOnEliteKill;
        public float valueIncreaseOnBossKill;
    }
}
