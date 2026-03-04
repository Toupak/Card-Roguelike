using System;
using System.Collections;
using Cards.Scripts;
using UnityEngine;

namespace Inventory.Items.Consumables
{
    public class ConsumableController : MonoBehaviour
    {
        protected ItemController itemController;
        protected ConsumableData data;
        
        public virtual void Setup(ItemController item, ConsumableData consumableData)
        {
            itemController = item;
            data = consumableData;
        }
        
        public virtual bool CanUseConsumable(CardMovement target)
        {
            return true;
        }

        public virtual void UseConsumable(CardMovement target, Action callback = null)
        {
            StartCoroutine(UseConsumableCoroutine(target, callback));
        }

        protected virtual IEnumerator UseConsumableCoroutine(CardMovement target, Action callback = null)
        {
            if (callback != null)
                callback?.Invoke();
            
            yield break;
        }
    }
}
