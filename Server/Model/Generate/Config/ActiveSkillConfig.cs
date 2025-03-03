using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class ActiveSkillConfigCategory : ProtoObject, IMerge
    {
        public static ActiveSkillConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ActiveSkillConfig> dict = new Dictionary<int, ActiveSkillConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ActiveSkillConfig> list = new List<ActiveSkillConfig>();
		
        public ActiveSkillConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            ActiveSkillConfigCategory s = o as ActiveSkillConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            for(int i =0 ;i<list.Count;i++)
            {
                ActiveSkillConfig config = list[i];
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public ActiveSkillConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ActiveSkillConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (ActiveSkillConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ActiveSkillConfig> GetAll()
        {
            return this.dict;
        }
        public List<ActiveSkillConfig> GetAllList()
        {
            return this.list;
        }
        public ActiveSkillConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ActiveSkillConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>技能类型，攻击 0 或者治疗 1</summary>
		[ProtoMember(2)]
		public int SkillType { get; set; }
		/// <summary>攻击动画时间（ms）</summary>
		[ProtoMember(3)]
		public long attacktime { get; set; }
		/// <summary>攻击总动画时间（ms）</summary>
		[ProtoMember(4)]
		public long allAttacktime { get; set; }
		/// <summary>攻击范围</summary>
		[ProtoMember(5)]
		public int attackRange { get; set; }
		[BsonRepresentation(MongoDB.Bson.BsonType.Double, AllowTruncation = true)]
		/// <summary>飞行速度</summary>
		[ProtoMember(6)]
		public float projSpeed { get; set; }
		/// <summary>技能倍率 多少倍ATK</summary>
		[ProtoMember(7)]
		public int multiplyValue { get; set; }
		/// <summary>目标数量</summary>
		[ProtoMember(8)]
		public int targetNum { get; set; }
		/// <summary>攻击物体特效</summary>
		[ProtoMember(9)]
		public string projectileEffect { get; set; }
		/// <summary>击中物体特效</summary>
		[ProtoMember(10)]
		public string hitEffect { get; set; }
		/// <summary>是否大招</summary>
		[ProtoMember(11)]
		public int isSuper { get; set; }
		/// <summary>加buff</summary>
		[ProtoMember(12)]
		public int[] addBuffs { get; set; }

	}
}
