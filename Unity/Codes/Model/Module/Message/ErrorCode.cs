namespace ET
{
    public static partial class ErrorCode
    {
        public const int ERR_Success = 0;

        // 1-11004 是SocketError请看SocketError定义
        //-----------------------------------
        // 100000-109999是Core层的错误

        // 110000以下的错误请看ErrorCore.cs

        // 这里配置逻辑层的错误码
        // 110000 - 200000是抛异常的错误
        public const int ERR_AccountOrPasswordError = 110001;

        // 200001以上不抛异常

        // public const int ERR_LoginInfoError = 200003;
        public const int ERR_NetWorkError = 200002; //网络错误
        public const int ERR_LoginInfoIsNull = 200003; //登录信息错误
        public const int ERR_AccountNameFormError = 200004; //登录账号格式错误
        public const int ERR_PasswordFormError = 200005; //登录密码格式，错误
        public const int ERR_AccountInBlackListError = 200006; //账号处于黑名单
        public const int ERR_LoginPasswordError = 200007; //登录密码错误
        public const int ERR_RequestRepeatedly = 200008;
        public const int ERR_LoginInfoEmpty = 200009;
        public const int ERR_AccountNotExistError = 200010;
        public const int ERR_AccountAlreadyRegister = 200011;
        public const int ERR_AccountNotLoggedIn = 200012;
        public const int ERR_NotBindPlayer = 200013;
        public const int ERR_PlayerNotLoggedIn = 200014;
        public const int ERR_NotInMap = 200015;
        public const int ERR_RefreshShopFailed = 200016;
        public const int ArgumentNotRight = 200017;
        public const int GoldNotEnough = 200018;
        public const int ChampionArrayFull = 200019;
        public const int CannotDragChampionToOrFormMapInCombat = 200020;
        public const int ERR_ItemNotFound = 200021;
        public const int ERR_RoomNotFound = 200022;
        public const int ERR_InvalidPosition = 200023;
        public const int ERR_ChampionLimitReached = 200024;
        public const int ERR_ChampionPosNotExist = 200025;
        public const int ERR_HeroAlreadyOwned =  200026;
        public const int ERR_PointNotEnough = 200027;
    }
}