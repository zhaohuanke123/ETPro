using System.Xml.Schema;
using ET.UIEventType;
using UnityEngine;

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

		public static void HideAllInMap(this ChessBattleViewComponent self)
		{
			foreach (GameObjectComponent gameObjectComponent in self.gridChampionsArray)
			{
				if (gameObjectComponent == null)
				{
					continue;
				}

				gameObjectComponent.GameObject.SetActive(false);
			}
		}

		public static void ShowAllInMap(this ChessBattleViewComponent self)
		{
			foreach (GameObjectComponent gameObjectComponent in self.gridChampionsArray)
			{
				if (gameObjectComponent == null)
				{
					continue;
				}

				gameObjectComponent.GameObject.SetActive(true);
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

		public static void EndDrag(this ChessBattleViewComponent self, TriggerInfo endTriggerInfo)
		{
			Map.Instance.HideIndicators();

			GameObjectComponent draggedChampion = self.draggedChampion;
			if (draggedChampion != null)
			{
				draggedChampion.GetComponent<ChampionControllerComponent>().SetDrag(false);
				TriggerInfo startTrigger = self.dragStartTrigger;

				if (endTriggerInfo != null)
				{
					if (startTrigger.Equals(endTriggerInfo))
					{
						return;
					}

					GameObjectComponent currentChampion = self.GetChampionFromTriggerInfo(endTriggerInfo);

					// if (startTrigger.gridType == GPDefine.GridTypeOwnInventory && endTriggerInfo.gridType == GPDefine.GridTypeMap)
					// {
					// 	if (currentChampion == null)
					// 	{
					// 		if (self.championsOnMapCount >= GamePlayComponent.Instance.CurrentChampionLimit)
					// 		{
					// 			// 显示错误提示
					// 			Game.EventSystem.PublishAsync(new ShowToast()
					// 			{
					// 				Scene = self.ZoneScene(),
					// 				Text = $"上阵英雄数量已达上限({GamePlayComponent.Instance.CurrentChampionLimit})"
					// 			}).Coroutine();
					//
					// 			// 取消拖拽操作
					// 			self.CancelDrag();
					// 			return;
					// 		}
					// 	}
					// }
					// // 获取目标位置的英雄
					//
					// // 从背包到地图，且目标位置没有英雄时，数量+1
					// if (startTrigger.gridType == GPDefine.GridTypeOwnInventory &&
					//     endTriggerInfo.gridType == GPDefine.GridTypeMap &&
					//     currentChampion == null)
					// {
					// 	self.championsOnMapCount++;
					// }
					// // 从地图到背包，且不是交换（目标位置没有英雄），数量-1
					// else if (startTrigger.gridType == GPDefine.GridTypeMap &&
					//          endTriggerInfo.gridType == GPDefine.GridTypeOwnInventory &&
					//          currentChampion == null)
					// {
					// 	self.championsOnMapCount--;
					// }

					// 发布更新事件

					if (currentChampion != null)
					{
						self.StoreChampionInArray(startTrigger.gridType, startTrigger.gridX, startTrigger.gridZ, currentChampion);
						self.StoreChampionInArray(endTriggerInfo.gridType, endTriggerInfo.gridX, endTriggerInfo.gridZ, draggedChampion);

						ChessBattleHelper.SendDragMessage(self.ZoneScene(), startTrigger, endTriggerInfo).Coroutine();
					}
					else
					{
						if (endTriggerInfo.gridType == GPDefine.GridTypeMap)
						{
							if (startTrigger.gridType != GPDefine.GridTypeMap)
							{
								if (self.championsOnMapCount >= GamePlayComponent.Instance.CurrentChampionLimit)
								{
									Game.EventSystem.PublishAsync(new ShowToast()
									{
										Scene = self.ZoneScene(),
										Text = $"上阵英雄数量已达上限({GamePlayComponent.Instance.CurrentChampionLimit})"
									}).Coroutine();
									self.CancelDrag();
									return;
								}

								self.AddToChampionsOnMapCount(1);
							}

							self.RemoveChampionFromArray(startTrigger.gridType, startTrigger.gridX, startTrigger.gridZ);
							self.StoreChampionInArray(endTriggerInfo.gridType, endTriggerInfo.gridX, endTriggerInfo.gridZ, draggedChampion);
							ChessBattleHelper.SendDragMessage(self.ZoneScene(), startTrigger, endTriggerInfo).Coroutine();
						}
						else if (endTriggerInfo.gridType == GPDefine.GridTypeOwnInventory)
						{
							self.RemoveChampionFromArray(startTrigger.gridType, startTrigger.gridX, startTrigger.gridZ);
							self.StoreChampionInArray(endTriggerInfo.gridType, endTriggerInfo.gridX, endTriggerInfo.gridZ, draggedChampion);

							if (startTrigger.gridType == GPDefine.GridTypeMap)
							{
								self.AddToChampionsOnMapCount(-1);
							}

							ChessBattleHelper.SendDragMessage(self.ZoneScene(), startTrigger, endTriggerInfo).Coroutine();
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

		private static int CountChampionsOnMap(this ChessBattleViewComponent self)
		{
			int count = 0;
			for (int x = 0; x < Map.hexMapSizeX; x++)
			{
				for (int z = 0; z < Map.hexMapSizeZ / 2; z++)
				{
					if (self.gridChampionsArray[x, z] != null)
					{
						count++;
					}
				}
			}
			self.championsOnMapCount = count; // 更新计数器
			return count;
		}

		// 添加取消拖拽的辅助方法
		public static void CancelDrag(this ChessBattleViewComponent self)
		{
			self.draggedChampion = null;
		}

		public static void AddToChampionsOnMapCount(this ChessBattleViewComponent self, int value)
		{
			if (value == 0)
			{
				return;
			}

			self.championsOnMapCount += value;
			Game.EventSystem.Publish(new UpdateChampionLimit
			{
				CurrentCount = self.championsOnMapCount,
				MaxLimit = GamePlayComponent.Instance.CurrentChampionLimit
			});
		}
	}
}
