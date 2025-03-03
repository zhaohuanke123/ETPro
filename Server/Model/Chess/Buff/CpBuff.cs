using MongoDB.Bson.Serialization.Attributes;

namespace ET
{
    public enum BuffType
    {
        Attr = 1,
        Control,
        Damage,
    }

    [ChildOf(typeof (CpBuffComponent))]
    public class CpBuff: Entity, IAwake<int>, IDestroy
    {
        public int ConfigId;

        [BsonIgnore]
        public BuffConfig Config
        {
            get => BuffConfigCategory.Instance.Get(ConfigId);
        }

        public int time;

        public BuffType BuffType => (BuffType)this.Config.Type[0];
    }
}