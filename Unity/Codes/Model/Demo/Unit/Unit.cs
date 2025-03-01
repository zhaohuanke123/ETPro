using MongoDB.Bson.Serialization.Attributes;
using UnityEngine;

namespace ET
{
    [ChildOf(typeof (UnitComponent))]
    public class Unit: Entity, IAwake<int>, IAwake
    {
        public int ConfigId; //配置表id

        [BsonIgnore]
        public UnitConfig Config => UnitConfigCategory.Instance.Get(this.ConfigId);

        [BsonIgnore]
        public UnitType Type => (UnitType)Config.Type;

        private Vector3 position = new Vector3(); //坐标

        public Vector3 Position
        {
            get => this.position;
            set
            {
                EventType.ChangePosition.Instance.OldPos = this.position;
                this.position = value;

                // EventType.ChangePosition.Instance.Unit = this;
                // Game.EventSystem.PublishClass(EventType.ChangePosition.Instance);
            }
        }

        [BsonIgnore]
        public Vector3 Forward
        {
            get => this.Rotation * Vector3.forward;
            set => this.Rotation = Quaternion.LookRotation(value, Vector3.up);
        }

        private Quaternion rotation = new Quaternion();

        public Quaternion Rotation
        {
            get => this.rotation;
            set
            {
                this.rotation = value;
                // EventType.ChangeRotation.Instance.Unit = this;
                // Game.EventSystem.PublishClass(EventType.ChangeRotation.Instance);
            }
        }

#if !SERVER

        private Vector3 viewPosition; //坐标

        public Vector3 ViewPosition
        {
            get => this.viewPosition;
            set
            {
                this.viewPosition = value;

                Game.EventSystem.Publish(new EventType.CPChangePosition() { Unit = this });
            }
        }

        private Quaternion viewRotation = Quaternion.identity;

        public Quaternion ViewRotation
        {
            get => this.viewRotation;
            set
            {
                this.viewRotation = value;
                EventType.ChangeRotation.Instance.Unit = this;
                Game.EventSystem.Publish(new EventType.CPChangeRotate() { Unit = this });
            }
        }

        public ETTask ViewReadTask;

#endif
    }
}