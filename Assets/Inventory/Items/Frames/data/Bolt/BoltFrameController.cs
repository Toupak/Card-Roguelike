using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;

namespace Inventory.Items.Frames.data.Bolt
{
    public class BoltFrameController : FrameController
    {
        public override void Setup(CardController controller, FrameData data)
        {
            base.Setup(controller, data);
            if (cardController.singleButton.spellController != null)
                cardController.singleButton.spellController.OnCastSpell.AddListener(() => CastSpellReaction(1));
            if (cardController.leftButton.spellController != null)
                cardController.leftButton.spellController.OnCastSpell.AddListener(() => CastSpellReaction(2));
            if (cardController.rightButton.spellController != null)
                cardController.rightButton.spellController.OnCastSpell.AddListener(() => CastSpellReaction(3));
        }

        public override void Remove()
        {
            if (cardController.singleButton.spellController != null)
                cardController.singleButton.spellController.OnCastSpell.RemoveListener(() => CastSpellReaction(1));
            if (cardController.leftButton.spellController != null)
                cardController.leftButton.spellController.OnCastSpell.RemoveListener(() => CastSpellReaction(2));
            if (cardController.rightButton.spellController != null)
                cardController.rightButton.spellController.OnCastSpell.RemoveListener(() => CastSpellReaction(3));
            base.Remove();
        }

        private void CastSpellReaction(int spellIndex)
        {
            if (spellIndex == 1 && cardController.singleButton.spellController != null && cardController.singleButton.spellController.spellData.energyCost > 0 && cardController.singleButton.spellController.HasCastedThisTurn)
                cardController.singleButton.spellController.RefreshCooldown();
            
            if (spellIndex == 2 && cardController.leftButton.spellController != null && cardController.leftButton.spellController.spellData.energyCost > 0 && cardController.leftButton.spellController.HasCastedThisTurn)
                cardController.leftButton.spellController.RefreshCooldown();
            
            if (spellIndex == 3 && cardController.rightButton.spellController != null && cardController.rightButton.spellController.spellData.energyCost > 0 && cardController.rightButton.spellController.HasCastedThisTurn)
                cardController.rightButton.spellController.RefreshCooldown();
        }
    }
}
