using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace BoomLib.Pause_Menu.Scripts
{
    public class ScreenShakeController : MonoBehaviour
    {
        [SerializeField] private Toggle toggle;

        public static UnityEvent<bool> OnSetScreenShakeState = new UnityEvent<bool>();

        private bool isScreenShakeActivated = true;
        public bool IsScreenShakeActivated => isScreenShakeActivated;

        private void Start()
        {
            toggle.onValueChanged.AddListener(OnSwitchToggle);
        }

        private void OnSwitchToggle(bool toggleValue)
        {
            isScreenShakeActivated = toggleValue;
            OnSetScreenShakeState?.Invoke(isScreenShakeActivated);
        }
    }
}
