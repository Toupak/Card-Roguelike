using System;
using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Passives;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Spells.Data.Faces
{
    public class FaceSwapMask : SpellController
    {
        [SerializeField] private SpellData whiteSpell;
        [SerializeField] private SpellData blueSpell;
        [SerializeField] private SpellData redSpell;
        
        [Space]
        [SerializeField] private PassiveData whitePassive;
        [SerializeField] private PassiveData bluePassive;
        [SerializeField] private PassiveData redPassive;

        [Space]
        [SerializeField] private Sprite whiteSprite;
        [SerializeField] private Sprite blueSprite;
        [SerializeField] private Sprite redSprite;
        
        [Space]
        [SerializeField] private List<CardData> tokenData;
        
        private enum FaceColor
        {
            White,
            Blue,
            Red
        }

        private FaceColor currentColor;
        
        protected override IEnumerator CastSpellOnTarget(List<CardMovement> targets)
        {
            yield return base.CastSpellOnTarget(targets);

            if (currentColor == FaceColor.White)
                HasCastedThisTurn = true;
            else if (currentColor == FaceColor.Red)
                HasCastedThisTurn = false;
            
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            int index = Random.Range(0, tokenData.Count);
            SpawnCardGA spawnCardGa = new SpawnCardGA(tokenData[index], cardController, true);
            ActionSystem.instance.Perform(spawnCardGa);
            
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            cardController.passiveHolder.RemovePassive(ComputePassive(currentColor));
            currentColor = ComputeNextColor(currentColor);
            cardController.SetArtwork(ComputeSprite(currentColor));
            cardController.SetupRightSpell(ComputeSpell(currentColor));
            cardController.passiveHolder.AddPassive(ComputePassive(currentColor));
        }
        
        private SpellData ComputeSpell(FaceColor faceColor)
        {
            switch (faceColor)
            {
                case FaceColor.White:
                    return whiteSpell;
                case FaceColor.Blue:
                    return blueSpell;
                case FaceColor.Red:
                    return redSpell;
                default:
                    throw new ArgumentOutOfRangeException(nameof(faceColor), faceColor, null);
            }
        }
        
        private PassiveData ComputePassive(FaceColor faceColor)
        {
            switch (faceColor)
            {
                case FaceColor.White:
                    return whitePassive;
                case FaceColor.Blue:
                    return bluePassive;
                case FaceColor.Red:
                    return redPassive;
                default:
                    throw new ArgumentOutOfRangeException(nameof(faceColor), faceColor, null);
            }
        }
        
        private Sprite ComputeSprite(FaceColor faceColor)
        {
            switch (faceColor)
            {
                case FaceColor.White:
                    return whiteSprite;
                case FaceColor.Blue:
                    return blueSprite;
                case FaceColor.Red:
                    return redSprite;
                default:
                    throw new ArgumentOutOfRangeException(nameof(faceColor), faceColor, null);
            }
        }

        private FaceColor ComputeNextColor(FaceColor faceColor)
        {
            switch (faceColor)
            {
                case FaceColor.White:
                    return FaceColor.Blue;
                case FaceColor.Blue:
                    return FaceColor.Red;
                case FaceColor.Red:
                    return FaceColor.White;
                default:
                    throw new ArgumentOutOfRangeException(nameof(faceColor), faceColor, null);
            }
        }
    }
}
