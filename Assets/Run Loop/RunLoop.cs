using System;
using System.Collections;
using System.Collections.Generic;
using BoomLib.Tools;
using Cards.Scripts;
using Character_Selection;
using Character_Selection.Character;
using Combat;
using Combat.Battles;
using Combat.Battles.Data;
using Inventory.Items.Frames;
using Map;
using Map.Floors;
using Map.MiniMap;
using Map.Rooms;
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
        [SerializeField] private FrameDatabase frameDatabase;
        
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
        public CardDatabase CardDatabase => cardDatabase;
        public FrameDatabase FrameDatabase => frameDatabase;

        public int currentBattleIndex { get; private set; }
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
            
            MinimapBuilder.instance.SetMinimapState(true);
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
            yield return SceneLoader.instance.LoadScene(combatScene, () => MinimapBuilder.instance.SetMinimapState(false));
            OnStartBattle?.Invoke();
            yield return new WaitUntil(IsCombatOver);
            bool isPlayerAlive = CheckCombatResult();
            yield return PerformEndBattleAnimation(isPlayerAlive);
            StoreCardsHealth();

            if (!IsRunOver() && isPlayerAlive)
            {
                yield return PerformRewardScene();
                
                currentBattleIndex += 1;

                RoomBuilder.instance.MarkCurrentRoomAsCleared();
                yield return LoadRoom(RoomBuilder.instance.GetCurrentRoomName(), () =>
                {
                    MinimapBuilder.instance.SetMinimapState(true);
                    UnlockPlayer();
                });
                
                UnlockRoom();
            }

            if (!isPlayerAlive)
                yield return GoBackToHub();
        }

        public void StartOpeningReward()
        {
            StartCoroutine(OpenReward());
        }

        private IEnumerator OpenReward()
        {
            MinimapBuilder.instance.SetMinimapState(false);
            yield return PerformRewardScene();
            RoomBuilder.instance.MarkCurrentRoomAsCleared();
            yield return LoadRoom(RoomBuilder.instance.GetCurrentRoomName(), () =>
            {
                MinimapBuilder.instance.SetMinimapState(true);
                UnlockPlayer();
            });
        }

        private IEnumerator PerformRewardScene()
        {
            yield return SceneLoader.instance.LoadScene(rewardScene);
            yield return new WaitUntil(IsRewardSelected);
        }

        private bool IsRewardSelected()
        {
            return RewardLoop.instance != null && RewardLoop.instance.isRewardScreenOver;
        }

        private bool IsCombatOver()
        {
            return CombatLoop.instance.IsMatchOver();
        }

        private bool CheckCombatResult()
        {
            return CombatLoop.instance.HasPlayerWon();
        }
        
        private IEnumerator PerformEndBattleAnimation(bool isPlayerAlive)
        {
            yield return CombatLoop.instance.PerformEndBattleAnimation(isPlayerAlive);
        }
        
        private void StoreCardsHealth()
        {
            CombatLoop.instance.StoreCardsHealth();
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
            yield return SceneLoader.instance.LoadScene(hubScene, () =>
            {
                isInRun = false;
                MovePlayerToCenterOfRoom();
                UnlockPlayer();
            });
        }

        private IEnumerator GoToNextRoom(RoomData.DoorDirection doorDirection)
        {
            yield return LoadRoom(RoomBuilder.instance.GetNextRoom(doorDirection),() =>
            {
                MinimapBuilder.instance.UpdateMap();
                MovePlayerToRoomDoor(doorDirection);
            });

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
           return battlesDataHolder.ChooseRandomBattle(BattleData.Floor.First, ComputeDifficulty(RoomBuilder.instance.CurrentRoom.roomType));
        }

        private BattleData.Difficulty ComputeDifficulty(RoomData.RoomType roomType)
        {
            switch (roomType)
            {
                case RoomData.RoomType.Starting:
                case RoomData.RoomType.Encounter:
                case RoomData.RoomType.Battle:
                    return currentBattleIndex < 3 ? BattleData.Difficulty.Easy : BattleData.Difficulty.Hard;
                case RoomData.RoomType.Elite:
                    return BattleData.Difficulty.Elite;
                case RoomData.RoomType.Boss:
                    return BattleData.Difficulty.Boss;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        public BattleData SelectBattle(BattleData.Floor floor, BattleData.Difficulty difficulty)
        {
            return battlesDataHolder.ChooseRandomBattle(floor, difficulty);
        }

        public FloorData GetCurrentFloorData()
        {
            return floorData;
        }
    }
}
