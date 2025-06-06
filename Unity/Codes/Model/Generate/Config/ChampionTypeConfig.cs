using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class ChampionTypeConfigCategory : ProtoObject, IMerge
    {
        public static ChampionTypeConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ChampionTypeConfig> dict = new Dictionary<int, ChampionTypeConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ChampionTypeConfig> list = new List<ChampionTypeConfig>();
		
        public ChampionTypeConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            ChampionTypeConfigCategory s = o as ChampionTypeConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            for(int i =0 ;i<list.Count;i++)
            {
                ChampionTypeConfig config = list[i];
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public ChampionTypeConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ChampionTypeConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (ChampionTypeConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ChampionTypeConfig> GetAll()
        {
            return this.dict;
        }
        public List<ChampionTypeConfig> GetAllList()
        {
            return this.list;
        }
        public ChampionTypeConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ChampionTypeConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>羁绊名称</summary>
		[ProtoMember(2)]
		public string displayName { get; set; }
		/// <summary>图标</summary>
		[ProtoMember(3)]
		public string icon { get; set; }
		/// <summary>关联的羁绊效果</summary>
		[ProtoMember(4)]
		public int championBonusId { get; set; }

	}
}
