using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MongoDB.Bson;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class MapComponentAwakeSystem: AwakeSystem<MapComponent>
    {
        public override void Awake(MapComponent self)
        {
            MapComponent.Instance = self;
#if !SERVER
            self.mapGridPositions = Map.Instance.mapGridPositions;
            self.ownInventoryGridPositions = Map.Instance.ownInventoryGridPositions;
#else
            var mapData = ConfigComponent.Instance.ConfigLoader.GetMapData();

            self.mapGridPositions = new Vector3[GPDefine.HexMapSizeX, GPDefine.HexMapSizeZ];
            var map = JsonHelper.FromJson<List<List<Vector3>>>(mapData[0]);
            for (int i = 0; i < map.Count; i++)
            {
                for (int j = 0; j < map[i].Count; j++)
                {
                    self.mapGridPositions[i, j] = map[i][j];
                }
            }
#endif
        }
    }

    [ObjectSystem]
    public class MapComponentDestroySystem: DestroySystem<MapComponent>
    {
        public override void Destroy(MapComponent self)
        {
        }
    }

    [FriendClass(typeof (MapComponent))]
    public static partial class MapComponentSystem
    {
        public static Vector3 GetMapPosition(this MapComponent self, int x, int y)
        {
            return self.mapGridPositions[x, y];
        }
    }
}