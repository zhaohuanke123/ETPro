namespace ET.Chess
{
	[ObjectSystem]
	public class GamePlayAwakeSystem: AwakeSystem<GamePlayComponent>
	{
		public override void Awake(GamePlayComponent self)
		{
			self.currentGameStage = GameStage.Preparation;
			// self.ownChampionInventoryArray = new GameObject[Map.inventorySize];
			// self.oponentChampionInventoryArray = new GameObject[Map.inventorySize];
			// self.gridChampionsArray = new GameObject[Map.hexMapSizeX, Map.hexMapSizeZ / 2];
		}
	}

	[ObjectSystem]
	public class GamePlayUpdateSystem: UpdateSystem<GamePlayComponent>
	{
		public override void Update(GamePlayComponent self)
		{
			if (self.currentGameStage == GameStage.Preparation)
			{
				// self.timer += Time.deltaTime;
				self.timerDisplay = (int)(self.PreparationStageDuration - self.timer);

				if (self.timer > self.PreparationStageDuration)
				{
					self.timer = 0;
					// self.OnGameStageComplate();
				}
			}
			else if (self.currentGameStage == GameStage.Combat)
			{
				// self.timer += Time.deltaTime;
				self.timerDisplay = (int)self.timer;

				if (self.timer > self.CombatStageDuration)
				{
					self.timer = 0;
					// self.OnGameStageComplate();
				}
			}
		}
	}

	[ObjectSystem]
	public class GamePlayDestroySystem: DestroySystem<GamePlayComponent>
	{
		public override void Destroy(GamePlayComponent self)
		{
			// Add cleanup logic here if necessary.
		}
	}

	[FriendClassAttribute(typeof (ET.GamePlayComponent))]
	public static partial class GamePlayComponentSystemSystem
	{
		public static void OnGameStageComplate(this GamePlayComponent self)
		{
			if (self.currentGameStage == GameStage.Preparation)
			{
				self.currentGameStage = GameStage.Combat;
				// Trigger logic for combat stage
				self.StartCombat();
			}
			else if (self.currentGameStage == GameStage.Combat)
			{
				self.currentGameStage = GameStage.Preparation;
				// Trigger logic for preparation stage
				self.StartPreparation();
			}
		}

		private static void StartPreparation(this GamePlayComponent self)
		{
			// Logic to reset for preparation stage
			self.ResetChampions();
			self.currentGold += self.CalculateIncome();
			// More logic for preparing the game...
		}

		private static void StartCombat(this GamePlayComponent self)
		{
			// Logic to start combat stage
			// For example, trigger all champions to start combat behavior.
			self.ResetChampionsForCombat();
		}

		private static void ResetChampions(this GamePlayComponent self)
		{
			// Reset champion state at the end of the preparation phase
			// for (int x = 0; x < Map.hexMapSizeX; x++)
			{
				// for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
				{
					// if (self.gridChampionsArray[x, z] != null)
					// {
					//     ChampionController championController = self.gridChampionsArray[x, z].GetComponent<ChampionController>();
					//     championController.Reset();
					// }
				}
			}
		}

		private static void ResetChampionsForCombat(this GamePlayComponent self)
		{
			// Logic to reset champions before combat starts
			// for (int i = 0; i < self.ownChampionInventoryArray.Length; i++)
			{
				// if (self.ownChampionInventoryArray[i] != null)
				{
					// ChampionController championController = self.ownChampionInventoryArray[i].GetComponent<ChampionController>();
					// championController.OnCombatStart();
				}
			}

			// for (int x = 0; x < Map.hexMapSizeX; x++)
			{
				// for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
				{
					// if (self.gridChampionsArray[x, z] != null)
					{
						// ChampionController championController = self.gridChampionsArray[x, z].GetComponent<ChampionController>();
						// championController.OnCombatStart();
					}
				}
			}
		}

		private static int CalculateIncome(this GamePlayComponent self)
		{
			int income = 0;
			int bank = (int)(self.currentGold / 10);
			income += self.baseGoldIncome;
			income += bank;
			return income;
		}

		// public static void BuyChampionFromShop(this GamePlayComponent self, Champion champion)
		// {
		// 	int emptyIndex = -1;
		// 	for (int i = 0; i < self.ownChampionInventoryArray.Length; i++)
		// 	{
		// 		if (self.ownChampionInventoryArray[i] == null)
		// 		{
		// 			emptyIndex = i;
		// 			break;
		// 		}
		// 	}
		//
		// 	if (emptyIndex == -1 || self.currentGold < champion.cost)
		// 		return;
		//
		// GameObject championPrefab = Object.Instantiate(champion.prefab);
		// ChampionController championController = championPrefab.GetComponent<ChampionController>();

		// championController.Init(champion, ChampionController.TEAMID_PLAYER);
		// championController.SetGridPosition(Map.GRIDTYPE_OWN_INVENTORY, emptyIndex, -1);
		// championController.SetWorldPosition();
		// championController.SetWorldRotation();

		// self.StoreChampionInArray(Map.GRIDTYPE_OWN_INVENTORY, emptyIndex, -1, championPrefab);

		// 	self.currentGold -= champion.cost;
		// }

		// public static void StoreChampionInArray(this GamePlayComponent self, int gridType, int gridX, int gridZ, GameObject champion)
		// {
		//     ChampionController championController = champion.GetComponent<ChampionController>();
		//     championController.SetGridPosition(gridType, gridX, gridZ);
		//
		//     if (gridType == Map.GRIDTYPE_OWN_INVENTORY)
		//     {
		//         self.ownChampionInventoryArray[gridX] = champion;
		//     }
		//     else if (gridType == Map.GRIDTYPE_HEXA_MAP)
		//     {
		//         self.gridChampionsArray[gridX, gridZ] = champion;
		//     }
		// }

		public static void RemoveChampionFromArray(this GamePlayComponent self, int type, int gridX, int gridZ)
		{
			// if (type == Map.GRIDTYPE_OWN_INVENTORY)
			// {
			//     self.ownChampionInventoryArray[gridX] = null;
			// }
			// else if (type == Map.GRIDTYPE_HEXA_MAP)
			// {
			//     self.gridChampionsArray[gridX, gridZ] = null;
			// }
		}
	}
}
