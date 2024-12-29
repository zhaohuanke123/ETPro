namespace ET
{
    public static class DisConnectHelper
    {
        public static async ETTask DisConnect(this Session self)
        {
            if (self == null || self.IsDisposed)
            {
                return;
            }

            long instanceId = self.InstanceId;

            await TimerComponent.Instance.WaitAsync(2000);

            if (self.InstanceId != instanceId)
            {
                return;
            }

            self.Dispose();
        }
    }
}