using System;
using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
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
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);

            int index = Random.Range(0, tokenData.Count);
            SpawnCardGA spawnCardGa = new SpawnCardGA(tokenData[index], cardController, true);
            ActionSystem.instance.Perform(spawnCardGa);
            
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            
            currentColor = ComputeNextColor(currentColor);
            cardController.SetArtwork(ComputeSprite(currentColor));
            cardController.SetupRightSpell(ComputeSpell(currentColor));
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
