using System;
using System.Collections;
using Cards.Scripts;
using Character_Selection;
using CombatLoop.Battles;
using CombatLoop.Battles.Data;
using Data;
using MapMaker;
using MapMaker.Floors;
using MapMaker.Rooms;
using Overworld.Character;
using Run_Loop.Rewards;
using Tutorial;
using UnityEngine;
using UnityEngine.Events;

namespace Run_Loop
{
    public class RunLoop : MonoBehaviour
    {
        [Space]
        [SerializeField] private SceneField hubScene;
        [SerializeField] private SceneField introScene;
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

        public static UnityEvent OnStartRun = new UnityEvent();
        public static UnityEvent OnWinRun = new UnityEvent();
        public static UnityEvent OnStartBattle = new UnityEvent();
        
        public static RunLoop instance;

        private OverWorldCharacterData characterData;
        public OverWorldCharacterData CharacterData => isInRun && characterData != null ? characterData : defaultCharacter;
        public CardDatabase dataBase => cardDatabase;

        private int currentBattleIndex;
        public bool isInRun { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        public void StartRun()
        {
            if (isInRun)
                return;

            isInRun = true;
            StartCoroutine(StartNewRun(true));
        }
        
        public void StartRunFromIntro()
        {
            if (isInRun)
                return;

            isInRun = true;
            StartCoroutine(StartNewRun(false, () => TutorialManager.instance.PlayCharacterWakeUpAnimation()));
        }

        private IEnumerator StartNewRun(bool isSelectingCharacter, Action callback = null)
        {
            currentBattleIndex = 0;
            PlayerDeck.instance.ClearDeck();
            MapBuilder.instance.SetupMap(floorData);

            if (isSelectingCharacter)
                yield return SelectCharacter();
            
            LoadCharacterDeck(CharacterData);
            SpawnCharacter();
            
            yield return LoadRoom(RoomBuilder.instance.GetStartingRoom(), callback);
            OnStartRun?.Invoke();
        }

        private IEnumerator SelectCharacter()
        {
            yield return SceneLoader.instance.LoadScene(characterSelectionScene);
            yield return new WaitUntil(IsCharacterSelected);
            StoreSelectedCharacter();
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
            yield return SceneLoader.instance.LoadScene(combatScene);
            OnStartBattle?.Invoke();
            yield return new WaitUntil(IsCombatOver);
            bool isPlayerAlive = CheckCombatResult();
            StoreCardsHealth();

            if (!IsRunOver() && isPlayerAlive)
            {
                yield return SceneLoader.instance.LoadScene(rewardScene);
                yield return new WaitUntil(IsRewardSelected);
                
                currentBattleIndex += 1;
                if (currentBattleIndex >= battlesDataHolder.battles.Count)
                    currentBattleIndex = 0;
                
                RoomBuilder.instance.MarkCurrentRoomAsCleared();
                yield return LoadRoom(RoomBuilder.instance.GetCurrentRoom(), UnlockPlayer);
                
                UnlockRoom();
            }

            if (!isPlayerAlive)
                yield return GoBackToHub();
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
            if (!SceneLoader.instance.IsLoading)
                StartCoroutine(GoToNextRoom(doorDirection));
        }
        
        public void GoToIntroFromMainMenu()
        {
            StartCoroutine(SceneLoader.instance.LoadScene(introScene.SceneName));
        }
        
        public void LoadHubFromMainMenu()
        {
            StartCoroutine(GoBackToHub());
        }

        private IEnumerator GoBackToHub()
        {
            yield return SceneLoader.instance.LoadScene(hubScene, MovePlayerToCenterOfRoom);
        }

        private IEnumerator GoToNextRoom(RoomData.DoorDirection doorDirection)
        {
            yield return LoadRoom(RoomBuilder.instance.GetNextRoom(doorDirection),() => MovePlayerToRoomDoor(doorDirection));

            if (RoomBuilder.instance.GetCurrentRoomType() == RoomData.RoomType.Battle && !RoomBuilder.instance.HasRoomBeenCleared())
                LockRoom();
        }

        private IEnumerator LoadRoom(string roomName, Action callback = null)
        {
            yield return SceneLoader.instance.LoadRoom(roomName, RoomBuilder.instance.GetCurrentRoomType(), RoomBuilder.instance.HasRoomBeenCleared(), callback);
        }

        private void LockRoom()
        {
            DoorHolder.instance.LockAllDoors();
        }

        private void UnlockRoom()
        {
            DoorHolder.instance.UnlockAllDoors();
        }

        public BattleData SelectBattle()
        {
            return battlesDataHolder.ChooseRandomBattle(BattleData.Floor.First, currentBattleIndex < 3 ? BattleData.Difficulty.Easy : BattleData.Difficulty.Hard);
        }
    }
}
