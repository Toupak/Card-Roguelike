using UnityEngine;

namespace Character_Selection.Character
{
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] private float speed;
        //[SerializeField] private float accelerationSpeed;
        //[SerializeField] private float decelerationSpeed;

        private Rigidbody2D rb;

        public Vector2 lastMovement { get; private set; } = Vector2.right;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!CharacterSingleton.instance.IsLocked)
                Walk();
        }
    
        private void Walk()
        {
            Vector2 inputDirection = PlayerInput.ComputeMoveDirection();

            MovePlayer(inputDirection, speed);
        }

        private void MovePlayer(Vector2 direction, float movementSpeed)
        {
            if (direction.magnitude > 0.01f)
                lastMovement = (direction * movementSpeed).normalized;

            rb.linearVelocity = direction * movementSpeed;
        }

        private void StopMovement()
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}
