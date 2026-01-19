using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;

namespace Combat.Spells.Data.Squire
{
    public class SquirePassive : PassiveController
    {
        private SpellController leftSpell => cardController.leftButton.spellController;
        private SpellController rightSpell => cardController.rightButton.spellController;
        
        public override void Setup(CardController controller, PassiveData data)
        {
            base.Setup(controller, data);
            
            if (leftSpell == null || rightSpell == null)
                return;
            
            leftSpell.OnCastSpell?.AddListener(CheckCooldown);
            rightSpell.OnCastSpell?.AddListener(CheckCooldown);
        }

        private void CheckCooldown()
        {
            if (leftSpell.HasCastedThisTurn && rightSpell.HasCastedThisTurn)
            {
                ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Stun, 1, cardController, cardController);
                
                if (ActionSystem.instance.IsPerforming)
                    ActionSystem.instance.AddReaction(applyStatusGa);
                else
                    ActionSystem.instance.Perform(applyStatusGa);
            }
        }
    }
}
