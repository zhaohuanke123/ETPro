namespace ET
{
    public static class PlayerAnimHelper
    {
        public static async ETTask PlayAttack(CharacterControlComponent controlComponent, long attackTime, int isSuper)
        {
            string anim = isSuper == 1? AnimDefine.SAttack : AnimDefine.Attack;
            controlComponent.PlayAnim(anim);

            await TimerComponent.Instance.WaitAsync(attackTime);

            if (!controlComponent.IsDisposed)
            {
                await TimerComponent.Instance.WaitAsync((long)(controlComponent.GetAnimTime(anim) * 1000) - attackTime);
                controlComponent.PlayAnim(AnimDefine.Idle, 0.1f);
            }
        }
    }
}