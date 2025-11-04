using PrimeTween;
using TMPro;
using UnityEngine;

namespace UI.Damage_Numbers
{
    public class DamageNumberDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI damageText;
        
        public void Setup(Vector2 position, int damage, bool isPositive)
        {
            string sign = isPositive ? "+" : "-";
            damageText.text = $"{sign}{damage}";
            damageText.fontSize = 35.0f + 10.0f * damage;
            
            RectTransform rectTransform = GetComponent<RectTransform>();
            position += Random.insideUnitCircle * 30.0f;
            rectTransform.anchoredPosition = position;
            transform.localScale = new Vector3(0.0f, 0.0f, 1.0f);

            Sequence.Create()
                .Group(Tween.Scale(transform, Vector3.one, 0.1f))
                .Group(Tween.UIAnchoredPositionY(rectTransform, position.y - 40, 0.1f, Ease.OutBounce))
                .Chain(Tween.Scale(transform, Vector3.one * 1.3f, 0.1f, Ease.InOutBounce, 2, CycleMode.Yoyo))
                .Chain(Tween.UIAnchoredPositionY(rectTransform, position.y + 80, 1.0f, Ease.Linear))
                .Group(Tween.Color(damageText, Color.clear, 0.8f))
                .ChainCallback(() => Destroy(gameObject));
        }
    }
}
