using BoomLib.Dialog_System;
using Character_Selection.Character.Dialog;
using Run_Loop;

namespace Map.Decor.Reward_Interact
{
    public class RewardInteract : DialogStarter
    {
        protected override void StartDialog()
        {
            DialogManager.instance.StartDialog(dialogData.DialogTexts, dialogPosition, () => RunLoop.instance.StartOpeningReward());
        }
    }
}
