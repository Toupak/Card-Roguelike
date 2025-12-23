using Run_Loop;
using UnityEngine;

namespace MapMaker.Rooms
{
    public class RoomFiller : MonoBehaviour
    {
        private RoomBuilder roomBuilder;
        
        private void Start()
        {
            roomBuilder = GetComponent<RoomBuilder>();
            SceneLoader.OnLoadRoom.AddListener(FillRoom);
        }

        private void FillRoom()
        {
            RoomData.RoomType roomType = roomBuilder.GetCurrentRoomType();
            
            Debug.Log($"Enter room of type : {roomType}");
        }
    }
}
