using System;
using ActionReaction;
using ActionReaction.Game_Actions;
using Run_Loop;
using UnityEngine;

namespace Save_System
{
    public class SaveStatsGatherer : MonoBehaviour
    {
        private void Start()
        {
            RunLoop.OnStartRun.AddListener(() => SaveSystem.instance.SaveData.totalRunCount += 1);
            RunLoop.OnWinRun.AddListener(() => SaveSystem.instance.SaveData.totalRunWinCount += 1);
            RunLoop.OnStartBattle.AddListener(() => SaveSystem.instance.SaveData.totalBattleCount += 1);
        }

        private void OnEnable()
        {
            ActionSystem.SubscribeReaction<DeathGA>(DeathReaction, ReactionTiming.POST);
        }

        private void OnDisable()
        {
            ActionSystem.UnsubscribeReaction<DeathGA>(DeathReaction, ReactionTiming.POST);
        }

        private void DeathReaction(DeathGA deathGa)
        {
            if (deathGa.killer != null && !deathGa.killer.cardData.isEnemy && deathGa.isEnemy)
                SaveSystem.instance.SaveData.totalKillCount += 1;
        }

        private void OnApplicationQuit()
        {
            SaveSystem.instance.SaveData.lastTimePlayed = DateTime.Now;
            SaveSystem.instance.SaveData.totalPlayTimeInSeconds += Time.time;

            SaveSystem.instance.Save();
        }
    }
}
