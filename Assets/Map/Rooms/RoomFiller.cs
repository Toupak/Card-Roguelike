using System;
using System.Collections.Generic;
using System.Linq;
using Map.Encounters;
using Run_Loop;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Map.Rooms
{
    public class RoomFiller : MonoBehaviour
    {
        [SerializeField] private GameObject battleInteractionPrefab;
        [SerializeField] private Sprite battleInteractionIcon;
        
        [Space]
        [SerializeField] private GameObject eliteBattleGroundMark;
        [SerializeField] private Sprite eliteBattleIcon;
        
        [Space]
        [SerializeField] private GameObject bossBattleGroundMark;
        [SerializeField] private Sprite bossBattleIcon;

        private Dictionary<EncounterPrefabData, int> mandatoryEncountersPrefabs = new Dictionary<EncounterPrefabData, int>();
        private Dictionary<EncounterPrefabData, int> optionalEncountersPrefabs = new Dictionary<EncounterPrefabData, int>();
        
        public static RoomFiller instance;
        
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            MapBuilder.OnBuildMap.AddListener(StoreEncounterData);
        }

        private void StoreEncounterData()
        {
            List<EncounterPrefabData> mandatory = RunLoop.instance.GetCurrentFloorData().mandatoryEncountersPrefabs;
            List<EncounterPrefabData> optional = RunLoop.instance.GetCurrentFloorData().optionalEncountersPrefabs;

            foreach (EncounterPrefabData prefabData in mandatory)
                mandatoryEncountersPrefabs.Add(prefabData, Random.Range(prefabData.minAmountPerFloor, prefabData.maxAmountPerFloor + 1));
            
            foreach (EncounterPrefabData prefabData in optional)
                optionalEncountersPrefabs.Add(prefabData, Random.Range(prefabData.minAmountPerFloor, prefabData.maxAmountPerFloor + 1));
        }

        public void FillRoom(RoomPackage roomPackage)
        {
            switch (roomPackage.roomType)
            {
                case RoomData.RoomType.Starting:
                    SetupStartingRoom();
                    break;
                case RoomData.RoomType.Battle:
                    SetupBattleRoom(roomPackage);
                    break;
                case RoomData.RoomType.Encounter:
                    SetupSpecialRoom(roomPackage);
                    break;
                case RoomData.RoomType.Elite:
                    SetupEliteRoom(roomPackage);
                    break;
                case RoomData.RoomType.Boss:
                    SetupBossRoom(roomPackage);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetupStartingRoom()
        {
            
        }
        
        private void SetupBattleRoom(RoomPackage roomPackage)
        {
            if (roomPackage.pointOfInterests.Count < 1)
                roomPackage.AddPointOfInterest(battleInteractionPrefab, battleInteractionIcon, ComputeNewPosition(), true);
            
            SpawnRoomContent(roomPackage);
        }

        private void SetupSpecialRoom(RoomPackage roomPackage)
        {
            if (roomPackage.pointOfInterests.Count < 1)
            {
                EncounterPrefabData encounter = ComputeSpecialRoomEncounter();
                roomPackage.AddPointOfInterest(encounter.prefab, encounter.minimapIcon, ComputeNewPosition(), true);
            }
            
            SpawnRoomContent(roomPackage);
        }

        private EncounterPrefabData ComputeSpecialRoomEncounter()
        {
            IEnumerable<KeyValuePair<EncounterPrefabData, int>> validMandatory = mandatoryEncountersPrefabs.Where((kp) => kp.Value > 0);

            List<KeyValuePair<EncounterPrefabData, int>> mandatoryResults = validMandatory.ToList();
            if (mandatoryResults.Any())
            {
                KeyValuePair<EncounterPrefabData, int> result = mandatoryResults[Random.Range(0, mandatoryResults.Count())];
                mandatoryEncountersPrefabs[result.Key] -= 1;
                return result.Key;
            }
            
            IEnumerable<KeyValuePair<EncounterPrefabData, int>> optional = optionalEncountersPrefabs.Where((kp) => kp.Value > 0);

            List<KeyValuePair<EncounterPrefabData, int>> optionalResults = optional.ToList();
            if (optionalResults.Any())
            {
                KeyValuePair<EncounterPrefabData, int> result = optionalResults[Random.Range(0, optionalResults.Count())];
                optionalEncountersPrefabs[result.Key] -= 1;
                return result.Key;
            }
            
            List<KeyValuePair<EncounterPrefabData, int>> randomResults = optionalEncountersPrefabs.ToList();
            return randomResults[Random.Range(0, optionalResults.Count())].Key;
        }

        private void SetupEliteRoom(RoomPackage roomPackage)
        {
            if (roomPackage.pointOfInterests.Count < 1)
            {
                roomPackage.AddPointOfInterest(eliteBattleGroundMark, null, Vector3.zero, false);
                roomPackage.AddPointOfInterest(battleInteractionPrefab, eliteBattleIcon, ComputeNewPosition(), true);
            }
            
            SpawnRoomContent(roomPackage);
        }
        
        private void SetupBossRoom(RoomPackage roomPackage)
        {
            if (roomPackage.pointOfInterests.Count < 1)
            {
                roomPackage.AddPointOfInterest(bossBattleGroundMark, null, Vector3.zero, false);
                roomPackage.AddPointOfInterest(battleInteractionPrefab, bossBattleIcon, ComputeNewPosition(), true);
            }
            
            SpawnRoomContent(roomPackage);
        }

        private void SpawnRoomContent(RoomPackage roomPackage)
        {
            foreach (PointOfInterest item in roomPackage.pointOfInterests)
            {
                if (roomPackage.hasBeenCleared && item.removeOnCleared)
                    continue;
                
                Instantiate(item.prefab, item.position, Quaternion.identity);
            }
        }

        private Vector3 ComputeNewPosition()
        {
            if (RoomPointsOfInterest.instance != null)
                return RoomPointsOfInterest.instance.GetPointOfInterestPosition();
            else
                return Vector3.zero;
        }
    }
}
