using System;

namespace Save_System
{
    [Serializable]
    public class SaveData
    {
        #region STATS
        
        public DateTime firstTimePlayed;
        public DateTime lastTimePlayed;
        public float totalPlayTimeInSeconds;
        
        public int totalRunCount;
        public int totalRunWinCount;
        
        public int totalBattleCount;
        public int totalKillCount;
        public int totalDamageDealt;//TODO
        public int totalDamageReceived;//TODO
        public int highestDamageInOneAttack;//TODO
        public int highestTokenCountInOneTurn;//TODO
        
        //public string mostPlayedCharacter;
        //public string mostPlayedCard;
        //public string cardWithBestWinRate;

        public int totalBoosterOpenedCount;//TODO
        
        #endregion

        public SaveData()
        {
            firstTimePlayed = DateTime.Now;
        }
    }
}
