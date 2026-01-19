using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Passives;
using UnityEngine;

namespace Spells.Data.Snuk
{
    public class SnukEggWait : PassiveController
    {
        [SerializeField] private CardData tokenData;
        [SerializeField] private int turnCountBeforeHatching;
        [SerializeField] private List<Sprite> sprites;
        
        private int currentTurnCount;
        
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<StartTurnGa>(StartTurnGaReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<StartTurnGa>(StartTurnGaReaction, ReactionTiming.POST);
        }
        
        private void StartTurnGaReaction(StartTurnGa startTurnGa)
        {
            if (startTurnGa.starting == CombatLoop.CombatLoop.TurnType.Player)
            {
                currentTurnCount += 1;

                if (currentTurnCount >= turnCountBeforeHatching)
                    HatchEgg();
                else
                    UpdateEggSprite();
            }
        }

        private void HatchEgg()
        {
            SpawnCardGA spawnCardGa = new SpawnCardGA(tokenData, cardController.tokenParentController, true);
            ActionSystem.instance.AddReaction(spawnCardGa);
            cardController.KillCard();
        }

        private void UpdateEggSprite()
        {
            int index = Mathf.Min(currentTurnCount, sprites.Count - 1);
            cardController.SetArtwork(sprites[index]);
        }
    }
}
