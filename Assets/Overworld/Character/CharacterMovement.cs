using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    float speed;
    float accelerationSpeed;
    float decelerationSpeed;

    private Rigidbody2D rb;

    public Vector2 lastMovement { get; private set; }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        
    }

    private void MovePlayer(Vector2 direction, float speed)
    {

    }

    private void StopMovement()
    {

    }

    private void Move()
    {

    }
}
