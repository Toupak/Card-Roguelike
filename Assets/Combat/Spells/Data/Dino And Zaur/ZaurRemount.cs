using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using Cards.Scripts;
using Passives;
using PrimeTween;
using UnityEngine;

namespace Spells.Data.Dino_And_Zaur
{
    public class ZaurRemount : SpellController
    {
        [SerializeField] private PassiveData dinoAndZaurPassiveData;
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            foreach (CardMovement target in targets)
            {
                PassiveController passiveController = target.cardController.passiveHolder.GetPassive(dinoAndZaurPassiveData);
                if (passiveController != null)
                {
                    yield return MoveTowardTarget(target.rectTransform);
                    Remount(passiveController);
                    break;
                }
            }
        }

        private IEnumerator MoveTowardTarget(RectTransform targetRectTransform)
        {
            bool isArrived = false;

            cardController.SetFollowState(false);

            Sequence.Create()
                .Chain(Tween.Position(cardController.transform, targetRectTransform.transform.position, 0.2f, Ease.OutBack))
                .ChainCallback(() => isArrived = true);
            
            yield return new WaitUntil(() => isArrived);
        }
        
        private void Remount(PassiveController passiveController)
        {
            passiveController.GetComponent<DinoAndZaurPassive>().Remount();
            cardController.KillCard();    
        }
    }
}
