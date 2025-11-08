using Cards.Scripts;
using UnityEngine;

namespace Spells.Passives
{
    public class PassiveHolder : MonoBehaviour
    {
        [SerializeField] private PassiveDisplay passivePrefab;

        private CardController cardController;
        
        public void Setup(CardController controller, CardData cardData)
        {
            cardController = controller;
            
            foreach (PassiveData data in cardData.passiveList)
            {
                SpawnPassiveObject(data);
            }
        }

        private void SpawnPassiveObject(PassiveData data)
        {
            Instantiate(passivePrefab, transform).Setup(cardController, data);
        }
    }
}
