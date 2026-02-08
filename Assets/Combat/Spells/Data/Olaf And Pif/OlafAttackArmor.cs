using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Spells.Targeting;
using UnityEngine;

namespace Combat.Spells.Data.Olaf_And_Pif
{
    public class OlafAttackArmor : SpellController
    {
        [SerializeField] private CardData pifData;
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            CardMovement pif = TargetingSystem.instance.RetrieveCard(pifData);
            
            foreach (CardMovement target in targets)
            {
                DealDamageGA dealDamageGa = new DealDamageGA(spellData.damage, cardController, target.cardController);
                ActionSystem.instance.Perform(dealDamageGa);
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                if (target.cardController.cardStatus.IsStatusApplied(StatusType.Vengeance))
                {
                    int stackCount = target.cardController.cardStatus.GetCurrentStackCount(StatusType.Vengeance);
                    
                    ConsumeStacksGa consumeStacksGa = new ConsumeStacksGa(StatusType.Vengeance, stackCount, cardController, target.cardController);
                    ActionSystem.instance.Perform(consumeStacksGa);
                    yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                    ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Armor, stackCount, cardController, cardController);
                    ActionSystem.instance.Perform(applyStatusGa);
                    yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

                    if (pif != null)
                    {
                        ApplyStatusGa applyStatusPifGa = new ApplyStatusGa(StatusType.Armor, stackCount, cardController, pif.cardController);
                        ActionSystem.instance.Perform(applyStatusPifGa);
                        yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                    }
                }
            }
        }
    }
}
