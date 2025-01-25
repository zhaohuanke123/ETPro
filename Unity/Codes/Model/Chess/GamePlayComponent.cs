using System;
using System.Collections.Generic;

namespace ET
{
    public enum GameStage
    {
        BeforeGame,
        Preparation,
        Combat,
        Loss
    }

    [ComponentOf]
    public class GamePlayComponent: Entity, IAwake, IDestroy, IFixedUpdate
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

#if SERVER
        public Dictionary<Player, List<Unit>> playerChampionDict;
        public Dictionary<long, bool> playerReadyDict;
#endif
        /// <summary>
        /// 计时 
        /// </summary>
        public long timer = 0;

        public long preTime = 0;

        public long PreparationStageDuration = 12 * 1000;
        public int CombatStageDuration = 60;
        public int baseGoldIncome = 5;

        public const int InitChampionLimit = 3;
        public const int InitGold = 10;

        public int currentChampionCount = 0;
        public int currentGold = 5;
        public int currentHP = 100;
        public int timerDisplay = 0;
    }
}