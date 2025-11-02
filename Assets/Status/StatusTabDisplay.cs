using Status.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Status
{
    public class StatusTabDisplay : MonoBehaviour
    {
        [SerializeField] private RectTransform tabRectTransform;
        [SerializeField] private TextMeshProUGUI stackCountText;
        [SerializeField] private Image lineImage;
        [SerializeField] private Image circleImage;

        public void Setup(StatusData data, int stackCount)
        {
            stackCountText.text = stackCount.ToString();
            lineImage.color = data.barColor;
            circleImage.color = data.circleColor;
        }

        public void UpdateStackCount(int stackCount)
        {
            stackCountText.text = stackCount.ToString();
        }

        public void Remove()
        {
            Destroy(gameObject);
        }
    }
}
