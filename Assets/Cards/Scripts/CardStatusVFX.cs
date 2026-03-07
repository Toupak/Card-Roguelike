using System;
using UnityEngine;
using UnityEngine.UI;

namespace Cards.Scripts
{
    public class CardStatusVFX : MonoBehaviour
    {
        [SerializeField] private Image stunEffect;
        
        private CardController cardController;

        private void Start()
        {
            cardController = GetComponentInParent<CardController>();
            
            stunEffect.gameObject.SetActive(false);
            cardController.cardStatus.OnUpdateStatus.AddListener(UpdateEffect);
        }

        private void UpdateEffect(StatusType type, StatusTabModification statusTabModification)
        {
            switch (type)
            {
                case StatusType.None:
                    break;
                case StatusType.Stun:
                    stunEffect.gameObject.SetActive(cardController.cardStatus.IsStatusApplied(StatusType.Stun));
                    break;
                default:
                    break;
            }
        }
    }
}
