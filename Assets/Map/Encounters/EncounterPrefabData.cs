using System;
using UnityEngine;

namespace Map.Encounters
{
    [Serializable]
    public class EncounterPrefabData
    {
        public GameObject prefab;
        public Sprite minimapIcon;
        public int minAmountPerFloor;
        public int maxAmountPerFloor;
    }
}
