using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Options;

namespace ET
{
    [ComponentOf]
    public class HeroComponent: Entity, IAwake, IDestroy
    {
        // 玩家拥有的英雄列表
        [BsonDictionaryOptions(DictionaryRepresentation.ArrayOfDocuments)]
        public Dictionary<int, int> Heroes = new Dictionary<int, int>();
    }

    // 单个英雄实体
    // [ChildOf(typeof (HeroComponent))]
    // public class Hero: Entity, IAwake<int>
    // {
    //     public int ConfigId;
    // }
}