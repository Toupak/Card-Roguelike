using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Character_Selection.Character
{
    public class CharacterMovement : MonoBehaviour
    {
        public static UnityEvent<Vector2, bool> OnRightClickSetTarget = new UnityEvent<Vector2, bool>();

        [SerializeField] private float speed;
        //[SerializeField] private float accelerationSpeed;
        //[SerializeField] private float decelerationSpeed;

        private bool hasTarget;
        
        private Rigidbody2D rb;
        private Vector2 rightClickTargetMovement;
        public Vector2 lastMovement { get; private set; } = Vector2.right;

        private bool isLocked;

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (isLocked)
                return;

            if (PlayerInput.GetRightClickInput())
            {
                bool hasClicked = Mouse.current.rightButton.wasPressedThisFrame;

                rightClickTargetMovement = GetRightClickTarget();
                OnRightClickSetTarget.Invoke(rightClickTargetMovement, hasClicked);
                
                hasTarget = true;
            }

            if (HasReachedTarget())
                hasTarget = false;
        }

        private void FixedUpdate()
        {
            if (isLocked)
                return;

            if (hasTarget)
                MoveTowardsTarget();
            else
                Walk();
        }

        private void MoveTowardsTarget()
        {
            Vector2 inputDirection = (rightClickTargetMovement - (Vector2)transform.position).normalized;

            MovePlayer(inputDirection, speed);
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

        public void SetLockState(bool state)
        {
            isLocked = state;
            StopMovement();
        }

        private bool HasReachedTarget()
        {
            return hasTarget && Vector2.Distance(rightClickTargetMovement, (Vector2)transform.position) < 0.1f;
        }

        public Vector2 GetRightClickTarget()
        {
            return ScreenToWorld2D(Mouse.current.position.ReadValue(), Camera.main);
        }

        private Vector2 ScreenToWorld2D(Vector2 screenPos, Camera cam)
        {
            float z = -cam.transform.position.z;
            Vector3 w = cam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, z));
            return (Vector2)w;
        }
    }
}
