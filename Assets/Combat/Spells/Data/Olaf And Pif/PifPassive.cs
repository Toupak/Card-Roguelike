using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;
using UnityEngine;

namespace Combat.Spells.Data.Olaf_And_Pif
{
    public class PifPassive : PassiveController
    {
        [SerializeField] private CardData olafData;
        
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DealDamageGA>(DealDamageReaction, ReactionTiming.POST);
        }

        private void DealDamageReaction(DealDamageGA dealDamageGa)
        {
            if (dealDamageGa.attacker.cardData.name == olafData.name)
            {
                foreach (DealDamageGA.DamagePackage package in dealDamageGa.packages)
                {
                    if (package.amount > 0)
                    {
                        ApplyStatusGa applyStatusGa = new ApplyStatusGa(StatusType.Vengeance, 1, cardController, package.target);
                        ActionSystem.instance.AddReaction(applyStatusGa);
                    }
                }
            }
        }
    }
}
