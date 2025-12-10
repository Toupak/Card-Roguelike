using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
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

        if (Gamepad.current != null)
        {
            Vector2 joystickInput = Gamepad.current.leftStick.value;

            if (joystickInput.magnitude < 0.1f)
                joystickInput = Vector2.zero;

            return joystickInput;
        }

        return inputDirection.normalized;
    }
}
