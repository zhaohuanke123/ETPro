using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class ChampionBonusConfigCategory : ProtoObject, IMerge
    {
        public static ChampionBonusConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ChampionBonusConfig> dict = new Dictionary<int, ChampionBonusConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ChampionBonusConfig> list = new List<ChampionBonusConfig>();
		
        public ChampionBonusConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            ChampionBonusConfigCategory s = o as ChampionBonusConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            for(int i =0 ;i<list.Count;i++)
            {
                ChampionBonusConfig config = list[i];
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public ChampionBonusConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ChampionBonusConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (ChampionBonusConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ChampionBonusConfig> GetAll()
        {
            return this.dict;
        }
        public List<ChampionBonusConfig> GetAllList()
        {
            return this.list;
        }
        public ChampionBonusConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ChampionBonusConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>羁绊触发所需人数</summary>
		[ProtoMember(2)]
		public int championCount { get; set; }
		/// <summary>羁绊效果类型</summary>
		[ProtoMember(3)]
		public int championBonusType { get; set; }
		/// <summary>羁绊效果目标</summary>
		[ProtoMember(4)]
		public int bonusTarget { get; set; }
		/// <summary>羁绊效果数值</summary>
		[ProtoMember(5)]
		public int bonusValue { get; set; }
		[BsonRepresentation(MongoDB.Bson.BsonType.Double, AllowTruncation = true)]
		/// <summary>效果持续时间</summary>
		[ProtoMember(6)]
		public float duration { get; set; }
		/// <summary>效果预制件</summary>
		[ProtoMember(7)]
		public string effectPrefab { get; set; }
		/// <summary>增加x点固定伤害</summary>
		[ProtoMember(8)]
		public int damageAddBonus { get; set; }
		/// <summary>增加%伤害</summary>
		[ProtoMember(9)]
		public int damagePctBonus { get; set; }
		/// <summary>最终增加x点伤害</summary>
		[ProtoMember(10)]
		public int damageFinalAddBonus { get; set; }
		/// <summary>最终增加%伤害</summary>
		[ProtoMember(11)]
		public int damageFinalPctBonus { get; set; }

	}
}
