using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class PassiveSkillConfigCategory : ProtoObject, IMerge
    {
        public static PassiveSkillConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, PassiveSkillConfig> dict = new Dictionary<int, PassiveSkillConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<PassiveSkillConfig> list = new List<PassiveSkillConfig>();
		
        public PassiveSkillConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            PassiveSkillConfigCategory s = o as PassiveSkillConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            for(int i =0 ;i<list.Count;i++)
            {
                PassiveSkillConfig config = list[i];
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public PassiveSkillConfig Get(int id)
        {
            this.dict.TryGetValue(id, out PassiveSkillConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (PassiveSkillConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, PassiveSkillConfig> GetAll()
        {
            return this.dict;
        }
        public List<PassiveSkillConfig> GetAllList()
        {
            return this.list;
        }
        public PassiveSkillConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class PassiveSkillConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>被动属性列表</summary>
		[ProtoMember(2)]
		public string[] attrNames { get; set; }
		/// <summary>被动属性数值</summary>
		[ProtoMember(3)]
		public int[] attrValues { get; set; }

	}
}
