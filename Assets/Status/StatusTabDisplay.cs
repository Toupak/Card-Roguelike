using PrimeTween;
using Status.Data;
using TMPro;
using Tooltip;
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

        private DisplayTooltipOnHover displayTooltipOnHover;

        public void Setup(StatusData data, int stackCount)
        {
            stackCountText.text = stackCount.ToString();
            lineImage.color = data.barColor;
            circleImage.color = data.circleColor;

            SetupTooltip(data);
            
            Vector2 position = tabRectTransform.anchoredPosition;
            tabRectTransform.anchoredPosition = new Vector2(position.x - 50.0f, position.y);
            Sequence.Create()
                .Chain(Tween.UIAnchoredPositionX(tabRectTransform, position.x, 0.1f))
                .Chain(Tween.PunchScale(tabRectTransform, Vector3.one * 0.5f, 0.1f));
        }

        private void SetupTooltip(StatusData data)
        {
            displayTooltipOnHover = GetComponent<DisplayTooltipOnHover>();
            displayTooltipOnHover.SetupPassiveTooltip(data.statusName, data.statusDescription, data.icon);
        }

        public void UpdateStackCount(int stackCount)
        {
            stackCountText.text = stackCount.ToString();
            Tween.PunchScale(tabRectTransform, Vector3.one * 0.5f, 0.1f);
        }

        public void Remove()
        {
            Sequence.Create()
                .Chain(Tween.PunchScale(tabRectTransform, Vector3.one * 0.5f, 0.1f))
                .Chain(Tween.UIAnchoredPositionX(tabRectTransform, tabRectTransform.anchoredPosition.x - 50.0f, 0.1f))
                .ChainCallback(() => Destroy(gameObject));
        }
    }
}
