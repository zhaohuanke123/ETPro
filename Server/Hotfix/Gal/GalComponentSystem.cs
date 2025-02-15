using System;

namespace ET
{
    [ObjectSystem]
    public class GalComponentAwakeSystem: AwakeSystem<GalComponent>
    {
        public override void Awake(GalComponent self)
        {
        }
    }

    [ObjectSystem]
    public class GalComponentDestroySystem: DestroySystem<GalComponent>
    {
        public override void Destroy(GalComponent self)
        {
        }
    }

    [FriendClass(typeof (GalComponent))]
    public static partial class GalComponentSystem
    {
        public static int GetNextGalId(this GalComponent self)
        {
            return self.nextGalId;
        }

        public static int PassGal(this GalComponent self)
        {
            int galId = self.nextGalId + 1;
            try
            {
                GalConfigCategory.Instance.Get(galId);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                galId = self.nextGalId;
            }

            self.nextGalId = galId;
            return galId;
        }
    }
}