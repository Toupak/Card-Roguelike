using System;
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

        private int currentBattleIndex;
        private bool isInRun;

        private void Awake()
        {
            instance = this;
        }

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
            
            yield return SceneLoader.instance.LoadScene(characterSelectionScene);
            yield return new WaitUntil(IsCharacterSelected);
            StoreSelectedCharacter();
            LoadCharacterDeck(characterData);
            SpawnCharacter();
            
            yield return LoadRoom(RoomBuilder.instance.GetStartingRoom());
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
    }
}
