using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using UnityEngine;

namespace Combat.Spells.Data.Xordan
{
    public class XordanShoot : SpellController
    {
        [SerializeField] private SpellData reloadSpellData;
        
        public override bool CanCastSpell()
        {
            return base.CanCastSpell() && cardController.cardStatus.IsStatusApplied(StatusType.BulletAmmo);
        }
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            int ammoCount = cardController.rightButton.spellController.HasCastedThisTurn ? cardController.cardStatus.GetCurrentStackCount(StatusType.BulletAmmo) : 1;
            
            ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.BulletAmmo, ammoCount, cardController, cardController);
            ActionSystem.instance.Perform(consumeStacksGa);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            int damage = ComputeCurrentDamage(spellData.damage * ammoCount);
            
            foreach (CardMovement target in targets)
            {
                DealDamageGA dealDamageGa = new DealDamageGA(damage, cardController, target.cardController);
                ActionSystem.instance.Perform(dealDamageGa);
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            }
        }
        
        protected override void SubscribeReactions()
        {
            ActionSystem.SubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.PRE);
        }

        protected override  void UnsubscribeReactions()
        {
            ActionSystem.UnsubscribeReaction<StartTurnGa>(StartTurnReaction, ReactionTiming.PRE);
        }

        private void StartTurnReaction(StartTurnGa startTurnGa)
        {
            if (startTurnGa.starting == CombatLoop.TurnType.Player)
                cardController.SetupLeftSpell(reloadSpellData);
        }
    }
}
