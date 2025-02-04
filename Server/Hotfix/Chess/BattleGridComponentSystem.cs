using System;
using System.Collections.Generic;

namespace ET
{
    [ObjectSystem]
    public class BattleGridComponentAwakeSystem : AwakeSystem<BattleGridComponent>
    {
        public override void Awake(BattleGridComponent self)
        {
            self.OccupiedGrid = new bool[self.Width, self.Height];
        }
    }

    [FriendClass(typeof(BattleGridComponent))]
    public static class BattleGridComponentSystem
    {
        public static void Reset(this BattleGridComponent self)
        {
            for (int x = 0; x < self.Width; x++)
            {
                for (int z = 0; z < self.Height; z++)
                {
                    self.OccupiedGrid[x, z] = false;
                }
            }
        }

        public static void SetOccupied(this BattleGridComponent self, int x, int z, bool occupied)
        {
            if (x >= 0 && x < self.Width && z >= 0 && z < self.Height)
            {
                self.OccupiedGrid[x, z] = occupied;
            }
        }

        public static bool IsOccupied(this BattleGridComponent self, int x, int z)
        {
            if (x >= 0 && x < self.Width && z >= 0 && z < self.Height)
            {
                return self.OccupiedGrid[x, z];
            }
            return true; // 超出边界视为占用
        }

        public static List<(int x, int z)> FindPath(this BattleGridComponent self, int startX, int startZ, int endX, int endZ)
        {
            var openSet = new PriorityQueue<GridNode, int>();
            var closedSet = new HashSet<(int x, int z)>();
            var nodeDict = new Dictionary<(int x, int z), GridNode>();

            var startNode = new GridNode(startX, startZ);
            startNode.H = CalculateHeuristic(startX, startZ, endX, endZ);
            openSet.Enqueue(startNode, startNode.F);
            nodeDict[(startX, startZ)] = startNode;

            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();

                if (current.X == endX && current.Z == endZ)
                {
                    return ReconstructPath(current);
                }

                closedSet.Add((current.X, current.Z));

                // 检查八个方向的相邻格子
                int[] dx = { -1, -1, -1, 0, 0, 1, 1, 1 };
                int[] dz = { -1, 0, 1, -1, 1, -1, 0, 1 };

                for (int i = 0; i < 8; i++)
                {
                    int newX = current.X + dx[i];
                    int newZ = current.Z + dz[i];

                    if (newX < 0 || newX >= self.Width || newZ < 0 || newZ >= self.Height ||
                        self.IsOccupied(newX, newZ) || closedSet.Contains((newX, newZ)))
                    {
                        continue;
                    }

                    int newG = current.G + 1;
                    var neighbor = new GridNode(newX, newZ)
                    {
                        G = newG,
                        H = CalculateHeuristic(newX, newZ, endX, endZ),
                        Parent = current
                    };

                    var key = (newX, newZ);
                    if (nodeDict.TryGetValue(key, out var existingNode))
                    {
                        if (newG < existingNode.G)
                        {
                            nodeDict[key] = neighbor;
                            openSet.Enqueue(neighbor, neighbor.F);
                        }
                    }
                    else
                    {
                        nodeDict[key] = neighbor;
                        openSet.Enqueue(neighbor, neighbor.F);
                    }
                }
            }

            return new List<(int x, int z)>(); // 没找到路径
        }

        private static int CalculateHeuristic(int x1, int z1, int x2, int z2)
        {
            return Math.Abs(x1 - x2) + Math.Abs(z1 - z2);
        }

        private static List<(int x, int z)> ReconstructPath(GridNode endNode)
        {
            var path = new List<(int x, int z)>();
            var current = endNode;

            while (current.Parent != null)
            {
                path.Add((current.X, current.Z));
                current = current.Parent;
            }

            path.Reverse();
            return path;
        }
    }
} 