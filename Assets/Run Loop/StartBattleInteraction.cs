using Overworld;

namespace Run_Loop
{
    public class StartBattleInteraction : Interactable
    {
        public override void ExecuteInteract()
        {
            base.ExecuteInteract();
            
            RunLoop.instance.StartBattle();
        }
    }
}
