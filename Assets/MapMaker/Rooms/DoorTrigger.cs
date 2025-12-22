using Run_Loop;
using UnityEngine;

namespace MapMaker.Rooms
{
    public class DoorTrigger : MonoBehaviour
    {
        [SerializeField] private RoomData.DoorDirection doorDirection;

        public RoomData.DoorDirection DoorDirection => doorDirection;
        
        private void OnTriggerStay2D(Collider2D other)
        {
            if (other.CompareTag("Character"))
                TriggerDoor();
        }

        private void TriggerDoor()
        {
            RunLoop.instance.OnTriggerDoor(doorDirection);
        }
    }
}
