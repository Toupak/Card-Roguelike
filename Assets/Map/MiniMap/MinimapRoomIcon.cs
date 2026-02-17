using System.Collections.Generic;
using Map.Rooms;
using UnityEngine;

namespace Map.MiniMap
{
    public class MinimapRoomIcon : MonoBehaviour
    {
        [Space] 
        [SerializeField] private Color currentRoomColor;
        [SerializeField] private Color visitedRoomColor;
        [SerializeField] private Color fogOfWarRoomColor;
        
        [Space]
        [SerializeField] private SpriteRenderer pointOfInterestIconPrefab;
        
        [Space] 
        [SerializeField] private Color pointOfInterestIconColor;
        [SerializeField] private Color pointOfInterestIconClearedColor;
        
        private SpriteRenderer spriteRenderer;
        
        public RoomPackage roomPackage { get; private set; }
        private Dictionary<PointOfInterest, SpriteRenderer> pointOfInterestIcons = new Dictionary<PointOfInterest, SpriteRenderer>();

        public void Setup(RoomPackage package, bool isCurrentRoom)
        {
            this.roomPackage = package;
            spriteRenderer = GetComponent<SpriteRenderer>();
            
            UpdateIcon(isCurrentRoom);
        }

        public void UpdateIcon(bool isCurrentRoom)
        {
            spriteRenderer.color = ComputeRoomColor(isCurrentRoom, roomPackage.hasBeenVisited);
            UpdatePointOfInterestIcons();
        }
        
        private Color ComputeRoomColor(bool isCurrentRoom, bool roomHasBeenVisited)
        {
            if (isCurrentRoom)
                return currentRoomColor;
            
            return roomHasBeenVisited ? visitedRoomColor : fogOfWarRoomColor;
        }
        
        private void UpdatePointOfInterestIcons()
        {
            if (!roomPackage.hasBeenVisited)
                return;
            
            bool isRoomCleared = roomPackage.hasBeenCleared;
            
            foreach (PointOfInterest pointOfInterest in roomPackage.pointOfInterests)
            {
                if (pointOfInterest.icon == null)
                    continue;
                
                bool shouldDisplayIcon = !pointOfInterest.removeOnCleared || !isRoomCleared;

                if (!shouldDisplayIcon && pointOfInterestIcons.ContainsKey(pointOfInterest))
                {
                    Destroy(pointOfInterestIcons[pointOfInterest].gameObject);
                    pointOfInterestIcons.Remove(pointOfInterest);
                }
                
                if (shouldDisplayIcon && !pointOfInterestIcons.ContainsKey(pointOfInterest))
                    pointOfInterestIcons.Add(pointOfInterest, SpawnNewPointOfInterestIcon(pointOfInterest));
                
                if (shouldDisplayIcon)
                    pointOfInterestIcons[pointOfInterest].color = ComputePointOfInterestColor(isRoomCleared);
            }
        }

        private SpriteRenderer SpawnNewPointOfInterestIcon(PointOfInterest pointOfInterest)
        {
            Vector3 position = transform.position;
            SpriteRenderer sr = Instantiate(pointOfInterestIconPrefab, position, Quaternion.identity, transform);

            sr.sprite = pointOfInterest.icon;
            return sr;
        }

        private Color ComputePointOfInterestColor(bool isRoomCleared)
        {
            return isRoomCleared ? pointOfInterestIconClearedColor : pointOfInterestIconColor;
        }
    }
}
