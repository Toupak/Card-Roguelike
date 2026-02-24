using BoomLib.Dialog_System;
using Character_Selection.Character.Dialog;
using Map.Rooms;
using Run_Loop;
using UnityEngine;

namespace Map.Encounters.VendingMachine
{
    public class VendingMachineEncounter : DialogStarter
    {
        private bool isUsed;
        
        protected override void Start()
        {
            base.Start();
            
            isUsed = RoomBuilder.instance.HasRoomBeenCleared();
            if (isUsed)
                GetComponent<Animator>().Play("Used");
        }
        
        public override bool CanInteract()
        {
            return !isUsed;
        }
        
        protected override void StartDialog()
        {
            DialogManager.instance.StartDialog(dialogData.DialogTexts, dialogPosition, () =>
            {
                RoomBuilder.instance.MarkCurrentRoomAsCleared();
                RunLoop.instance.StartOpeningReward();
            });
        }
    }
}
