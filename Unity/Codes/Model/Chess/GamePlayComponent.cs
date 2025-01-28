using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
	public static class GPDefine
	{
		public static readonly int GridTypeOwnInventory = 0;
		public static readonly int GridTypeMap = 2;

		public static readonly int TeamId_Player = 0;
		public static readonly int TeamId_AI = 1;
		public static readonly int InventorySize = 9;
		public static readonly int HexMapSizeX = 7;
		public static readonly int HexMapSizeZ = 8;
		public const int InitChampionLimit = 3;
		public const int InitGold = 10;
	}

	public enum Camp
	{
		None,
		Player1,
		Player2,
	}

	public enum GameStage
	{
		BeforeGame,
		Preparation,
		Combat,
		GameOver,
	}

	public struct UnitState
	{
		public Camp camp;
		public ChampionInfo championInfo;
	}

	[ComponentOf]
	public class GamePlayComponent: Entity, IAwake, IDestroy, IFixedUpdate
	// ,IFixedUpdate
	{
		[BsonIgnore]
		public GameStage currentGameStage;
		[BsonIgnore]
		public int CombatRound = 1;

#if SERVER
		[BsonIgnore]
		public Dictionary<Player, List<Unit>> playerChampionDict;

		[BsonIgnore]
		public Dictionary<long, bool> playerReadyDict;

		[BsonIgnore]
		public List<Unit> player1Units;

		[BsonIgnore]
		public List<ChampionInfo> player1ChampionInfos;

		[BsonIgnore]
		public List<Unit> player2Units;

		[BsonIgnore]
		public List<ChampionInfo> player2ChampionInfos;

		[BsonIgnore]
		public LinkedList<Unit> combatQueue;

		[BsonIgnore]
		public Dictionary<Unit, UnitState> unitStateDict;
#endif
		/// <summary>
		/// 计时 
		/// </summary>
		[BsonIgnore]
		public long timer = 0;

		[BsonIgnore]
		public long preTime = 0;

		[BsonIgnore]
		public long PreparationStageDuration = 12 * 1000;
	}
}
