namespace ET
{
	[ComponentOf(typeof (Scene))]
	public class BattleGridComponent: Entity, IAwake, IDestroy
	{
		public bool[,] OccupiedGrid; // true表示格子被占用
		public int Width => GPDefine.HexMapSizeX;
		public int Height => GPDefine.HexMapSizeZ;
	}

	public class GridNode
	{
		public int X;
		public int Z;
		public int G; // 从起点到当前点的代价
		public int H; // 从当前点到终点的预估代价
		public int F => G + H; // 总代价
		public GridNode Parent; // 父节点，用于重建路径

		public GridNode(int x, int z)
		{
			X = x;
			Z = z;
			G = 0;
			H = 0;
			Parent = null;
		}
	}
}
