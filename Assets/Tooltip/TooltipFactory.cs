using System.Collections.Generic;
using System.Linq;
using BoomLib.Tools;
using Combat.Status;
using Combat.Status.Data;
using Localization;
using UnityEngine;

namespace Tooltip
{
    public class TooltipFactory : MonoBehaviour
    {
        [SerializeField] private TooltipDisplay tooltipPrefab;

        [Space]
        [SerializeField] private Transform leftSubTooltipHolder;
        [SerializeField] private Transform rightSubTooltipHolder;
        
        public static TooltipFactory instance;
        
        private TooltipDisplay main;
        private List<StatusData> subs = new List<StatusData>();

        private Vector2 targetPosition;

        private void Awake()
        {
            instance = this;
        }

        public TooltipDisplay CreateTooltip(string title, string body, Vector2 position, StatusData statusData = null, int energyCost = -1)
        {
            main = SpawnTooltip(true, position);
            
            targetPosition = position;
            
            if (statusData != null) //To prevent displaying a sub of a status when displaying said status
                subs.Add(statusData);
            
            if (energyCost >= 0)
                SetupEnergyCost(energyCost);
            
            main.SetupTooltip(title, CheckForIcons(body, true));
            main.SetPosition(position);

            return main;
        }
        
        private void SetupEnergyCost(int energyCost)
        {
            main.GetComponent<TooltipEnergyDisplay>().AddEnergyCost(energyCost);
        }

        private TooltipDisplay SpawnTooltip(bool isMainTooltip, Vector2 position)
        {
            return Instantiate(tooltipPrefab, ComputeTooltipHolder(isMainTooltip, position));
        }

        private Transform ComputeTooltipHolder(bool isMainTooltip, Vector2 position)
        {
            if (isMainTooltip)
                return transform;
            
            return position.x >= 1100.0f ? leftSubTooltipHolder : rightSubTooltipHolder;
        }
        
        private void CreateSubTooltip(StatusData statusData)
        {
            if (statusData == null || subs.Contains(statusData))
                return;

            string title = LocalizationSystem.instance.GetStatusTitle(statusData.localizationKey) + " " + $"<sprite name=\"{statusData.icon.name}\">";
            string body = LocalizationSystem.instance.GetStatusDescription(statusData.localizationKey);
            
            TooltipDisplay sub = SpawnTooltip(false, targetPosition);
            sub.SetupTooltip(title, CheckForIcons(body, false));
            sub.SetAsSubTooltip();
            subs.Add(statusData);
        }
        
        protected string CheckForIcons(string message, bool isMainTooltip)
        {
            string finalString = message;
            string[] chunks = message.Split('$');

            List<StatusData> rules = StatusSystem.instance.statusData.data;
            
            foreach (string chunk in chunks)
            {
                if (rules.Where((r) => r.statusTag.Equals(chunk)).ToList().Count > 0)
                {
                    StatusData rule = rules.First((r) => r.statusTag.Equals(chunk));
                    finalString = finalString.Replace($"${rule.statusTag}$", $"<sprite name=\"{rule.icon.name}\">");
                    
                    if (isMainTooltip)
                        CreateSubTooltip(rule);
                }
            }
            
            return finalString;
        }

        public void Hide()
        {
            targetPosition = Vector2.zero;
            subs = new List<StatusData>();
            
            Destroy(main.gameObject);
            leftSubTooltipHolder.DeleteAllChildren();
            rightSubTooltipHolder.DeleteAllChildren();
        }
    }
}
