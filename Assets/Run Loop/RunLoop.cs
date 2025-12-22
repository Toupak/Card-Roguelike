using System.Collections;
using Battles.Data;
using BoomLib.Tools;
using Cards.Scripts;
using Character_Selection;
using CombatLoop.Battles;
using Data;
using MapMaker;
using MapMaker.Floors;
using MapMaker.Rooms;
using Overworld.Character;
using Run_Loop.Rewards;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Run_Loop
{
    public class RunLoop : MonoBehaviour
    {
        [SerializeField] private Image blackScreen;
        
        [Space]
        [SerializeField] private SceneField hubScene;
        [SerializeField] private SceneField overWorldScene;
        [SerializeField] private SceneField rewardScene;
        [SerializeField] private SceneField combatScene;
        [SerializeField] private SceneField characterSelectionScene;

        [Space] 
        [SerializeField] private FloorData floorData;
        
        [Space] 
        [SerializeField] private CardDatabase cardDatabase;
        
        [Space] 
        [SerializeField] private OverWorldCharacterData defaultCharacter;

        [Space] 
        [SerializeField] private BattlesDataHolder battlesDataHolder;

        public static RunLoop instance;

        private OverWorldCharacterData characterData;
        public OverWorldCharacterData CharacterData => isInRun ? characterData : defaultCharacter;
        public CardDatabase dataBase => cardDatabase;
        public BattleData currentBattleData => battlesDataHolder.battles[currentBattleIndex];

        private int currentBattleIndex;
        private bool isInRun;

        private void Awake()
        {
            instance = this;
        }

        /*
        private void Start()
        {
            string currentSceneName = SceneManager.GetActiveScene().name;
            
            if (currentSceneName != hubScene)
            {
                Debug.Log($"Game was not started in scene : {hubScene.SceneName} => RunLoop will self destroy.");
                Destroy(gameObject);
                return;
            }
        }
        */

        public void StartRun()
        {
            if (isInRun)
                return;

            isInRun = true;
            StartCoroutine(StartNewRun());
        }

        private IEnumerator StartNewRun()
        {
            currentBattleIndex = 0;
            PlayerDeck.instance.ClearDeck();
            MapBuilder.instance.SetupMap(floorData);
            
            yield return LoadScene(characterSelectionScene);
            yield return new WaitUntil(IsCharacterSelected);
            StoreSelectedCharacter();
            LoadCharacterDeck(characterData);
            
            yield return LoadScene(RoomBuilder.instance.GetStartingRoom());
        }

        private void LoadCharacterDeck(OverWorldCharacterData data)
        {
            foreach (CardData card in data.startingCards)
            {
                PlayerDeck.instance.AddCardToDeck(card);
            }
        }

        private bool IsCharacterSelected()
        {
            return CharacterSelectionLoop.instance != null && CharacterSelectionLoop.instance.isCharacterSelected;
        }
        
        private void StoreSelectedCharacter()
        {
            characterData = CharacterSelectionLoop.instance.GetSelectedCharacter();
        }
        
        public void StartBattle()
        {
            if (!isInRun && PlayerDeck.instance.IsEmpty)
                LoadCharacterDeck(defaultCharacter);
            
            StartCoroutine(StartNewBattle());
        }

        private IEnumerator StartNewBattle()
        {
            yield return LoadScene(combatScene);
            yield return new WaitUntil(IsCombatOver);
            bool isPlayerAlive = CheckCombatResult();
            StoreCardsHealth();

            if (!IsRunOver() && isPlayerAlive)
            {
                yield return LoadScene(rewardScene);
                yield return new WaitUntil(IsRewardSelected);
                
                currentBattleIndex += 1;
                if (currentBattleIndex >= battlesDataHolder.battles.Count)
                    currentBattleIndex = 0;
                
                yield return LoadScene(RoomBuilder.instance.GetCurrentRoom());
            }
            
            if (!isPlayerAlive)
                yield return LoadScene(hubScene);
        }

        private bool IsRewardSelected()
        {
            return RewardLoop.instance != null && RewardLoop.instance.isRewardScreenOver;
        }

        private bool IsCombatOver()
        {
            return CombatLoop.CombatLoop.instance.IsMatchOver();
        }

        private bool CheckCombatResult()
        {
            return CombatLoop.CombatLoop.instance.HasPlayerWon();
        }
        
        private void StoreCardsHealth()
        {
            CombatLoop.CombatLoop.instance.StoreCardsHealth();
        }
        
        private bool IsRunOver()
        {
            return false;
        }

        private IEnumerator LoadScene(string sceneName)
        {
            Debug.Log($"Load Scene : {sceneName}");
            yield return Fader.Fade(blackScreen, 0.3f, true);

            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
            yield return new WaitUntil(() => operation.isDone);
            
            yield return Fader.Fade(blackScreen, 0.3f, false);
        }

        public void OnTriggerDoor(RoomData.DoorDirection doorDirection)
        {
            StartCoroutine(GoToNextRoom(doorDirection));
        }

        private IEnumerator GoToNextRoom(RoomData.DoorDirection doorDirection)
        {
            yield return LoadScene(RoomBuilder.instance.GetNextRoom(doorDirection));
        }
    }
}
