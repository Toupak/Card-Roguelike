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
        public int totalDamageDealt;
        public int totalDamageReceived;
        public int highestDamageInOneAttack;
        public int highestTokenCountInOneTurn;
        
        //public string mostPlayedCharacter;
        //public string mostPlayedCard;
        //public string cardWithBestWinRate;

        public int totalBoosterOpenedCount;
        
        #endregion
    }
}
