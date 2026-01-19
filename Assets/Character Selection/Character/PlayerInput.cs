using UnityEngine;
using UnityEngine.InputSystem;

namespace Overworld.Character
{
    public static class PlayerInput
    {
        public static Vector2 ComputeMoveDirection()
        {
            Vector2 inputDirection = Vector2.zero;

            if (Keyboard.current.wKey.isPressed || Keyboard.current.zKey.isPressed)
                inputDirection.y = 1.0f;

            if (Keyboard.current.sKey.isPressed)
                inputDirection.y = -1.0f;

            if (Keyboard.current.dKey.isPressed)
                inputDirection.x = 1.0f;

            if (Keyboard.current.aKey.isPressed || Keyboard.current.qKey.isPressed)
                inputDirection.x = -1.0f;

            if (Gamepad.current != null && Gamepad.current.leftStick.value.magnitude > 0.1f)
                return Gamepad.current.leftStick.value;

            return inputDirection.normalized;
        }

        public static bool GetInteractInput()
        {
            if (Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame)
                return true;
            else
                return Mouse.current.leftButton.wasPressedThisFrame;
        }
    }
}
