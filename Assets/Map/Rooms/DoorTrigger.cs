using Run_Loop;
using UnityEngine;

namespace Map.Rooms
{
    public class DoorTrigger : MonoBehaviour
    {
        [SerializeField] private RoomData.DoorDirection doorDirection;

        public RoomData.DoorDirection DoorDirection => doorDirection;

        private SpriteRenderer spriteRenderer;
        
        private bool isLocked;

        private void Start()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            if (!isLocked && other.CompareTag("Character"))
                TriggerDoor();
        }

        private void TriggerDoor()
        {
            RunLoop.instance.OnTriggerDoor(doorDirection);
        }

        public void LockDoor()
        {
            isLocked = true;
            spriteRenderer.color = Color.red;
        }

        public void UnlockDoor()
        {
            isLocked = false;
            spriteRenderer.color = Color.green;
        }
    }
}
