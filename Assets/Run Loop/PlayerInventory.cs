using System.Collections.Generic;
using Cards.Scripts;
using Inventory.Items.Consumables;
using Inventory.Items.Frames;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Run_Loop
{
    public class FrameItem
    {
        public FrameData data { get; private set; }
        public CardData target;

        public FrameItem(FrameData data, CardData target = null)
        {
            this.data = data;
            this.target = target;
        }
    }
    
    public class PlayerInventory : MonoBehaviour
    {
        public static UnityEvent<FrameItem> OnEquipFrame = new UnityEvent<FrameItem>();
        public static UnityEvent<FrameItem, Vector3> OnUnEquipFrame = new UnityEvent<FrameItem, Vector3>();
        
        public static PlayerInventory instance;

        public List<FrameItem> frames { get; private set; } = new List<FrameItem>();
        public Dictionary<ConsumableData, int> consumables { get; private set; } = new Dictionary<ConsumableData, int>();

        public bool isEmpty => frames.Count < 1 && consumables.Count < 1;
        
        
        [SerializeField] private List<ConsumableData> debugConsumables = new List<ConsumableData>();
        
        [Space]
        [SerializeField] private GameObject moneyParentGameObject;
        [SerializeField] private TextMeshProUGUI moneyAmountDisplayBox;

        public int money { get; private set; }
        
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            LoadDebugInventory();
            moneyParentGameObject.SetActive(false);
            SceneLoader.OnLoadScene.AddListener(ToggleMoneyDisplay);
        }
        private void OnDestroy()
        {
            SceneLoader.OnLoadScene.RemoveListener(ToggleMoneyDisplay);
        }

        private void LoadDebugInventory()
        {
            if (SceneManager.GetActiveScene().name != "CombatScene" || RunLoop.instance.isInRun)
                return;
            
            foreach (FrameData frame in RunLoop.instance.FrameDatabase.frames)
            {
                frames.Add(new FrameItem(frame));
            }

            foreach (ConsumableData data in debugConsumables)
            {
                LootConsumable(data, 1);
            }
        }

        public void LootFrame(FrameData newFrame)
        {
            frames.Add(new FrameItem(newFrame));
        }

        public void EquipFrame(FrameData newFrame, CardData target)
        {
            foreach (FrameItem frameItem in frames)
            {
                if (frameItem.data == newFrame)
                {
                    frameItem.target = target;
                    OnEquipFrame?.Invoke(frameItem);
                    return;
                }
            }
        }

        public void UnEquipFrame(FrameData target, Vector3 position)
        {
            foreach (FrameItem frameItem in frames)
            {
                if (frameItem.data == target)
                {
                    frameItem.target = null;
                    OnUnEquipFrame?.Invoke(frameItem, position);
                    return;
                }
            }
        }

        public void LootConsumable(ConsumableData consumableData, int amount)
        {
            if (consumables.ContainsKey(consumableData))
                consumables[consumableData] += amount;
            else
                consumables.Add(consumableData, amount);
        }

        public void AddMoney(int amount)
        {
            money += amount;
            UpdateMoneyDisplay();
        }

        public void ResetMoney()
        {
            money = 0;
            UpdateMoneyDisplay();
        }

        private void UpdateMoneyDisplay()
        {
            moneyAmountDisplayBox.text = money.ToString();
        }

        private void ToggleMoneyDisplay(string sceneName)
        {
//Only works with the current naming convention : exploration rooms start with a digit and other scenes don't
            bool isExplorationScene = !string.IsNullOrEmpty(sceneName) && char.IsDigit(sceneName[0]);

            moneyParentGameObject.SetActive(isExplorationScene);
        }
    }
}
