using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class OperaComponentAwakeSystem: AwakeSystem<OperaComponent>
    {
        public override void Awake(OperaComponent self)
        {
            self.mapMask = LayerMask.GetMask("Map");
            InputWatcherComponent.Instance.RegisterInputEntity(self);
        }
    }

    [ObjectSystem]
    public class OperaComponentDestroySystem: DestroySystem<OperaComponent>
    {
        public override void Destroy(OperaComponent self)
        {
            InputWatcherComponent.Instance.RemoveInputEntity(self);
        }
    }

    [InputSystem((int)KeyCode.R, InputType.KeyDown, -10000)]
    public class OperaComponentInputSystem_Load: InputSystem<OperaComponent>
    {
        public override void Run(OperaComponent self, int key, int type, ref bool stop)
        {
            CodeLoader.Instance.LoadLogic();
            Game.EventSystem.Add(CodeLoader.Instance.GetHotfixTypes());
            Game.EventSystem.Load();
            Log.Debug("hot reload success!");
        }
    }

    [InputSystem(116, InputType.KeyDown, -10000)]
    public class OperaComponentInputSystem_Transfer: InputSystem<OperaComponent>
    {
        public override void Run(OperaComponent self, int key, int type, ref bool stop)
        {
            // C2M_TransferMap c2MTransferMap = new C2M_TransferMap();
            // self.ZoneScene().RemoveComponent<KeyCodeComponent>();
            // self.ZoneScene().GetComponent<SessionComponent>().Session.Call(c2MTransferMap).Coroutine();
            // self.Dispose();
        }
    }

    [InputSystem((int)KeyCode.Mouse0, InputType.KeyDown)]
    public class OperaComponentInputSystem_Move: InputSystem<OperaComponent>
    {
        public override void Run(OperaComponent self, int key, int type, ref bool stop)
        {
            var unit = self.GetMyUnitFromZoneScene();
            if (unit == null)
            {
                return;
            }

            if (!unit.GetComponent<MoveComponent>().Enable)
            {
                Log.Error("暂时无法移动");
                return;
            }

            UnityEngine.Ray ray = CameraManagerComponent.Instance.MainCamera().ScreenPointToRay(Input.mousePosition);
            UnityEngine.RaycastHit hit;
            if (UnityEngine.Physics.Raycast(ray, out hit, 1000, self.mapMask))
            {
                self.ClickPoint = hit.point;
                self.frameClickMap.X = self.ClickPoint.x;
                self.frameClickMap.Y = self.ClickPoint.y;
                self.frameClickMap.Z = self.ClickPoint.z;
                self.ZoneScene().GetComponent<SessionComponent>().Session.Send(self.frameClickMap);
                unit.GetComponent<CombatUnitComponent>().GetComponent<MoveAndSpellComponent>().Cancel(); //取消施法
            }
        }
    }

    [InputSystem((int)KeyCode.Mouse1, InputType.KeyDown)]
    public class OperaComponentSystem_AddUnit: InputSystem<OperaComponent>
    {
        public override void Run(OperaComponent self, int key, int type, ref bool stop)
        {
            // 1. 获取点击位置
            // 2. 请求生成一个unit
        }
    }

    [FriendClass(typeof (OperaComponent))]
    public static class OperaComponentSystem
    {
    }
}