using System;
using UnityEngine;

public class TriggerInfo: MonoBehaviour, IEquatable<TriggerInfo>
{
    public int gridType = -1;

    public int gridX = -1;

    public int gridZ = -1;

    public bool Equals(TriggerInfo other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return base.Equals(other) && this.gridType == other.gridType && this.gridX == other.gridX && this.gridZ == other.gridZ;
    }
}