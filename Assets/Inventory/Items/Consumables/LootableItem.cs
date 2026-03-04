using BoomLib.Dialog_System;
using Character_Selection.Character.Dialog;
using PrimeTween;
using Run_Loop;
using UnityEngine;

namespace Inventory.Items.Consumables
{
    public class LootableItem : DialogStarter
    {
        [SerializeField] private ConsumableData consumableData;
        [SerializeField] private int amount;

        private bool hasBeenLooted;
        
        public override bool CanInteract()
        {
            return !hasBeenLooted;
        }
        
        protected override void StartDialog()
        {
            DialogManager.instance.StartDialog(dialogData.DialogTexts, dialogPosition, () =>
            {
                hasBeenLooted = true;
                PlayerInventory.instance.LootConsumable(consumableData, amount);
                DestroyConsumable();
            });
        }

        private void DestroyConsumable()
        {
            Sequence.Create()
                .Chain(Tween.PunchScale(transform, Vector3.one * -0.3f, 0.3f))
                .ChainCallback(() => Destroy(gameObject));
        }
    }
}
