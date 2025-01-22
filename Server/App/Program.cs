using System;
using System.Threading;
using CommandLine;
using NLog;

namespace ET
{
    internal static class Program
    {
        private static FixedUpdate m_FixedUpdate;

        //打开所有进程 dotnet Server.dll --AppType=Watcher --Console=1
        private static void Main(string[] args)
        {
            WinPeriod.Init();
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => { Log.Error(e.ExceptionObject.ToString()); };

            ETTask.ExceptionHandler += Log.Error;

            // 异步方法全部会回掉到主线程
            SynchronizationContext.SetSynchronizationContext(ThreadSynchronizationContext.Instance);

            try
            {
                Game.EventSystem.Add(typeof (Game).Assembly);
                Game.EventSystem.Add(DllHelper.GetHotfixAssembly());

                ProtobufHelper.Init();
                MongoRegister.Init();

                // 命令行参数
                Options options = null;
                Parser.Default.ParseArguments<Options>(args)
                        .WithNotParsed(error => throw new Exception($"命令行格式错误!"))
                        .WithParsed(o => { options = o; });

                Options.Instance = options;

                Log.ILog = new NLogger(Game.Options.AppType.ToString());
                LogManager.Configuration.Variables["appIdFormat"] = $"{Game.Options.Process:000000}";

                Log.Info($"server start........................ {Game.Scene.Id}");

                Game.EventSystem.Publish(new EventType.AppStart());
                // 注册FixedUpdate，注意必须是在这实例化，否则会因为初始化时间与下一个Tick时间间隔过长而产生追帧操作，造成大量的重复执行后果
                m_FixedUpdate = new FixedUpdate() { UpdateCallback = Game.FixedUpdate };

                while (true)
                {
                    try
                    {
                        Thread.Sleep(1);
                        Game.Update();
                        m_FixedUpdate.Tick();
                        Game.LateUpdate();
                        Game.FrameFinishUpdate();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
    }
}