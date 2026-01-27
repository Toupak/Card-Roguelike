using System;
using System.Collections;
using System.Collections.Generic;
using ActionReaction;
using ActionReaction.Game_Actions;
using Cards.Scripts;
using Combat.Passives;
using UnityEngine;

namespace Combat.Spells.Data.Gardener
{
    public class GardenerPotsPassive : PassiveController
    {
        private enum PotColor
        {
            Blue,
            Green,
            Red,
            Error
        }
        
        [SerializeField] private int plantStartingHealthPoints;

        [Space]
        [SerializeField] private CardData bluePot;
        [SerializeField] private CardData greenPot;
        [SerializeField] private CardData redPot;

        [Space]
        [SerializeField] private List<Sprite> blueSprites;
        [SerializeField] private List<Sprite> greenSprites;
        [SerializeField] private List<Sprite> redSprites;

        [Space]
        [SerializeField] private PassiveData seedPassive;
        
        [Space]
        [SerializeField] private PassiveData bluePotPassive;
        [SerializeField] private PassiveData greenPotPassive;
        [SerializeField] private PassiveData redPotPassive;
        
        [Space]
        [SerializeField] private List<PassiveData> bluePlantPassives;
        [SerializeField] private List<PassiveData> greenPlantPassives;
        [SerializeField] private List<PassiveData> redPlantPassives;
        
        private CardController bluePotController;
        private CardController greenPotController;
        private CardController redPotController;

        public int bluePlantLevel { get; private set; } = 0;
        public int greenPlantLevel { get; private set; } = 0;
        public int redPlantLevel { get; private set; } = 0;
        
        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<StartTurnGa>(SpawnPots, ReactionTiming.POST);
            ActionSystem.SubscribeReaction<EndTurnGA>(UpdatePlantHealth, ReactionTiming.PRE, -10);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<StartTurnGa>(SpawnPots, ReactionTiming.POST);
            ActionSystem.UnsubscribeReaction<EndTurnGA>(UpdatePlantHealth, ReactionTiming.PRE);
        }

        private void SpawnPots(StartTurnGa startTurnGa)
        {
            if (startTurnGa.starting == CombatLoop.TurnType.Player && startTurnGa.turnCount == 1)
                StartCoroutine(SpawnPotsCoroutine());
        }
        
        private IEnumerator SpawnPotsCoroutine()
        {
            SpawnCardGA spawnBluePot = new SpawnCardGA(bluePot, cardController, true);
            SpawnCardGA spawnGreenPot = new SpawnCardGA(greenPot, cardController, true);
            SpawnCardGA spawnRedPot = new SpawnCardGA(redPot, cardController, true);
                
            ActionSystem.instance.AddReaction(spawnBluePot);
            ActionSystem.instance.AddReaction(spawnGreenPot);
            ActionSystem.instance.AddReaction(spawnRedPot);
            
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            
            bluePotController = spawnBluePot.spawnedCard;
            greenPotController = spawnGreenPot.spawnedCard;
            redPotController = spawnRedPot.spawnedCard;
        }
        
        private void UpdatePlantHealth(EndTurnGA endTurnGa)
        {
            if (endTurnGa.ending == CombatLoop.TurnType.Player)
                StartCoroutine(UpdatePlantHealthCoroutine());
        }

        private IEnumerator UpdatePlantHealthCoroutine()
        {
            if (bluePlantLevel > 0)
            {
                DealDamageGA dealDamageGa = new DealDamageGA(1, bluePotController, bluePotController);
                ActionSystem.instance.AddReaction(dealDamageGa);
            }
            if (greenPlantLevel > 0)
            {
                DealDamageGA dealDamageGa = new DealDamageGA(1, greenPotController, greenPotController);
                ActionSystem.instance.AddReaction(dealDamageGa);
            }
            if (redPlantLevel > 0)
            {
                DealDamageGA dealDamageGa = new DealDamageGA(1, redPotController, redPotController);
                ActionSystem.instance.AddReaction(dealDamageGa);
            }
            
            yield return new WaitWhile(() => ActionSystem.instance.IsPerforming);
            
            if (bluePlantLevel > 0 && bluePotController.cardHealth.currentHealth == 0)
            {
                bluePlantLevel = 0;
                yield return UpdatePlantVisuals(PotColor.Blue, bluePlantLevel);
            }
            if (greenPlantLevel > 0 && greenPotController.cardHealth.currentHealth == 0)
            {
                greenPlantLevel = 0;
                yield return UpdatePlantVisuals(PotColor.Green, greenPlantLevel);
            }
            if (redPlantLevel > 0 && redPotController.cardHealth.currentHealth == 0)
            {
                redPlantLevel = 0;
                yield return UpdatePlantVisuals(PotColor.Red, redPlantLevel);
            }
        }

        public IEnumerator PlantSeed(CardData data)
        {
            PotColor potColor = ComputePotColor(data);
            if (potColor == PotColor.Error)
                yield break;

            int plantLevel = ComputePlantLevel(potColor);

            if (plantLevel == 0)
                yield return IncreasePlantLevel(potColor);
            else
                yield return HealPlant(potColor);
        }

        public IEnumerator UseWater(CardData data)
        {
            PotColor potColor = ComputePotColor(data);
            if (potColor == PotColor.Error)
                yield break;
            
            int plantLevel = ComputePlantLevel(potColor);
            
            if (plantLevel == 5)
                yield return HealPlant(potColor);
            else if (plantLevel > 0)
                yield return IncreasePlantLevel(potColor);
        }

        private IEnumerator IncreasePlantLevel(PotColor potColor)
        {
            int plantLevel = 0;
            switch (potColor)
            {
                case PotColor.Blue:
                    bluePlantLevel = Mathf.Min(bluePlantLevel + 1, 5);
                    plantLevel = bluePlantLevel;
                    break;
                case PotColor.Green:
                    greenPlantLevel = Mathf.Min(greenPlantLevel + 1, 5);
                    plantLevel = greenPlantLevel;
                    break;
                case PotColor.Red:
                    redPlantLevel = Mathf.Min(redPlantLevel + 1, 5);
                    plantLevel = redPlantLevel;
                    break;
                case PotColor.Error:
                default:
                    throw new ArgumentOutOfRangeException();
            }

            yield return UpdatePlantVisuals(potColor, plantLevel);
        }

        private IEnumerator UpdatePlantVisuals(PotColor potColor, int plantLevel)
        {
            CardController pot = ComputePotController(potColor);
            pot.SetArtwork(ComputeArtwork(potColor, plantLevel));

            UpdateCardHealthDisplay(pot, plantLevel);
            UpdateCardPassives(pot, potColor, plantLevel);

            yield break;
        }

        private void UpdateCardHealthDisplay(CardController pot, int plantLevel)
        {
            if (plantLevel == 0)
            {
                pot.cardHealth.Hide();
            }
            else if (plantLevel == 1)
            {
                pot.cardHealth.SetHealth(plantStartingHealthPoints);
                pot.cardHealth.Show();
                
            }
        }
        
        private void UpdateCardPassives(CardController pot, PotColor potColor, int plantLevel)
        {
            pot.passiveHolder.RemoveAllPassives();
            pot.passiveHolder.AddPassive(ComputePotPassive(potColor));
            
            if (plantLevel > 0)
                pot.passiveHolder.AddPassive(seedPassive);
            
            if (plantLevel > 2)
                pot.passiveHolder.AddPassive(ComputePlantPassive(potColor, plantLevel));
        }

        private IEnumerator HealPlant(PotColor potColor)
        {
            CardController pot = ComputePotController(potColor);
            HealGa healGa = new HealGa(1, cardController, pot);
            ActionSystem.instance.Perform(healGa);
            yield break;
        }
        
        private int ComputePlantLevel(PotColor potColor)
        {
            switch (potColor)
            {
                case PotColor.Blue:
                    return bluePlantLevel;
                case PotColor.Green:
                    return greenPlantLevel;
                case PotColor.Red:
                    return redPlantLevel;
                case PotColor.Error:
                default:
                    throw new ArgumentOutOfRangeException(nameof(potColor), potColor, null);
            }
        }

        private Sprite ComputeArtwork(PotColor potColor, int plantLevel)
        {
            switch (potColor)
            {
                case PotColor.Blue:
                    return blueSprites[plantLevel];
                case PotColor.Green:
                    return greenSprites[plantLevel];
                case PotColor.Red:
                    return redSprites[plantLevel];
                case PotColor.Error:
                default:
                    throw new ArgumentOutOfRangeException(nameof(potColor), potColor, null);
            }
        }

        private PotColor ComputePotColor(CardData data)
        {
            if (data.cardName == bluePot.cardName)
                return PotColor.Blue;
            else if (data.cardName == greenPot.cardName)
                return PotColor.Green;
            else if (data.cardName == redPot.cardName)
                return PotColor.Red;
            else
                return PotColor.Error;
        }

        private CardController ComputePotController(PotColor potColor)
        {
            switch (potColor)
            {
                case PotColor.Blue:
                    return bluePotController;
                case PotColor.Green:
                    return greenPotController;
                case PotColor.Red:
                    return redPotController;
                case PotColor.Error:
                default:
                    throw new ArgumentOutOfRangeException(nameof(potColor), potColor, null);
            }
        }
        
        private PassiveData ComputePotPassive(PotColor potColor)
        {
            switch (potColor)
            {
                case PotColor.Blue:
                    return bluePotPassive;
                case PotColor.Green:
                    return greenPotPassive;
                case PotColor.Red:
                    return redPotPassive;
                case PotColor.Error:
                default:
                    throw new ArgumentOutOfRangeException(nameof(potColor), potColor, null);
            }   
        }
        
        private PassiveData ComputePlantPassive(PotColor potColor, int plantLevel)
        {
            switch (potColor)
            {
                case PotColor.Blue:
                    return bluePlantPassives[plantLevel - 3];
                case PotColor.Green:
                    return greenPlantPassives[plantLevel - 3];
                case PotColor.Red:
                    return redPlantPassives[plantLevel - 3];
                case PotColor.Error:
                default:
                    throw new ArgumentOutOfRangeException(nameof(potColor), potColor, null);
            }
        }
    }
}
