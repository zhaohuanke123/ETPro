using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class ChampionConfigCategory : ProtoObject, IMerge
    {
        public static ChampionConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ChampionConfig> dict = new Dictionary<int, ChampionConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ChampionConfig> list = new List<ChampionConfig>();
		
        public ChampionConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            ChampionConfigCategory s = o as ChampionConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            for(int i =0 ;i<list.Count;i++)
            {
                ChampionConfig config = list[i];
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public ChampionConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ChampionConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (ChampionConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ChampionConfig> GetAll()
        {
            return this.dict;
        }
        public List<ChampionConfig> GetAllList()
        {
            return this.list;
        }
        public ChampionConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ChampionConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>英雄预制件</summary>
		[ProtoMember(2)]
		public string prefab { get; set; }
		/// <summary>攻击投射物</summary>
		[ProtoMember(3)]
		public string attackProjectile { get; set; }
		/// <summary>英雄名称</summary>
		[ProtoMember(4)]
		public string uiname { get; set; }
		/// <summary>消耗</summary>
		[ProtoMember(5)]
		public int cost { get; set; }
		/// <summary>主羁绊类型</summary>
		[ProtoMember(6)]
		public int type1Id { get; set; }
		/// <summary>副羁绊类型</summary>
		[ProtoMember(7)]
		public int type2Id { get; set; }
		/// <summary>血量</summary>
		[ProtoMember(8)]
		public int health { get; set; }
		/// <summary>攻击力</summary>
		[ProtoMember(9)]
		public int damage { get; set; }
		/// <summary>攻击范围</summary>
		[ProtoMember(10)]
		public int attackRange { get; set; }

	}
}
