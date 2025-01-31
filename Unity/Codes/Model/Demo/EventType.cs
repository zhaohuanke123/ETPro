﻿using System.Collections.Generic;
using UnityEngine;

namespace ET
{
	namespace EventType
	{
		public struct AppStart
		{
		}

		public struct SceneChangeStart
		{
			public Scene ZoneScene;
			public string Name;
		}

		public struct SceneChangeFinish
		{
			public Scene ZoneScene;
			public Scene CurrentScene;
		}

		public struct SceneChangeLoginBegin
		{
		}

		public struct SceneChangeLoginFinish
		{
		}

		public struct SceneChangeChessMapStart
		{
			public Scene ZoneScene;
			public string Name;
		}

		public struct SceneChangeChessMapFinish
		{
			public Scene ZoneScene;
			public Scene CurrentScene;
		}

		public struct CPChangePosition
		{
			public Unit Unit;
		}

		public struct CPChangeRotate
		{
			public Unit Unit;
		}

		public class ChangePosition: DisposeObject
		{
			public static readonly ChangePosition Instance = new ChangePosition();

			public Unit Unit;
			public Vector3 OldPos = new Vector3();

			// 因为是重复利用的，所以用完PublishClass会调用Dispose
			public override void Dispose()
			{
				this.Unit = null;
			}
		}

		public class ChangeRotation: DisposeObject
		{
			public static readonly ChangeRotation Instance = new ChangeRotation();

			public Unit Unit;

			// 因为是重复利用的，所以用完PublishClass会调用Dispose
			public override void Dispose()
			{
				this.Unit = null;
			}
		}

		public struct PingChange
		{
			public Scene ZoneScene;
			public long Ping;
		}

		public struct AfterCreateZoneScene
		{
			public Scene ZoneScene;
		}

		public struct AfterCreateCurrentScene
		{
			public Scene CurrentScene;
		}

		public struct AfterCreateLoginScene
		{
			public Scene LoginScene;
		}

		public struct AppStartInitFinish
		{
			public Scene ZoneScene;
		}

		public struct LoginFinish
		{
			public Scene ZoneScene;
			public string Account;
		}

		public struct LoadingBegin
		{
			public Scene Scene;
		}

		public struct LoadingFinish
		{
			public Scene Scene;
		}

		public struct EnterMapFinish
		{
			public Scene ZoneScene;
		}

		public struct AfterUnitCreate
		{
			public Unit Unit;
		}

		public struct MoveStart
		{
			public Unit Unit;
		}

		public struct MoveStop
		{
			public Unit Unit;
		}

		public struct PlaySound
		{
			public string Path;
		}

		public struct AddEffect
		{
			public int EffectId;
			public Unit Unit;
			public Entity Parent;
		}

		public struct GenChampions
		{
			public Scene zoneScene;
			public List<ChampionInfoPB> CPInfos;
			public Unit unit;
		}

		public struct GenChampionView
		{
			public Unit unit;
			public ChampionInfoPB ChampionInfoPb;
		}

		public struct OneCpBattleEnd
		{
			public Scene zoneScene;
		}
	}
}
