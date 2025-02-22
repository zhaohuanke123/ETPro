using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using ProtoBuf;

namespace ET
{
    [ProtoContract]
    [Config]
    public partial class ExerciseConfigCategory : ProtoObject, IMerge
    {
        public static ExerciseConfigCategory Instance;
		
        [ProtoIgnore]
        [BsonIgnore]
        private Dictionary<int, ExerciseConfig> dict = new Dictionary<int, ExerciseConfig>();
		
        [BsonElement]
        [ProtoMember(1)]
        private List<ExerciseConfig> list = new List<ExerciseConfig>();
		
        public ExerciseConfigCategory()
        {
            Instance = this;
        }
        
        public void Merge(object o)
        {
            ExerciseConfigCategory s = o as ExerciseConfigCategory;
            this.list.AddRange(s.list);
        }
		
        public override void EndInit()
        {
            for(int i =0 ;i<list.Count;i++)
            {
                ExerciseConfig config = list[i];
                config.EndInit();
                this.dict.Add(config.Id, config);
            }            
            this.AfterEndInit();
        }
		
        public ExerciseConfig Get(int id)
        {
            this.dict.TryGetValue(id, out ExerciseConfig item);

            if (item == null)
            {
                throw new Exception($"配置找不到，配置表名: {nameof (ExerciseConfig)}，配置id: {id}");
            }

            return item;
        }
		
        public bool Contain(int id)
        {
            return this.dict.ContainsKey(id);
        }

        public Dictionary<int, ExerciseConfig> GetAll()
        {
            return this.dict;
        }
        public List<ExerciseConfig> GetAllList()
        {
            return this.list;
        }
        public ExerciseConfig GetOne()
        {
            if (this.dict == null || this.dict.Count <= 0)
            {
                return null;
            }
            return this.dict.Values.GetEnumerator().Current;
        }
    }

    [ProtoContract]
	public partial class ExerciseConfig: ProtoObject, IConfig
	{
		/// <summary>Id</summary>
		[ProtoMember(1)]
		public int Id { get; set; }
		/// <summary>题目列表名称</summary>
		[ProtoMember(2)]
		public string Name { get; set; }
		/// <summary>题目列表地址</summary>
		[ProtoMember(3)]
		public string Url { get; set; }
		/// <summary>奖励积分</summary>
		[ProtoMember(4)]
		public int PointCount { get; set; }

	}
}
