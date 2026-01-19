using System;
using UnityEngine;

namespace MapMaker.Rooms
{
    public class RoomFiller : MonoBehaviour
    {
        [SerializeField] private GameObject battleInteractionPrefab;
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
                case RoomData.RoomType.Special:
                    SetupSpecialRoom();
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
            if (!hasRoomBeenCleared)
                Instantiate(battleInteractionPrefab, Vector3.zero, Quaternion.identity);
        }
        
        private void SetupSpecialRoom()
        {
            Instantiate(dialogInteractionPrefab, Vector3.zero, Quaternion.identity);
        }
        
        private void SetupBossRoom(bool hasRoomBeenCleared)
        {
            
        }
    }
}
