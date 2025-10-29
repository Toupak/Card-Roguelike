using UnityEngine;

namespace Cards.Scripts
{
    public class DisplayCardEffects : MonoBehaviour
    {
        [SerializeField] private GameObject targetEffect;

        private bool isTargeted;

        private void Start()
        {
            targetEffect.SetActive(false);
        }

        public void SetTargetState(bool state)
        {
            isTargeted = state;
            targetEffect.SetActive(isTargeted);
        }
    }
}
