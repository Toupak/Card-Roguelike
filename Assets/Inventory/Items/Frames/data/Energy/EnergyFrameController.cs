using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;

namespace Inventory.Items.Frames.data.Energy
{
    public class EnergyFrameController : FrameController
    {
        public override void Setup(CardController controller, FrameData data)
        {
            base.Setup(controller, data);
            if (cardController.leftButton.spellController != null)
                cardController.leftButton.spellController.OnCastSpell.AddListener(CastSpellReaction);
            if (cardController.rightButton.spellController != null)
                cardController.rightButton.spellController.OnCastSpell.AddListener(CastSpellReaction);
        }

        public override void Remove()
        {
            if (cardController.leftButton.spellController != null)
                cardController.leftButton.spellController.OnCastSpell.RemoveListener(CastSpellReaction);
            if (cardController.rightButton.spellController != null)
                cardController.rightButton.spellController.OnCastSpell.RemoveListener(CastSpellReaction);
            base.Remove();
        }

        private void CastSpellReaction()
        {
            bool hasLeftCasted = cardController.leftButton.spellController != null && cardController.leftButton.spellController.HasCastedThisTurn;
            bool hasRightCasted = cardController.rightButton.spellController != null && cardController.rightButton.spellController.HasCastedThisTurn;

            if (hasLeftCasted && hasRightCasted)
            {
                GainEnergyGa gainEnergyGa = new GainEnergyGa(1, cardController);
                
                if (ActionSystem.instance.IsPerforming)
                    ActionSystem.instance.AddReaction(gainEnergyGa);
                else
                    ActionSystem.instance.Perform(gainEnergyGa);
            }
        }
    }
}
