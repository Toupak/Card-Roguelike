namespace Run_Loop
{
    public class StartRunInteraction : Interactable
    {
        public override void ExecuteInteract()
        {
            base.ExecuteInteract();
            
            RunLoop.instance.StartRun();
        }
    }
}
