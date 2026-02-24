using System.Collections.Generic;
using BoomLib.Tools;
using Map.Rooms;
using UnityEngine;

namespace Map.MiniMap
{
    public class MinimapBuilder : MonoBehaviour
    {
        [SerializeField] private GameObject renderTextureImage;
        [SerializeField] private Transform cameraTransform;

        [Space] 
        [SerializeField] private MinimapRoomIcon roomPrefab;
        
        [Space]
        [SerializeField] private Vector2 cellSize;

        public static MinimapBuilder instance;

        private List<MinimapRoomIcon> roomIcons = new List<MinimapRoomIcon>();

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            RoomBuilder.OnBuildRooms.AddListener(BuildMinimap);
            renderTextureImage.SetActive(false);
        }
        
        public void UpdateMap()
        {
            RoomPackage currentRoom = RoomBuilder.instance.CurrentRoom;

            foreach (MinimapRoomIcon roomIcon in roomIcons)
            {
                if (roomIcon.roomPackage.isVisible)
                {
                    roomIcon.gameObject.SetActive(true);
                    
                    bool isCurrentRoom = currentRoom == roomIcon.roomPackage;
                    if (isCurrentRoom)
                        SetCameraPositionOnCurrentRoom(roomIcon);
                    
                    roomIcon.UpdateIcon(isCurrentRoom);
                }
            }
        }

        public void SetMinimapState(bool state)
        {
            renderTextureImage.SetActive(state);
        }

        private void BuildMinimap()
        {
            List<RoomPackage> rooms = RoomBuilder.instance.Rooms;
            RoomPackage currentRoom = RoomBuilder.instance.CurrentRoom;
            Vector2 offset = Vector2.one * (MapBuilder.instance.mapCenter * -1.0f);
            
            foreach (RoomPackage room in rooms)
            {
                bool isCurrentRoom = currentRoom == room;
                
                MinimapRoomIcon roomIcon = SpawnRoom(room.x, room.y, offset, room, isCurrentRoom);
                roomIcons.Add(roomIcon);
                
                roomIcon.gameObject.SetActive(room.isVisible);
                
                if (isCurrentRoom)
                    SetCameraPositionOnCurrentRoom(roomIcon);
            }
        }

        private MinimapRoomIcon SpawnRoom(int x, int y, Vector2 offset, RoomPackage roomPackage, bool isCurrentRoom)
        {
            Vector2 position = transform.position.ToVector2() + new Vector2(x * cellSize.x, y * cellSize.y) + new Vector2(offset.x * cellSize.x, offset.y * cellSize.y);

            MinimapRoomIcon roomIcon = Instantiate(roomPrefab, position, Quaternion.identity, transform);
            roomIcon.Setup(roomPackage, isCurrentRoom);

            return roomIcon;
        }
        
        
        private void SetCameraPositionOnCurrentRoom(MinimapRoomIcon roomIcon)
        {
            Vector3 position = roomIcon.transform.position;
            position.z = -10.0f;

            cameraTransform.transform.position = position;
        }
    }
}
