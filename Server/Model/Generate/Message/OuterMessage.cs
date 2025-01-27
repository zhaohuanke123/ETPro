using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[ResponseType(nameof(M2C_TestResponse))]
	[Message(OuterOpcode.C2M_TestRequest)]
	[ProtoContract]
	public partial class C2M_TestRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string request { get; set; }

	}

	[Message(OuterOpcode.M2C_TestResponse)]
	[ProtoContract]
	public partial class M2C_TestResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string response { get; set; }

	}

	[ResponseType(nameof(Actor_TransferResponse))]
	[Message(OuterOpcode.Actor_TransferRequest)]
	[ProtoContract]
	public partial class Actor_TransferRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int MapIndex { get; set; }

	}

	[Message(OuterOpcode.Actor_TransferResponse)]
	[ProtoContract]
	public partial class Actor_TransferResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2C_EnterMap))]
	[Message(OuterOpcode.C2G_EnterMap)]
	[ProtoContract]
	public partial class C2G_EnterMap: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_EnterMap)]
	[ProtoContract]
	public partial class G2C_EnterMap: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

// 自己unitId
		[ProtoMember(4)]
		public long MyId { get; set; }

	}

	[ResponseType(nameof(G2C_EnterChessMap))]
	[Message(OuterOpcode.C2G_EnterChessMap)]
	[ProtoContract]
	public partial class C2G_EnterChessMap: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_EnterChessMap)]
	[ProtoContract]
	public partial class G2C_EnterChessMap: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public long MyId { get; set; }

		[ProtoMember(5)]
		public long SceneInstanceId { get; set; }

		[ProtoMember(6)]
		public string SceneName { get; set; }

	}

	[ResponseType(nameof(G2C_ExitMap))]
	[Message(OuterOpcode.C2G_ExitMap)]
	[ProtoContract]
	public partial class C2G_ExitMap: Object, IRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_ExitMap)]
	[ProtoContract]
	public partial class G2C_ExitMap: Object, IResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.MoveInfo)]
	[ProtoContract]
	public partial class MoveInfo: Object
	{
		[ProtoMember(1)]
		public List<float> X = new List<float>();

		[ProtoMember(2)]
		public List<float> Y = new List<float>();

		[ProtoMember(3)]
		public List<float> Z = new List<float>();

		[ProtoMember(4)]
		public float A { get; set; }

		[ProtoMember(5)]
		public float B { get; set; }

		[ProtoMember(6)]
		public float C { get; set; }

		[ProtoMember(7)]
		public float W { get; set; }

		[ProtoMember(8)]
		public int TurnSpeed { get; set; }

	}

	[Message(OuterOpcode.UnitInfo)]
	[ProtoContract]
	public partial class UnitInfo: Object
	{
		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public int ConfigId { get; set; }

		[ProtoMember(3)]
		public int Type { get; set; }

		[ProtoMember(4)]
		public float X { get; set; }

		[ProtoMember(5)]
		public float Y { get; set; }

		[ProtoMember(6)]
		public float Z { get; set; }

		[ProtoMember(7)]
		public float ForwardX { get; set; }

		[ProtoMember(8)]
		public float ForwardY { get; set; }

		[ProtoMember(9)]
		public float ForwardZ { get; set; }

		[ProtoMember(10)]
		public List<int> Ks = new List<int>();

		[ProtoMember(11)]
		public List<long> Vs = new List<long>();

		[ProtoMember(12)]
		public MoveInfo MoveInfo { get; set; }

		[ProtoMember(13)]
		public List<int> SkillIds = new List<int>();

		[ProtoMember(14)]
		public List<int> BuffIds = new List<int>();

		[ProtoMember(15)]
		public List<long> BuffTimestamp = new List<long>();

		[ProtoMember(16)]
		public List<long> BuffSourceIds = new List<long>();

	}

	[Message(OuterOpcode.M2C_CreateUnits)]
	[ProtoContract]
	public partial class M2C_CreateUnits: Object, IActorMessage
	{
		[ProtoMember(2)]
		public List<UnitInfo> Units = new List<UnitInfo>();

	}

	[Message(OuterOpcode.M2C_CreateMyUnit)]
	[ProtoContract]
	public partial class M2C_CreateMyUnit: Object, IActorMessage
	{
		[ProtoMember(1)]
		public UnitInfo Unit { get; set; }

		[ProtoMember(2)]
		public List<int> GuidanceDone = new List<int>();

	}

	[Message(OuterOpcode.M2C_StartSceneChange)]
	[ProtoContract]
	public partial class M2C_StartSceneChange: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long SceneInstanceId { get; set; }

		[ProtoMember(2)]
		public string SceneName { get; set; }

	}

	[Message(OuterOpcode.M2C_StartSceneChangeToLogin)]
	[ProtoContract]
	public partial class M2C_StartSceneChangeToLogin: Object, IActorMessage
	{
	}

	[Message(OuterOpcode.M2C_RemoveUnits)]
	[ProtoContract]
	public partial class M2C_RemoveUnits: Object, IActorMessage
	{
		[ProtoMember(2)]
		public List<long> Units = new List<long>();

	}

	[Message(OuterOpcode.C2M_PathfindingResult)]
	[ProtoContract]
	public partial class C2M_PathfindingResult: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public float X { get; set; }

		[ProtoMember(2)]
		public float Y { get; set; }

		[ProtoMember(3)]
		public float Z { get; set; }

	}

	[Message(OuterOpcode.C2M_Stop)]
	[ProtoContract]
	public partial class C2M_Stop: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_PathfindingResult)]
	[ProtoContract]
	public partial class M2C_PathfindingResult: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public float X { get; set; }

		[ProtoMember(3)]
		public float Y { get; set; }

		[ProtoMember(4)]
		public float Z { get; set; }

		[ProtoMember(5)]
		public List<float> Xs = new List<float>();

		[ProtoMember(6)]
		public List<float> Ys = new List<float>();

		[ProtoMember(7)]
		public List<float> Zs = new List<float>();

	}

	[Message(OuterOpcode.M2C_Stop)]
	[ProtoContract]
	public partial class M2C_Stop: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public long Id { get; set; }

		[ProtoMember(3)]
		public float X { get; set; }

		[ProtoMember(4)]
		public float Y { get; set; }

		[ProtoMember(5)]
		public float Z { get; set; }

		[ProtoMember(6)]
		public float A { get; set; }

		[ProtoMember(7)]
		public float B { get; set; }

		[ProtoMember(8)]
		public float C { get; set; }

		[ProtoMember(9)]
		public float W { get; set; }

	}

	[ResponseType(nameof(G2C_Ping))]
	[Message(OuterOpcode.C2G_Ping)]
	[ProtoContract]
	public partial class C2G_Ping: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_Ping)]
	[ProtoContract]
	public partial class G2C_Ping: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long Time { get; set; }

	}

	[Message(OuterOpcode.G2C_Test)]
	[ProtoContract]
	public partial class G2C_Test: Object, IMessage
	{
	}

	[ResponseType(nameof(M2C_Reload))]
	[Message(OuterOpcode.C2M_Reload)]
	[ProtoContract]
	public partial class C2M_Reload: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.M2C_Reload)]
	[ProtoContract]
	public partial class M2C_Reload: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(R2C_Register))]
	[Message(OuterOpcode.C2R_Register)]
	[ProtoContract]
	public partial class C2R_Register: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		///<summary>帐号</summary>
		[ProtoMember(1)]
		public string Account { get; set; }

		///<summary>密码</summary>
		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.R2C_Register)]
	[ProtoContract]
	public partial class R2C_Register: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(R2C_Login))]
	[Message(OuterOpcode.C2R_Login)]
	[ProtoContract]
	public partial class C2R_Login: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		///<summary>帐号</summary>
		[ProtoMember(1)]
		public string Account { get; set; }

		///<summary>密码</summary>
		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.R2C_Login)]
	[ProtoContract]
	public partial class R2C_Login: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string Address { get; set; }

		[ProtoMember(2)]
		public long Key { get; set; }

		[ProtoMember(3)]
		public long GateId { get; set; }

	}

	[ResponseType(nameof(G2C_Logout))]
	[Message(OuterOpcode.C2G_Logout)]
	[ProtoContract]
	public partial class C2G_Logout: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_Logout)]
	[ProtoContract]
	public partial class G2C_Logout: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2C_LoginGate))]
	[Message(OuterOpcode.C2G_LoginGate)]
	[ProtoContract]
	public partial class C2G_LoginGate: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		///<summary>帐号</summary>
		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long GateId { get; set; }

	}

	[Message(OuterOpcode.G2C_LoginGate)]
	[ProtoContract]
	public partial class G2C_LoginGate: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long PlayerId { get; set; }

	}

	[Message(OuterOpcode.G2C_TestHotfixMessage)]
	[ProtoContract]
	public partial class G2C_TestHotfixMessage: Object, IMessage
	{
		[ProtoMember(1)]
		public string Info { get; set; }

	}

	[ResponseType(nameof(M2C_TestRobotCase))]
	[Message(OuterOpcode.C2M_TestRobotCase)]
	[ProtoContract]
	public partial class C2M_TestRobotCase: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int N { get; set; }

	}

	[Message(OuterOpcode.M2C_TestRobotCase)]
	[ProtoContract]
	public partial class M2C_TestRobotCase: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int N { get; set; }

	}

	[ResponseType(nameof(M2C_TransferMap))]
	[Message(OuterOpcode.C2M_TransferMap)]
	[ProtoContract]
	public partial class C2M_TransferMap: Object, IActorLocationRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_TransferMap)]
	[ProtoContract]
	public partial class M2C_TransferMap: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.C2M_UseSkill)]
	[ProtoContract]
	public partial class C2M_UseSkill: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int SkillConfigId { get; set; }

		[ProtoMember(2)]
		public long Id { get; set; }

		[ProtoMember(3)]
		public float X { get; set; }

		[ProtoMember(4)]
		public float Y { get; set; }

		[ProtoMember(5)]
		public float Z { get; set; }

	}

	[Message(OuterOpcode.M2C_UseSkill)]
	[ProtoContract]
	public partial class M2C_UseSkill: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public int SkillConfigId { get; set; }

		[ProtoMember(3)]
		public long Sender { get; set; }

		[ProtoMember(4)]
		public long Reciver { get; set; }

		[ProtoMember(5)]
		public float X { get; set; }

		[ProtoMember(6)]
		public float Y { get; set; }

		[ProtoMember(7)]
		public float Z { get; set; }

	}

	[Message(OuterOpcode.M2C_AddBuff)]
	[ProtoContract]
	public partial class M2C_AddBuff: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public int ConfigId { get; set; }

		[ProtoMember(3)]
		public long Timestamp { get; set; }

		[ProtoMember(4)]
		public long UnitId { get; set; }

		[ProtoMember(5)]
		public long SourceId { get; set; }

	}

	[Message(OuterOpcode.M2C_Damage)]
	[ProtoContract]
	public partial class M2C_Damage: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public long FromId { get; set; }

		[ProtoMember(3)]
		public long ToId { get; set; }

		[ProtoMember(4)]
		public long Damage { get; set; }

		[ProtoMember(5)]
		public long NowBase { get; set; }

	}

	[Message(OuterOpcode.M2C_ChangeSkillGroup)]
	[ProtoContract]
	public partial class M2C_ChangeSkillGroup: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

		[ProtoMember(3)]
		public int Result { get; set; }

		[ProtoMember(4)]
		public long Timestamp { get; set; }

	}

	[Message(OuterOpcode.M2C_RemoveBuff)]
	[ProtoContract]
	public partial class M2C_RemoveBuff: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public int ConfigId { get; set; }

		[ProtoMember(3)]
		public long Timestamp { get; set; }

		[ProtoMember(4)]
		public long UnitId { get; set; }

	}

	[Message(OuterOpcode.M2C_Interrupt)]
	[ProtoContract]
	public partial class M2C_Interrupt: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public int ConfigId { get; set; }

		[ProtoMember(3)]
		public long Timestamp { get; set; }

		[ProtoMember(4)]
		public long UnitId { get; set; }

	}

	[ResponseType(nameof(M2C_TestActotLocationResponse))]
	[Message(OuterOpcode.C2M_TestActorLocationRequest)]
	[ProtoContract]
	public partial class C2M_TestActorLocationRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Connect { get; set; }

	}

	[Message(OuterOpcode.M2C_TestActotLocationResponse)]
	[ProtoContract]
	public partial class M2C_TestActotLocationResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string Content { get; set; }

	}

	[Message(OuterOpcode.C2M_TestActorLocationMessage)]
	[ProtoContract]
	public partial class C2M_TestActorLocationMessage: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Info { get; set; }

	}

	[Message(OuterOpcode.M2C_TestActorMessage)]
	[ProtoContract]
	public partial class M2C_TestActorMessage: Object, IActorMessage
	{
		[ProtoMember(1)]
		public string Contend { get; set; }

	}

	[ResponseType(nameof(A2C_LoginAccount))]
	[Message(OuterOpcode.C2A_LoginAccount)]
	[ProtoContract]
	public partial class C2A_LoginAccount: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string AccountName { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.A2C_LoginAccount)]
	[ProtoContract]
	public partial class A2C_LoginAccount: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string Token { get; set; }

		[ProtoMember(2)]
		public long AccountId { get; set; }

		[ProtoMember(3)]
		public string Address { get; set; }

		[ProtoMember(4)]
		public long Key { get; set; }

		[ProtoMember(5)]
		public long GateId { get; set; }

	}

	[ResponseType(nameof(A2C_RegisterAccount))]
	[Message(OuterOpcode.C2A_RegisterAccount)]
	[ProtoContract]
	public partial class C2A_RegisterAccount: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string AccountName { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.A2C_RegisterAccount)]
	[ProtoContract]
	public partial class A2C_RegisterAccount: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(A2C_LogoutResponse))]
	[Message(OuterOpcode.C2A_LogoutRequest)]
	[ProtoContract]
	public partial class C2A_LogoutRequest: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.A2C_LogoutResponse)]
	[ProtoContract]
	public partial class A2C_LogoutResponse: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.A2C_Disconnect)]
	[ProtoContract]
	public partial class A2C_Disconnect: Object, IMessage
	{
		[ProtoMember(1)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(A2C_CreateChessUnit))]
	[Message(OuterOpcode.C2A_CreateChessUnit)]
	[ProtoContract]
	public partial class C2A_CreateChessUnit: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public float X { get; set; }

		[ProtoMember(2)]
		public float Y { get; set; }

		[ProtoMember(3)]
		public float Z { get; set; }

	}

	[Message(OuterOpcode.A2C_CreateChessUnit)]
	[ProtoContract]
	public partial class A2C_CreateChessUnit: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public UnitInfo Unit { get; set; }

	}

	[ResponseType(nameof(G2C_RefreshShop))]
	[Message(OuterOpcode.C2G_RefreshShop)]
	[ProtoContract]
	public partial class C2G_RefreshShop: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_RefreshGold)]
	[ProtoContract]
	public partial class G2C_RefreshGold: Object, IMessage
	{
		[ProtoMember(1)]
		public int GlodCount { get; set; }

	}

	[Message(OuterOpcode.G2C_RefreshShop)]
	[ProtoContract]
	public partial class G2C_RefreshShop: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<int> championIds = new List<int>();

	}

	[Message(OuterOpcode.C2G_EnterChessMapFinish)]
	[ProtoContract]
	public partial class C2G_EnterChessMapFinish: Object, IMessage
	{
	}

	[ResponseType(nameof(G2C_BuyChampion))]
	[Message(OuterOpcode.C2G_BuyChampion)]
	[ProtoContract]
	public partial class C2G_BuyChampion: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int SlopIndex { get; set; }

	}

	[Message(OuterOpcode.G2C_BuyChampion)]
	[ProtoContract]
	public partial class G2C_BuyChampion: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<ChampionInfoPB> CPInfos = new List<ChampionInfoPB>();

		[ProtoMember(2)]
		public UnitInfo UnitInfo { get; set; }

	}

	[Message(OuterOpcode.ChampionInfoPB)]
	[ProtoContract]
	public partial class ChampionInfoPB: Object
	{
		[ProtoMember(1)]
		public int ConfigId { get; set; }

		[ProtoMember(2)]
		public int GridPositionX { get; set; }

		[ProtoMember(3)]
		public int GridPositionY { get; set; }

		[ProtoMember(4)]
		public GridType Type { get; set; }

		[ProtoMember(5)]
		public int Lv { get; set; }

	}

	[Message(OuterOpcode.G2C_UpdateBonus)]
	[ProtoContract]
	public partial class G2C_UpdateBonus: Object, IMessage
	{
		[ProtoMember(1)]
		public List<int> TypeIdList = new List<int>();

		[ProtoMember(2)]
		public List<int> CountList = new List<int>();

	}

	[ResponseType(nameof(G2C_DragChampion))]
	[Message(OuterOpcode.C2G_DragChampion)]
	[ProtoContract]
	public partial class C2G_DragChampion: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int OldGridType { get; set; }

		[ProtoMember(2)]
		public int OldGridPositionX { get; set; }

		[ProtoMember(3)]
		public int OldGridPositionZ { get; set; }

		[ProtoMember(4)]
		public int NewGridType { get; set; }

		[ProtoMember(5)]
		public int NewGridPositionX { get; set; }

		[ProtoMember(6)]
		public int NewGridPositionZ { get; set; }

	}

	[Message(OuterOpcode.G2C_DragChampion)]
	[ProtoContract]
	public partial class G2C_DragChampion: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.G2C_SyncUnitPos)]
	[ProtoContract]
	public partial class G2C_SyncUnitPos: Object, IMessage
	{
		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public float X { get; set; }

		[ProtoMember(3)]
		public float Y { get; set; }

		[ProtoMember(4)]
		public float Z { get; set; }

		[ProtoMember(5)]
		public float ForwardX { get; set; }

		[ProtoMember(6)]
		public float ForwardY { get; set; }

		[ProtoMember(7)]
		public float ForwardZ { get; set; }

	}

	[Message(OuterOpcode.G2C_SyncTimer)]
	[ProtoContract]
	public partial class G2C_SyncTimer: Object, IMessage
	{
		[ProtoMember(1)]
		public long timer { get; set; }

	}

	[Message(OuterOpcode.G2C_CreateCpUnits)]
	[ProtoContract]
	public partial class G2C_CreateCpUnits: Object, IMessage
	{
		[ProtoMember(1)]
		public List<UnitInfo> Units = new List<UnitInfo>();

		[ProtoMember(2)]
		public List<ChampionInfoPB> ChampionInfoPBList = new List<ChampionInfoPB>();

	}

	[Message(OuterOpcode.C2G_SceneReady)]
	[ProtoContract]
	public partial class C2G_SceneReady: Object, IMessage
	{
	}

	[Message(OuterOpcode.C2G_ExitChessMap)]
	[ProtoContract]
	public partial class C2G_ExitChessMap: Object, IMessage
	{
	}

	[Message(OuterOpcode.G2C_OneCpBattleEnd)]
	[ProtoContract]
	public partial class G2C_OneCpBattleEnd: Object, IMessage
	{
		[ProtoMember(1)]
		public int Result { get; set; }

	}

	[Message(OuterOpcode.G2C_AttackDamage)]
	[ProtoContract]
	public partial class G2C_AttackDamage: Object, IMessage
	{
		[ProtoMember(1)]
		public long FromId { get; set; }

		[ProtoMember(2)]
		public long ToId { get; set; }

		[ProtoMember(3)]
		public int Damage { get; set; }

		[ProtoMember(4)]
		public bool IsDead { get; set; }

		[ProtoMember(5)]
		public long attackTime { get; set; }

	}

}
