using System;
using System.Collections.Generic;
using System.Linq;
using BoomLib.Tools;
using Localization;
using Status;
using Status.Data;
using UnityEngine;
using UnityEngine.UI;

namespace Tooltip
{
    public class MultiTooltipDisplay : MonoBehaviour
    {
        [SerializeField] private TooltipDisplay tooltipPrefab;
        
        private RectTransform rectTransform;

        private TooltipDisplay main;
        private List<StatusData> subs = new List<StatusData>();

        private Vector2 targetPosition;

        public void SetupTooltip(string title, string body, Vector2 position, StatusData statusData = null)
        {
            main = SpawnTooltip();
            
            if (statusData != null) //To prevent displaying a sub of a status when displaying said status
                subs.Add(statusData);
            
            main.SetupTooltip(title, CheckForIcons(body, true));

            targetPosition = position;
            SetPosition(targetPosition);
        }

        private void Update()
        {
            SetPosition(targetPosition);
        }

        public void SetupEnergyCost(int energyCost)
        {
            main.GetComponent<TooltipEnergyDisplay>().AddEnergyCost(energyCost);
        }
        
        private void SetPosition(Vector2 position)
        {
            rectTransform = GetComponent<RectTransform>();
            rectTransform.position = Tools.ClampPositionInScreen(position, rectTransform.rect.size);
        }
        
        private TooltipDisplay SpawnTooltip()
        {
            return Instantiate(tooltipPrefab, transform);
        }
        
        private void CreateSubTooltip(StatusData statusData)
        {
            if (statusData == null || subs.Contains(statusData))
                return;

            string title = LocalizationSystem.instance.GetStatusTitle(statusData.localizationKey) + " " + $"<sprite name=\"{statusData.icon.name}\">";
            string body = LocalizationSystem.instance.GetStatusDescription(statusData.localizationKey);
            
            TooltipDisplay sub = SpawnTooltip();
            sub.SetupTooltip(title, CheckForIcons(body, false));
            sub.SetAsSubTooltip();
            subs.Add(statusData);
        }
        
        protected string CheckForIcons(string message, bool isMainTooltip)
        {
            string finalString = message;
            string[] chunks = message.Split('$');

            List<StatusData> rules = StatusSystem.instance.statusData;
            
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
            Destroy(gameObject);
        }
    }
}
