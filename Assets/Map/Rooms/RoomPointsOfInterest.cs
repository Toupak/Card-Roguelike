using System.Collections.Generic;
using UnityEngine;

namespace Map.Rooms
{
    public class RoomPointsOfInterest : MonoBehaviour
    {
        [SerializeField] public List<Transform> pointsOfInterest;
        
        public static RoomPointsOfInterest instance;
        
        private void Awake()
        {
            instance = this;
        }

        public Vector3 GetPointOfInterestPosition()
        {
            return pointsOfInterest[Random.Range(0, pointsOfInterest.Count)].position;
        }
    }
}
