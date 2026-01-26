using BoomLib.BoomTween;
using TMPro;
using UnityEngine;

namespace Cards.Scripts
{
    public class CardHealthDisplay : MonoBehaviour
    {
        [SerializeField] private CardHealth cardHealth;
        [SerializeField] private TextMeshProUGUI healthText;

        private void OnEnable()
        {
            cardHealth.OnUpdateHP.AddListener(UpdateUI);
        }

        private void OnDisable()
        {
            cardHealth.OnUpdateHP.RemoveListener(UpdateUI);
        }

        private void UpdateUI(int currentHealth)
        {
            healthText.text = currentHealth.ToString();
            StartCoroutine(BTween.Squeeze(transform));
        }

        public void SetDisplayState(bool state, int currentHealth = 0)
        {
            gameObject.SetActive(state);
            if (state)
                UpdateUI(currentHealth);
        }
    }
}
