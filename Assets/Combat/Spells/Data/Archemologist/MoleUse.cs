using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Card_Container.CardSlot;
using Combat.Spells.Targeting;
using UnityEngine;

namespace Combat.Spells.Data.Archemologist
{
    public class MoleUse : SpellController
    {
        [SerializeField] private List<CardData> artefacts;

        private enum Artefacts
        {
            None,
            Cookie,
            Bullet,
            Potion,
            Helm,
            Grenade,
            HolyGrenade,
            PoisonCookie,
            Fireball,
            Pepper
        }
        
        private Artefacts currentArtefact;
        private CardController currentArtefactController;
        
        public override bool CanCastSpell()
        {
            return base.CanCastSpell() && cardController.cardMovement.tokenContainer.slotCount > 0;
        }
        
        protected override IEnumerator SelectTargetAndCast(Transform startPosition)
        {
            currentArtefact = Artefacts.None;
            currentArtefactController = null;
            yield return TargetingSystem.instance.SelectTargets(cardController.cardMovement, startPosition, spellData.targetType, spellData.targetingMode, 1, spellData.targetTokens, Validator);
            if (TargetingSystem.instance.IsCanceled)
                CancelTargeting();
            else
                yield return SelectArtefactTarget(TargetingSystem.instance.Targets);
        }

        private IEnumerator SelectArtefactTarget(List<CardMovement> targets)
        {
            CardMovement caster = targets.First();
            Transform startPosition = caster.cardController.transform;
            
            currentArtefact = ComputeCurrentArtefact(caster.cardController.cardData);
            currentArtefactController = caster.cardController;
            
            switch (currentArtefact)
            {
                case Artefacts.None:
                    CancelTargeting();
                    yield break;
                case Artefacts.Cookie:
                case Artefacts.Bullet:
                case Artefacts.Potion:
                case Artefacts.Helm:
                    yield return TargetingSystem.instance.SelectTargets(caster, startPosition, TargetType.Ally, TargetingMode.Single, 1, false, null);
                    break;
                case Artefacts.Grenade:
                    yield return TargetingSystem.instance.SelectTargets(caster, startPosition, TargetType.Enemy, TargetingMode.All, 1, false, null);
                    break;
                case Artefacts.HolyGrenade:
                case Artefacts.PoisonCookie:
                case Artefacts.Fireball:
                case Artefacts.Pepper:
                    yield return TargetingSystem.instance.SelectTargets(caster, startPosition, TargetType.Enemy, TargetingMode.Single, 1, false, null);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            if (TargetingSystem.instance.IsCanceled)
                CancelTargeting();
            else
                yield return LockActionAndCast(TargetingSystem.instance.Targets);
        }

        private Artefacts ComputeCurrentArtefact(CardData cardData)
        {
            if (cardData.cardName == "Cookie")
                return Artefacts.Cookie;
            if (cardData.cardName == "Ammunition")
                return Artefacts.Bullet;
            if (cardData.cardName == "Health Potion")
                return Artefacts.Potion;
            if (cardData.cardName == "Helm")
                return Artefacts.Helm;
            if (cardData.cardName == "Grenade")
                return Artefacts.Grenade;
            if (cardData.cardName == "Holy Grenade")
                return Artefacts.HolyGrenade;
            if (cardData.cardName == "Poisonous Cookie")
                return Artefacts.PoisonCookie;
            if (cardData.cardName == "Fireball Scroll")
                return Artefacts.Fireball;
            if (cardData.cardName == "Strong Pepper")
                return Artefacts.Pepper;

            return Artefacts.None;
        }

        private bool Validator(CardMovement card)
        {
            return artefacts.Count(c => c.cardName == card.cardController.cardData.cardName) > 0;
        }
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            Debug.Log($"Casting Mole Use : {targets[0].cardController.cardData.cardName}");

            foreach (CardMovement target in targets)
            {
                switch (currentArtefact)
                {
                    case Artefacts.None:
                        break;
                    case Artefacts.Cookie:
                        HealGa healGa = new HealGa(1, currentArtefactController, target.cardController);
                        ActionSystem.instance.Perform(healGa);
                        break;
                    case Artefacts.Bullet:
                        ApplyStatusGa ammo = new ApplyStatusGa(StatusType.BulletAmmo, 1, currentArtefactController, target.cardController);
                        ActionSystem.instance.Perform(ammo);
                        break;
                    case Artefacts.Potion:
                        HealGa potionGa = new HealGa(3, currentArtefactController, target.cardController);
                        ActionSystem.instance.Perform(potionGa);
                        break;
                    case Artefacts.Helm:
                        ApplyStatusGa armor = new ApplyStatusGa(StatusType.Armor, 1, currentArtefactController, target.cardController);
                        ActionSystem.instance.Perform(armor);
                        break;
                    case Artefacts.Grenade:
                        DealDamageGA grenade = new DealDamageGA(1, currentArtefactController, target.cardController);
                        ActionSystem.instance.Perform(grenade);
                        break;
                    case Artefacts.HolyGrenade:
                        List<CardMovement> otherArtefacts = ComputeOtherArtefactsList();
                        int damage = 2 + 2 * otherArtefacts.Count;

                        for (int i = otherArtefacts.Count - 1; i >= 0; i--)
                        {
                            DeathGA deathGa = new DeathGA(currentArtefactController, otherArtefacts[i].cardController);
                            ActionSystem.instance.Perform(deathGa);
                            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
                        }

                        DealDamageGA holyGrenade = new DealDamageGA(damage, currentArtefactController, target.cardController);
                        ActionSystem.instance.Perform(holyGrenade);
                        break;
                    case Artefacts.PoisonCookie:
                        ApplyStatusGa weak = new ApplyStatusGa(StatusType.Weak, 1, currentArtefactController, target.cardController);
                        ActionSystem.instance.Perform(weak);
                        break;
                    case Artefacts.Fireball:
                        DealDamageGA fireball = new DealDamageGA(3, currentArtefactController, target.cardController);
                        ActionSystem.instance.Perform(fireball);
                        break;
                    case Artefacts.Pepper:
                        ApplyStatusGa stun = new ApplyStatusGa(StatusType.Stun, 1, currentArtefactController, target.cardController);
                        ActionSystem.instance.Perform(stun);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
                
                yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            }
            
            DeathGA selfDestroy = new DeathGA(cardController, currentArtefactController);
            ActionSystem.instance.Perform(selfDestroy);
        }

        private List<CardMovement> ComputeOtherArtefactsList()
        {
            List<CardMovement> artefacts = new List<CardMovement>();
            foreach (Slot slot in cardController.cardMovement.tokenContainer.Slots)
            {
                if (Validator(slot.CurrentCard) && slot.CurrentCard.cardController != currentArtefactController)
                    artefacts.Add(slot.CurrentCard);
            }

            return artefacts;
        }
    }
}
