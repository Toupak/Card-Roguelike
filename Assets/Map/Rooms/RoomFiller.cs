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

        public void FillRoom(RoomData.RoomType roomType, bool hasRoomBeenCleared)
        {
            switch (roomType)
            {
                case RoomData.RoomType.Starting:
                    SetupStartingRoom();
                    break;
                case RoomData.RoomType.Battle:
                    SetupBattleRoom(hasRoomBeenCleared);
                    break;
                case RoomData.RoomType.Encounter:
                    SetupSpecialRoom();
                    break;
                case RoomData.RoomType.Elite:
                    SetupEliteRoom(hasRoomBeenCleared);
                    break;
                case RoomData.RoomType.Boss:
                    SetupBossRoom(hasRoomBeenCleared);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetupStartingRoom()
        {
            
        }
        
        private void SetupBattleRoom(bool hasRoomBeenCleared)
        {
            SpawnBattleInteraction(hasRoomBeenCleared);
        }
        
        private void SetupSpecialRoom()
        {
            Instantiate(dialogInteractionPrefab, Vector3.zero, Quaternion.identity);
        }
        
        private void SetupEliteRoom(bool hasRoomBeenCleared)
        {
            SpawnBattleInteraction(hasRoomBeenCleared);
            Instantiate(eliteBattleGroundMark, Vector3.zero, Quaternion.identity);
        }
        
        private void SetupBossRoom(bool hasRoomBeenCleared)
        {
            SpawnBattleInteraction(hasRoomBeenCleared);
            Instantiate(bossBattleGroundMark, Vector3.zero, Quaternion.identity);
        }

        private void SpawnBattleInteraction(bool hasRoomBeenCleared)
        {
            if (!hasRoomBeenCleared)
                Instantiate(battleInteractionPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}
