using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float accelerationSpeed;
    [SerializeField] private float decelerationSpeed;

    private Rigidbody2D rb;

    public Vector2 lastMovement { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        Walk();
    }

    private void MovePlayer(Vector2 direction, float speed)
    {
        if (direction.magnitude > 0.01f)
            lastMovement = (direction * speed).normalized;

        rb.linearVelocity = direction * speed;
    }

    private void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
    }

    private void Walk()
    {
        Vector2 inputDirection = PlayerInput.ComputeMoveDirection();

        MovePlayer(inputDirection, speed);
    }
}
