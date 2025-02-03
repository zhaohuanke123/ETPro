using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class HeroConfigCategory : ProtoObject, IMerge
    {
        public static HeroConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, HeroConfig> dict = new Dictionary<int, HeroConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<HeroConfig> list = new List<HeroConfig>();
		
        public HeroConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            HeroConfigCategory s = o as HeroConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            for(int i =0 ;i<list.Count;i++)
            {
                HeroConfig config = list[i];
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public HeroConfig Get(int id)
        {
            this.dict.TryGetValue(id, out HeroConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (HeroConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, HeroConfig> GetAll()
        {
            return this.dict;
        }
        public List<HeroConfig> GetAllList()
        {
            return this.list;
        }
        public HeroConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class HeroConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>英雄名称</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>品阶</summary>
		[ProtoMember(3)]
		public int Lv { get; set; }
		/// <summary>英雄描述</summary>
		[ProtoMember(4)]
		public string Description { get; set; }
		/// <summary>图标路径</summary>
		[ProtoMember(5)]
		public string Icon { get; set; }
		/// <summary>战斗配置Id</summary>
		[ProtoMember(6)]
		public int ChampionId { get; set; }
		/// <summary>购买需要的积分</summary>
		[ProtoMember(7)]
		public int ItemCount { get; set; }

	}
}
