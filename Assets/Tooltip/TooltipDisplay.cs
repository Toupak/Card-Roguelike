using BoomLib.Tools;
using CombatLoop.EnergyBar;
using Cursor.Script;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Tooltip
{
    public class TooltipDisplay : MonoBehaviour
    {
        public enum TooltipType
        {
            Spell,
            Passive,
            EnemyIntentions
        }

        [SerializeField] private GameObject titleGameObject;
        [SerializeField] private TextMeshProUGUI titleTextMeshPro;
        
        [Space]
        [SerializeField] private TextMeshProUGUI mainTextMeshPro;

        [Space] 
        [SerializeField] private GameObject iconGameObject;
        [SerializeField] private Image iconImage;
        
        [Space] 
        [SerializeField] private GameObject energyGameObject;
        [SerializeField] private RectTransform energyHolder;
        [SerializeField] private RectTransform energyBackground;
        [SerializeField] private GameObject emptyEnergyPrefab;
        [SerializeField] private GameObject filledEnergyPrefab;
        
        [Space]
        [SerializeField] private Vector2 leftOffset;
        [SerializeField] private Vector2 rightOffset;

        private RectTransform rectTransform;
        private Vector3 velocity;
        private float smoothTime = 0.1f;
        
        private bool isDisplayedOnTheLeft;
        
        private bool isSetup;

        public void Setup()
        {
            rectTransform = GetComponent<RectTransform>();
            transform.localPosition = CursorInfo.instance.currentPosition + (ComputeOffset() * 0.7f);
            
            titleGameObject.SetActive(false);
            iconGameObject.SetActive(false);
            energyGameObject.SetActive(false);
            
            isSetup = true;
        }

        private void Update()
        {
            if (!isSetup)
                return;
            
            FollowCursor();
        }

        private void FollowCursor()
        {
            Vector2 cursorPosition = CursorInfo.instance.currentPosition + ComputeOffset();
            Vector3 clampedPosition = Tools.ClampPositionInScreen(cursorPosition, rectTransform.rect.size);
            
            transform.localPosition = Vector3.SmoothDamp(transform.localPosition, clampedPosition, ref velocity, smoothTime);
        }

        private Vector2 ComputeOffset()
        {
            return isDisplayedOnTheLeft ? leftOffset : rightOffset;
        }

        public void Hide()
        {
            Destroy(gameObject);
        }

        public TooltipDisplay AddTitle(string titleToDisplay)
        {
            titleGameObject.SetActive(true);
            titleTextMeshPro.text = titleToDisplay;
            
            return this;
        }

        public TooltipDisplay AddMainText(string textToDisplay)
        {
            mainTextMeshPro.text = textToDisplay;
            
            return this;
        }

        public TooltipDisplay SetDisplaySide(bool displayOnTheLeft)
        {
            isDisplayedOnTheLeft = displayOnTheLeft;
            transform.localPosition = CursorInfo.instance.currentPosition + (ComputeOffset() * 0.7f);
            return this;
        }

        public TooltipDisplay AddEnergyCost(int energyCostToDisplay)
        {
            int maxEnergy = EnergyController.instance != null ? EnergyController.instance.StartingEnergyCount : 3;
            
            energyGameObject.SetActive(true);
            energyBackground.sizeDelta = new Vector2(45.0f, 118.0f + (35.0f * (maxEnergy - 3)));

            for (int i = 0; i < maxEnergy; i++)
            {
                Instantiate(i < energyCostToDisplay ? filledEnergyPrefab : emptyEnergyPrefab, energyHolder);
            }
            
            return this;
        }

        public TooltipDisplay AddIcon(Sprite iconToDisplay)
        {
            if (iconToDisplay == null)
                return this;
            
            iconGameObject.SetActive(true);
            iconImage.sprite = iconToDisplay;
            
            return this;
        }
    }
}
