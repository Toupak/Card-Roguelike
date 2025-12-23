using System;
using Run_Loop;
using UnityEngine;

namespace MapMaker.Rooms
{
    public class RoomFiller : MonoBehaviour
    {
        [SerializeField] private GameObject battleInteractionPrefab;
        [SerializeField] private GameObject dialogInteractionPrefab;
        
        private RoomBuilder roomBuilder;
        
        private void Start()
        {
            roomBuilder = GetComponent<RoomBuilder>();
            SceneLoader.OnLoadRoom.AddListener(FillRoom);
        }

        private void FillRoom()
        {
            RoomData.RoomType roomType = roomBuilder.GetCurrentRoomType();

            switch (roomType)
            {
                case RoomData.RoomType.Starting:
                    SetupStartingRoom();
                    break;
                case RoomData.RoomType.Battle:
                    SetupBattleRoom();
                    break;
                case RoomData.RoomType.Special:
                    SetupSpecialRoom();
                    break;
                case RoomData.RoomType.Boss:
                    SetupBossRoom();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetupStartingRoom()
        {
            
        }
        
        private void SetupBattleRoom()
        {
            Instantiate(battleInteractionPrefab, Vector3.zero, Quaternion.identity);
        }
        
        private void SetupSpecialRoom()
        {
            Instantiate(dialogInteractionPrefab, Vector3.zero, Quaternion.identity);
        }
        
        private void SetupBossRoom()
        {
            
        }
    }
}
