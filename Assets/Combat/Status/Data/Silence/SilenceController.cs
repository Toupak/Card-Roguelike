using BoomLib.Tools;
using Cards.Scripts;
using Combat.Spells;

namespace Combat.Status.Data.Silence
{
    public class SilenceController : StatusController
    {
        public SpellController targetSpell { get; private set; } = null;
        
        public override void Setup(CardController controller, StatusData data)
        {
            base.Setup(controller, data);
            LockRandomSpell();
        }

        private void LockRandomSpell()
        {
            targetSpell = SelectRandomSpell();
            
            if (targetSpell != null)
                return;

            targetSpell.SetSilenceState(true);
        }

        private SpellController SelectRandomSpell()
        {
            if (cardController.singleButton.spellController != null)
                return cardController.singleButton.spellController;
            
            if (cardController.leftButton.spellController != null && Tools.RandomBool())
                return cardController.leftButton.spellController;
            
            if (cardController.rightButton.spellController != null)
                return cardController.rightButton.spellController;

            return null;
        }

        public override void Remove()
        {
            if (targetSpell != null)
                targetSpell.SetSilenceState(false);
            base.Remove();
        }
    }
}
