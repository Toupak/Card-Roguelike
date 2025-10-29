using UnityEngine;

namespace Cards.Scripts
{
    public class DisplayCardEffects : MonoBehaviour
    {
        [SerializeField] private GameObject targetEffect;
        [SerializeField] private GameObject targetableEffect;

        private bool isTargeted;
        private bool isTargetable;

        private void Start()
        {
            targetEffect.SetActive(false);
            targetableEffect.SetActive(false);
        }
        
        public void SetTargetState(bool state)
        {
            isTargeted = state;
            targetEffect.SetActive(isTargeted);
            
            if (isTargeted && isTargetable)
                SetPotentialTargetState(false);
        }

        public void SetPotentialTargetState(bool state)
        {
            isTargetable = state;
            targetableEffect.SetActive(isTargetable);
        }
    }
}
