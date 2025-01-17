using System.Collections.Generic;

namespace ET
{
    public enum GameStage
    {
        Preparation,
        Combat,
        Loss
    }

    [ComponentOf]
    public class GamePlayComponent: Entity, IAwake, IUpdate, IDestroy
    {
        public GameStage currentGameStage;

        /// <summary>
        /// 计时 
        /// </summary>
        public float timer = 0;

        public int PreparationStageDuration = 16;
        public int CombatStageDuration = 60;
        public int baseGoldIncome = 5;

        public int currentChampionLimit = 3;
        public int currentChampionCount = 0;
        public int currentGold = 5;
        public int currentHP = 100;
        public int timerDisplay = 0;

        public Dictionary<ChampionTypeConfig, int> championTypeCount;

        public List<ChampionBonusConfig> activeBonusList;
        // Other necessary fie
    }
}