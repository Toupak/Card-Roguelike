using BoomLib.Tools;
using Overworld.Character;
using UnityEngine;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] private CharacterMovement movement;
    [SerializeField] private Rigidbody2D rb;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void LateUpdate()
    {
        if (rb.linearVelocity.magnitude > 0.1f)
            PlayAnimation("Walk");
        else
            PlayAnimation("Idle");
    }

    private void PlayAnimation(string animation)
    {
        animator.Play($"{animation}_{GetLastDirection()}");
    }

    private string GetLastDirection()
    {
        string[] directions = { "Right", "Up", "Left", "Down" };

        int direction = Tools.GetCardinalDirection(movement.lastMovement);

        return directions[direction];
    }
}
