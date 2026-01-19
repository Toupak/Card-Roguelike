using Cards.Scripts;
using Passives;

namespace Spells.Data.Faces
{
    public class WhiteFacePassive : PassiveController
    {
        public override void Setup(CardController controller, PassiveData data)
        {
            base.Setup(controller, data);

            if (cardController.leftButton.spellController != null)
                cardController.leftButton.spellController.OnCastSpell?.AddListener(() => cardController.leftButton.spellController.RefreshCooldown());
            
            if (cardController.rightButton.spellController != null)
                cardController.rightButton.spellController.OnCastSpell?.AddListener(() => cardController.rightButton.spellController.RefreshCooldown());
            
            if (cardController.singleButton.spellController != null)
                cardController.singleButton.spellController.OnCastSpell?.AddListener(() => cardController.rightButton.spellController.RefreshCooldown());
        }

        private void OnDisable()
        {
            if (cardController.leftButton.spellController != null)
                cardController.leftButton.spellController.OnCastSpell?.RemoveListener(() => cardController.leftButton.spellController.RefreshCooldown());
            
            if (cardController.rightButton.spellController != null)
                cardController.rightButton.spellController.OnCastSpell?.RemoveListener(() => cardController.rightButton.spellController.RefreshCooldown());
            
            if (cardController.singleButton.spellController != null)
                cardController.singleButton.spellController.OnCastSpell?.RemoveListener(() => cardController.rightButton.spellController.RefreshCooldown());
        }
    }
}
