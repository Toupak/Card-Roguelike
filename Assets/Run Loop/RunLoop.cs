using System.Collections;
using Battles.Data;
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

namespace Run_Loop
{
    public class RunLoop : MonoBehaviour
    {
        [Space]
        [SerializeField] private SceneField hubScene;
        [SerializeField] private SceneField rewardScene;
        [SerializeField] private SceneField combatScene;
        [SerializeField] private SceneField characterSelectionScene;

        [Space] 
        [SerializeField] private GameObject characterPrefab;
        
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

        private SceneLoader sceneLoader;
        
        private int currentBattleIndex;
        private bool isInRun;

        private void Awake()
        {
            instance = this;

            sceneLoader = GetComponent<SceneLoader>();
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
            
            yield return sceneLoader.LoadScene(characterSelectionScene);
            yield return new WaitUntil(IsCharacterSelected);
            StoreSelectedCharacter();
            LoadCharacterDeck(characterData);
            SpawnCharacter();
            
            yield return sceneLoader.LoadScene(RoomBuilder.instance.GetStartingRoom());
        }

        private void SpawnCharacter()
        {
            DontDestroyOnLoad(Instantiate(characterPrefab, Vector3.zero, Quaternion.identity));
        }
        
        private void MovePlayerToRoomDoor(RoomData.DoorDirection doorDirection)
        {
            CharacterSingleton.instance.transform.position = DoorHolder.instance.GetDoorExitPosition(doorDirection, 1.5f);
        }

        private void MovePlayerToCenterOfRoom()
        {
            CharacterSingleton.instance.transform.position = Vector3.zero;
        }

        private void LockPlayer()
        {
            CharacterSingleton.instance.LockPlayer();
        }

        private void UnlockPlayer()
        {
            CharacterSingleton.instance.UnlockPlayer();
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
            LockPlayer();
            yield return sceneLoader.LoadScene(combatScene);
            yield return new WaitUntil(IsCombatOver);
            bool isPlayerAlive = CheckCombatResult();
            StoreCardsHealth();

            if (!IsRunOver() && isPlayerAlive)
            {
                yield return sceneLoader.LoadScene(rewardScene);
                yield return new WaitUntil(IsRewardSelected);
                
                currentBattleIndex += 1;
                if (currentBattleIndex >= battlesDataHolder.battles.Count)
                    currentBattleIndex = 0;
                
                yield return sceneLoader.LoadScene(RoomBuilder.instance.GetCurrentRoom(), UnlockPlayer);
            }
            
            if (!isPlayerAlive)
                yield return sceneLoader.LoadScene(hubScene, MovePlayerToCenterOfRoom);
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
        
        public void OnTriggerDoor(RoomData.DoorDirection doorDirection)
        {
            if (!sceneLoader.IsLoading)
                StartCoroutine(GoToNextRoom(doorDirection));
        }

        private IEnumerator GoToNextRoom(RoomData.DoorDirection doorDirection)
        {
            yield return sceneLoader.LoadScene(RoomBuilder.instance.GetNextRoom(doorDirection), () => MovePlayerToRoomDoor(doorDirection));
        }
    }
}
