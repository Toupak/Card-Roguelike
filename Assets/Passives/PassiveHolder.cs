using System.Collections.Generic;
using System.Linq;
using Cards.Scripts;
using UnityEngine;

namespace Passives
{
    public class PassiveHolder : MonoBehaviour
    {
        [SerializeField] private PassiveDisplay passivePrefab;

        private CardController cardController;

        public List<PassiveController> passives { get; private set; } = new List<PassiveController>();

        public void Setup(CardController controller, CardData cardData)
        {
            cardController = controller;
            
            foreach (PassiveData data in cardData.passiveList)
            {
                SpawnPassiveObject(data);
            }
        }

        public void AddPassive(PassiveData data)
        {
            SpawnPassiveObject(data);
        }

        public void RemovePassive(PassiveData data)
        {
            int passiveIndex = -1;
            for (int i = 0; i < passives.Count; i++)
            {
                if (passives[i].passiveData.passiveName == data.passiveName)
                {
                    passiveIndex = i;
                    break;
                }
            }

            if (passiveIndex >= 0)
            {
                Destroy(passives[passiveIndex].transform.parent.gameObject);
                passives.RemoveAt(passiveIndex);
            }
        }
        
        private void SpawnPassiveObject(PassiveData data)
        {
            PassiveDisplay passive = Instantiate(passivePrefab, transform);
            passives.Add(passive.Setup(cardController, data));
        }

        public PassiveController GetPassive(PassiveData data)
        {
            foreach (PassiveController passiveController in passives)
            {
                if (passiveController != null && passiveController.passiveData.passiveName == data.passiveName)
                    return passiveController;
            }

            return null;
        }
    }
}
