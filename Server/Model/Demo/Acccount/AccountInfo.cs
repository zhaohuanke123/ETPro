namespace ET
{
    public enum AccountType
    {
        General = 0,
        BlackList = 1,
    }

    [ChildOf(typeof (AccountInfoComponent))]
    public class AccountInfo: Entity, IAwake
    {
        //用户名
        public string Account { get; set; }

        //密码
        public string Password { get; set; }

        public long CreateTime { get; set; }

        public int AccountType { get; set; }
    }
}