namespace ET
{
    public enum AccountType
    {
        General = 0,
        BlackList = 1,
    }

    [ChildOf(typeof (Session))]
    public class AccountInfo: Entity, IAwake
    {
        //用户名
        public string Account { get; set; }

        //密码
        public string Password { get; set; }

        //创建时间
        public long CreateTime { get; set; }

        //账号类型
        public int AccountType { get; set; }
    }
}