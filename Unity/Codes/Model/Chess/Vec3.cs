using UnityEngine;

namespace ET
{
    public class Vec3
    {
        public float x;
        public float y;
        public float z;

        public Vec3()
        {
        }

        public Vec3(Vector3 v): this(v.x, v.y, v.z)
        {
        }

        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
        
        public static implicit operator Vector3(Vec3 v)
        {
            return new Vector3(v.x, v.y, v.z);
        }
    }
}