using System.Collections.Generic;
using UnityEngine;

namespace Map.Rooms
{
    public class RoomPointsOfInterest : MonoBehaviour
    {
        [SerializeField] private List<Transform> pointsOfInterest;
        [SerializeField] private List<Transform> playerStartingPositions;
        
        public static RoomPointsOfInterest instance;
        
        private void Awake()
        {
            instance = this;
        }

        public Vector3 GetPointOfInterestPosition()
        {
            return pointsOfInterest[Random.Range(0, pointsOfInterest.Count)].position;
        }
        
        public Vector3 GetPlayerStartingPosition()
        {
            return playerStartingPositions[Random.Range(0, playerStartingPositions.Count)].position;
        }
    }
}
