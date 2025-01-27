using System.Xml.Schema;

namespace ET
{
	[ObjectSystem]
	public class ChessBattleViewComponentAwakeSystem: AwakeSystem<ChessBattleViewComponent>
	{
		public override void Awake(ChessBattleViewComponent self)
		{
			ChessBattleViewComponent.Instance = self;
			self.ownChampionInventoryArray = new GameObjectComponent[9];
			self.gridChampionsArray = new GameObjectComponent[Map.hexMapSizeX, Map.hexMapSizeZ / 2];
		}
	}

	[ObjectSystem]
	public class ChessBattleViewComponentDestroySystem: DestroySystem<ChessBattleViewComponent>
	{
		public override void Destroy(ChessBattleViewComponent self)
		{
		}
	}

	[FriendClass(typeof (ChessBattleViewComponent))]
	public static partial class ChessBattleViewComponentSystem
	{
		// public static void Test(this ChessBattleViewComponent self)
		// {
		// }

		public static void Replace(this ChessBattleViewComponent self, GameObjectComponent showView, int index)
		{
			ref GameObjectComponent gameObjectComponent = ref self.ownChampionInventoryArray[index];
			if (gameObjectComponent != null)
			{
				gameObjectComponent.Dispose();
			}

			gameObjectComponent = showView;
		}

		public static void Clear(this ChessBattleViewComponent self)
		{
			for (int i = 0; i < self.ownChampionInventoryArray.Length; i++)
			{
				GameObjectComponent goComponent = self.ownChampionInventoryArray[i];
				if (goComponent != null)
				{
					goComponent.Dispose();
				}

				self.ownChampionInventoryArray[i] = null;
			}
		}

		public static void HideAll(this ChessBattleViewComponent self)
		{
			for (int i = 0; i < self.ownChampionInventoryArray.Length; i++)
			{
				GameObjectComponent goComponent = self.ownChampionInventoryArray[i];
				if (goComponent == null)
				{
					continue;
				}

				goComponent.GameObject.SetActive(false);
			}
			foreach (GameObjectComponent gameObjectComponent in self.gridChampionsArray)
			{
				if (gameObjectComponent == null)
				{
					continue;
				}

				gameObjectComponent.GameObject.SetActive(false);
			}
		}

		public static void StartDrag(this ChessBattleViewComponent self, TriggerInfo triggerInfo)
		{
			if (triggerInfo != null)
			{
				self.dragStartTrigger = triggerInfo;

				GameObjectComponent championGO = self.GetChampionFromTriggerInfo(triggerInfo);

				if (championGO != null)
				{
					Map.Instance.ShowIndicators();

					self.draggedChampion = championGO;

					ChampionControllerComponent championControllerComponent = championGO.GetComponent<ChampionControllerComponent>();
					championControllerComponent.SetDrag(true);
				}
			}
		}

		public static void EndDrag(this ChessBattleViewComponent self, TriggerInfo triggerInfo)
		{
			Map.Instance.HideIndicators();

			if (self.draggedChampion != null)
			{
				self.draggedChampion.GetComponent<ChampionControllerComponent>().SetDrag(false);

				if (triggerInfo != null)
				{
					if (self.dragStartTrigger.Equals(triggerInfo))
					{
						return;
					}

					GameObjectComponent currentChampion = self.GetChampionFromTriggerInfo(triggerInfo);

					if (currentChampion != null)
					{
						self.StoreChampionInArray(self.dragStartTrigger.gridType,
						self.dragStartTrigger.gridX,
						self.dragStartTrigger.gridZ,
						currentChampion);
						self.StoreChampionInArray(triggerInfo.gridType, triggerInfo.gridX, triggerInfo.gridZ, self.draggedChampion);

						ChessBattleHelper.SendDragMessage(self.ZoneScene(), self.dragStartTrigger, triggerInfo).Coroutine();
					}
					else
					{
						if (triggerInfo.gridType == GPDefine.GridTypeMap)
						{
							// if (championsOnField < currentChampionLimit || dragStartTrigger.gridType == Map.GRIDTYPE_HEXA_MAP)
							// if (true || self.dragStartTrigger.gridType == Map.GRIDTYPE_HEXA_MAP)
							// {
							self.RemoveChampionFromArray(self.dragStartTrigger.gridType,
							self.dragStartTrigger.gridX,
							self.dragStartTrigger.gridZ);
							self.StoreChampionInArray(triggerInfo.gridType, triggerInfo.gridX, triggerInfo.gridZ, self.draggedChampion);

							// if (dragStartTrigger.gridType != Map.GRIDTYPE_HEXA_MAP)
							//     championsOnField++;

							ChessBattleHelper.SendDragMessage(self.ZoneScene(), self.dragStartTrigger, triggerInfo).Coroutine();
							// }
						}
						else if (triggerInfo.gridType == GPDefine.GridTypeOwnInventory)
						{
							self.RemoveChampionFromArray(self.dragStartTrigger.gridType, self.dragStartTrigger.gridX, self.dragStartTrigger.gridZ);

							self.StoreChampionInArray(triggerInfo.gridType, triggerInfo.gridX, triggerInfo.gridZ, self.draggedChampion);

							// if (dragStartTrigger.gridType == Map.GRIDTYPE_HEXA_MAP)
							//     championsOnField--;

							ChessBattleHelper.SendDragMessage(self.ZoneScene(), self.dragStartTrigger, triggerInfo).Coroutine();
						}
					}
				}
			}

			self.draggedChampion = null;
		}

		public static GameObjectComponent GetChampionFromTriggerInfo(this ChessBattleViewComponent self, TriggerInfo triggerInfo)
		{
			if (triggerInfo.gridType == GPDefine.GridTypeOwnInventory)
			{
				return self.ownChampionInventoryArray[triggerInfo.gridX];
			}

			if (triggerInfo.gridType == GPDefine.GridTypeMap)
			{
				return self.gridChampionsArray[triggerInfo.gridX, triggerInfo.gridZ];
			}

			Log.Error($"GetChampionFromTriggerInfo error : {triggerInfo}");
			return null;
		}

		public static void StoreChampionInArray(this ChessBattleViewComponent self, int gridType, int gridX, int gridZ, GameObjectComponent champion)
		{
			var championController = champion.GetComponent<ChampionControllerComponent>();
			championController.SetGridPosition(gridType, gridX, gridZ);
			// Log.Warning($"StoreChampionInArray : {gridType} {gridX} {gridZ}");

			if (gridType == GPDefine.GridTypeOwnInventory)
			{
				self.ownChampionInventoryArray[gridX] = champion;
			}
			else if (gridType == GPDefine.GridTypeMap)
			{
				self.gridChampionsArray[gridX, gridZ] = champion;
			}
		}

		public static void RemoveChampionFromArray(this ChessBattleViewComponent self, int type, int gridX, int gridZ)
		{
			if (type == GPDefine.GridTypeOwnInventory)
			{
				self.ownChampionInventoryArray[gridX] = null;
			}
			else if (type == GPDefine.GridTypeMap)
			{
				self.gridChampionsArray[gridX, gridZ] = null;
			}
		}
	}
}
