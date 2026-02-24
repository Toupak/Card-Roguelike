using Character_Selection.Character;
using Map;

namespace Run_Loop
{
    public class StartBattleInteraction : Interactable
    {
        public override void ExecuteInteract(CharacterInteract characterInteract)
        {
            base.ExecuteInteract(characterInteract);
            
            RunLoop.instance.StartBattle();
        }
    }
}
