namespace ET
{
    [ChildOf(typeof (AccountInfoComponent))]
    public class AccountInfo: Entity, IAwake
    {
        //用户名
        public string Account { get; set; }

        //密码
        public string Password { get; set; }
    }
}