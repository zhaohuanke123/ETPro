using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class GalConfigCategory : ProtoObject, IMerge
    {
        public static GalConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, GalConfig> dict = new Dictionary<int, GalConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<GalConfig> list = new List<GalConfig>();
		
        public GalConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            GalConfigCategory s = o as GalConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            for(int i =0 ;i<list.Count;i++)
            {
                GalConfig config = list[i];
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public GalConfig Get(int id)
        {
            this.dict.TryGetValue(id, out GalConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (GalConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, GalConfig> GetAll()
        {
            return this.dict;
        }
        public List<GalConfig> GetAllList()
        {
            return this.list;
        }
        public GalConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class GalConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>章节名称</summary>
		[ProtoMember(2)]
		public string ChapterName { get; set; }
		/// <summary>完成奖励</summary>
		[ProtoMember(3)]
		public int Count { get; set; }

	}
}
