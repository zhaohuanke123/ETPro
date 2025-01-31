using System;

namespace ET
{
	[FriendClassAttribute(typeof (ET.ChampionInfo))]
	[FriendClassAttribute(typeof (ET.GamePlayComponent))]
	public class C2G_DragChampionHandler: AMRpcHandler<C2G_DragChampion, G2C_DragChampion>
	{
		protected override async ETTask Run(Session session, C2G_DragChampion request, G2C_DragChampion response, Action reply)
		{
			// 添加位置合法性检查
			if (!IsValidPosition(request.NewGridPositionX, request.NewGridPositionZ, request.NewGridType) ||
			    !IsValidPosition(request.OldGridPositionX, request.OldGridPositionZ, request.OldGridType))
			{
				response.Error = ErrorCode.ERR_InvalidPosition;
				reply();
				return;
			}

			Player player = session.GetComponent<SessionPlayerComponent>().GetMyPlayer();
			GamePlayComponent gamePlayComponent = session.GetComponent<GamePlayComponent>();
			if (gamePlayComponent == null)
			{
				Room room = RoomComponent.Instance.GetRoom(player.RoomId);
				if (room == null)
				{
					throw new ArgumentException("room is null");
				}
				gamePlayComponent = room.GetComponent<GamePlayComponent>();
			}

			if (gamePlayComponent.currentGameStage != GameStage.Preparation)
			{
				if (request.OldGridType == GPDefine.GridTypeMap || request.NewGridType == GPDefine.GridTypeMap)
				{
					response.Error = ErrorCode.CannotDragChampionToOrFormMapInCombat;
					reply();
					return;
				}
			}

			ShopComponent shopComponent = gamePlayComponent.GetComponent<ShopComponent>();
			ChampionArrayComponent championArrayComponent = gamePlayComponent.GetComponent<ChampionArrayComponent>();
			ChampionMapArrayComponent mapArrayComponent = gamePlayComponent.GetComponent<ChampionMapArrayComponent>();

			// 在拖拽处理中添加英雄数量限制检查
			// if (request.OldGridType == GPDefine.GridTypeOwnInventory &&
			//     request.NewGridType == GPDefine.GridTypeMap)
			// {
			// 	int championsOnMap = mapArrayComponent.GetChampionInfos(player).Count;
			// 	int championLimit = shopComponent.GetPlayerChampionMaxLimit(player.Id);
			//
			// 	if (championsOnMap >= championLimit)
			// 	{
			// 		response.Error = ErrorCode.ERR_ChampionLimitReached;
			// 		reply();
			// 		return;
			// 	}
			// }

			if (request.OldGridType == GPDefine.GridTypeOwnInventory)
			{
				ChampionInfo championInfo = championArrayComponent.RemoveFromArray(player, request.OldGridPositionX);
				if (championInfo == null)
				{
					response.Error = ErrorCode.ERR_ChampionPosNotExist;
					reply();
					return;
				}

				// 从仓库拉到地图
				if (request.NewGridType == GPDefine.GridTypeMap)
				{
					ChampionInfo toChampionInfo = mapArrayComponent.RemoveFromGrid(player, request.NewGridPositionX, request.NewGridPositionZ);
					if (toChampionInfo != null)
					{
						championArrayComponent.AddToArray(player, toChampionInfo, request.OldGridPositionX);
					}
					else
					{
						int championsOnMap = mapArrayComponent.GetChampionInfos(player).Count;
						int championLimit = shopComponent.GetPlayerChampionMaxLimit(player.Id);
						if (championsOnMap >= championLimit)
						{
							response.Error = ErrorCode.ERR_ChampionLimitReached;
							reply();
							return;
						}
					}

					mapArrayComponent.AddToGrid(player, championInfo, request.NewGridPositionX, request.NewGridPositionZ);
				}
				// 从仓库一个位置到另一个位置
				else
				{
					ChampionInfo toChampionInfo = championArrayComponent.RemoveFromArray(player, request.NewGridPositionX);
					if (toChampionInfo != null)
					{
						championArrayComponent.Replace(player, toChampionInfo, request.OldGridPositionX);
					}

					championArrayComponent.Replace(player, championInfo, request.NewGridPositionX);
				}
			}
			else
			{
				ChampionInfo championInfo = mapArrayComponent.RemoveFromGrid(player, request.OldGridPositionX, request.OldGridPositionZ);
				if (championInfo == null)
				{
					response.Error = ErrorCode.ERR_ChampionPosNotExist;
					reply();
					return;
				}

				// 地图到地图
				if (request.NewGridType == GPDefine.GridTypeMap)
				{
					ChampionInfo toChampionInfo = mapArrayComponent.RemoveFromGrid(player, request.NewGridPositionX, request.NewGridPositionZ);
					if (toChampionInfo != null)
					{
						mapArrayComponent.Replace(player, toChampionInfo, request.OldGridPositionX, request.OldGridPositionZ);
					}

					mapArrayComponent.Replace(player, championInfo, request.NewGridPositionX, request.NewGridPositionZ);
				}
				// 地图到仓库
				else
				{
					ChampionInfo toChampionInfo = championArrayComponent.RemoveFromArray(player, request.NewGridPositionX);
					if (toChampionInfo != null)
					{
						mapArrayComponent.AddToGrid(player, toChampionInfo, request.OldGridPositionX, request.OldGridPositionZ);
					}

					championArrayComponent.AddToArray(player, championInfo, request.NewGridPositionX);
				}
			}

			mapArrayComponent.CalculateBonuses(player);

			reply();
			await ETTask.CompletedTask;
		}

		private bool IsValidPosition(int x, int z, int gridType)
		{
			if (gridType == GPDefine.GridTypeOwnInventory)
			{
				return x >= 0 && x < GPDefine.InventorySize;
			}
			else if (gridType == GPDefine.GridTypeMap)
			{
				return x >= 0 && x < GPDefine.HexMapSizeX &&
						z >= 0 && z < GPDefine.HexMapSizeZ;
			}
			return false;
		}
	}
}
