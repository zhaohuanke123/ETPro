using System;
using UnityEngine;

namespace ET
{
    [MessageHandler]
    [FriendClassAttribute(typeof(ET.GamePlayComponent))]
    [FriendClassAttribute(typeof(ET.Unit))]
    public class G2C_SyncUnitPosHandler : AMHandler<G2C_SyncUnitPos>
    {
        protected override async void Run(Session session, G2C_SyncUnitPos message)
        {
            Scene zoneScene = session.ZoneScene();
            CurrentScenesComponent currentScenesComponent = zoneScene.GetComponent<CurrentScenesComponent>();
            if (currentScenesComponent.Scene == null)
            {
                Log.Warning("收到同步位置消息, 当前场景为空");
                return;
            }

            UnitComponent unitComponent = currentScenesComponent.Scene.GetComponent<UnitComponent>();
            Unit unit = unitComponent.Get(message.UnitId);
            if (unit == null)
            {
                Log.Warning("收到同步位置消息, unit为空");
                return;
            }

            MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
            Log.Warning("收到同步位置消息:{0}", unit);

            long messageMoveToUnitId = message.MoveToUnitId;
            Unit moveToUnit = unitComponent.Get(messageMoveToUnitId);

            if (moveToUnit == null)
            {
                Log.Error("收到同步位置消息, moveToUnit为空");
                return;
            }
            
            ChampionConfig config = ChampionConfigCategory.Instance.Get(message.ChampionConfigId);
            ActiveSkillConfig activeSkillConfig = ActiveSkillConfigCategory.Instance.Get(message.SkillId);
            Vector3 targetPos = CpMoveHelper.GetTargetPos(unit, moveToUnit, activeSkillConfig.attackRange, config.moveRange);
            ETTask<bool> moveToAsync = moveComponent.MoveToAsync(targetPos, 10);

            await unit.ViewReadTask;
            CharacterControlComponent characterControlComponent = unit.GetComponent<CharacterControlComponent>();
            characterControlComponent.PlayAnim(AnimDefine.Run);
            await moveToAsync;
            characterControlComponent.PlayAnim(AnimDefine.Idle, 0.5f);
        }
    }
}
