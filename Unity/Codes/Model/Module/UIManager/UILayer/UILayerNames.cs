﻿namespace ET
{
	public enum UILayerNames : byte
	{
		GameBackgroudLayer,//打开会关闭除自身外所有GameBackgroudLayer，BackgroudLayer，GameLayer，NormalLayer层级的窗口
		BackgroudLayer,//打开会关闭除自身外所有GameBackgroudLayer，BackgroudLayer，GameLayer，NormalLayer层级的窗口
		GameLayer,
		SceneLayer,
		NormalLayer,
		TipLayer,
		TopLayer,
	}
}
