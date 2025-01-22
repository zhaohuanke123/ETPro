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
    public class GamePlayComponent: Entity, IAwake, IDestroy
    // ,IFixedUpdate
    {
        public static readonly int GridTypeOwnInventory = 0;
        public static readonly int GridTypeMap = 2;

        public static readonly int TeamId_Player = 0;
        public static readonly int TeamId_AI = 1;
        public static readonly int InventorySize = 9;
        public static readonly int HexMapSizeX = 7;
        public static readonly int HexMapSizeZ = 8;

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