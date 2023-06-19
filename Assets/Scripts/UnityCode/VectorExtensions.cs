using UnityEngine;

namespace UnityCode
{
    public static class VectorExtensions
    {
        public static bool Approximately(this Vector2 vector, Vector2 other)
        {
            return Mathf.Approximately(vector.x, other.x) && Mathf.Approximately(vector.y, other.y);
        }
        
        public static bool Approximately(this Vector3 vector, Vector3 other)
        {
            return Mathf.Approximately(vector.x, other.x) && Mathf.Approximately(vector.y, other.y) && Mathf.Approximately(vector.z, other.z);
        }
    }
}