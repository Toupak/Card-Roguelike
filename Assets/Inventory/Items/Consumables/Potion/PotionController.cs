using System;
using System.Collections;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Inventory.Items.Consumables.Potion
{
    public class PotionController : ConsumableController
    {
        [SerializeField] private int healthRestored;
        
        protected override IEnumerator UseConsumableCoroutine(CardMovement target, Action callback = null)
        {
            if (ActionSystem.instance != null)
            {
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                HealGa healGa = new HealGa(healthRestored, null, target.cardController);
                ActionSystem.instance.Perform(healGa);
                
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            }

            if (callback != null)
                callback?.Invoke();
        }
    }
}
