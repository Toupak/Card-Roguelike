using PrimeTween;
using TMPro;
using UnityEngine;

namespace Run_Loop.Run_Parameters
{
    public class OptionDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        [Space]
        [SerializeField] private int defaultValue;

        public int currentValue { get; private set; }
        
        private void Start()
        {
            currentValue = defaultValue;
            UpdateValueDisplay();
        }

        public void IncreaseValue()
        {
            currentValue += 1;
            UpdateValueDisplay();
        }

        public void DecreaseValue()
        {
            bool isAlreadyAtZero = currentValue == 0;
            currentValue = Mathf.Max(0, currentValue - 1);
            UpdateValueDisplay(isAlreadyAtZero);
        }

        private void UpdateValueDisplay(bool isNegativeEffect = false)
        {
            text.text = currentValue.ToString();

            if (isNegativeEffect)
                Tween.ShakeLocalPosition(text.transform, Vector3.right * 10.0f, 0.1f);
            else
                Tween.PunchScale(text.transform, Vector3.down, 0.1f);
        }
    }
}
