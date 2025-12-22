using Run_Loop;
using UnityEngine;

namespace MapMaker.Rooms
{
    public class DoorTrigger : MonoBehaviour
    {
        [SerializeField] private RoomData.DoorDirection doorDirection;
        
        private bool hasBeenTriggered;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Character"))
                TriggerDoor();
        }

        private void TriggerDoor()
        {
            if (hasBeenTriggered)
                return;
            
            hasBeenTriggered = true;

            RunLoop.instance.OnTriggerDoor(doorDirection);
        }
    }
}
