using System;
using UnityEngine;

namespace Map.Rooms
{
    public class RoomFiller : MonoBehaviour
    {
        [SerializeField] private GameObject battleInteractionPrefab;
        [SerializeField] private GameObject eliteBattleGroundMark;
        [SerializeField] private GameObject bossBattleGroundMark;
        [SerializeField] private GameObject dialogInteractionPrefab;
        
        public static RoomFiller instance;
        
        private void Awake()
        {
            instance = this;
        }

        public void FillRoom(RoomPackage roomPackage)
        {
            switch (roomPackage.roomType)
            {
                case RoomData.RoomType.Starting:
                    SetupStartingRoom();
                    break;
                case RoomData.RoomType.Battle:
                    SetupBattleRoom(roomPackage);
                    break;
                case RoomData.RoomType.Encounter:
                    SetupSpecialRoom(roomPackage);
                    break;
                case RoomData.RoomType.Elite:
                    SetupEliteRoom(roomPackage);
                    break;
                case RoomData.RoomType.Boss:
                    SetupBossRoom(roomPackage);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetupStartingRoom()
        {
            
        }
        
        private void SetupBattleRoom(RoomPackage roomPackage)
        {
            if (roomPackage.pointOfInterests.Count < 1)
                roomPackage.AddPointOfInterest(battleInteractionPrefab, ComputeNewPosition(), true);
            
            SpawnRoomContent(roomPackage);
        }

        private void SetupSpecialRoom(RoomPackage roomPackage)
        {
            if (roomPackage.pointOfInterests.Count < 1)
            {
                roomPackage.AddPointOfInterest(dialogInteractionPrefab, ComputeNewPosition(), true);
            }
            
            SpawnRoomContent(roomPackage);
        }
        
        private void SetupEliteRoom(RoomPackage roomPackage)
        {
            if (roomPackage.pointOfInterests.Count < 1)
            {
                roomPackage.AddPointOfInterest(eliteBattleGroundMark, Vector3.zero, false);
                roomPackage.AddPointOfInterest(battleInteractionPrefab, ComputeNewPosition(), true);
            }
            
            SpawnRoomContent(roomPackage);
        }
        
        private void SetupBossRoom(RoomPackage roomPackage)
        {
            if (roomPackage.pointOfInterests.Count < 1)
            {
                roomPackage.AddPointOfInterest(bossBattleGroundMark, Vector3.zero, false);
                roomPackage.AddPointOfInterest(battleInteractionPrefab, ComputeNewPosition(), true);
            }
            
            SpawnRoomContent(roomPackage);
        }

        private void SpawnRoomContent(RoomPackage roomPackage)
        {
            foreach (PointOfInterest item in roomPackage.pointOfInterests)
            {
                if (roomPackage.hasBeenCleared && item.removeOnCleared)
                    continue;
                
                Instantiate(item.prefab, item.position, Quaternion.identity);
            }
        }

        private Vector3 ComputeNewPosition()
        {
            if (RoomPointsOfInterest.instance != null)
                return RoomPointsOfInterest.instance.GetPointOfInterestPosition();
            else
                return Vector3.zero;
        }
    }
}
