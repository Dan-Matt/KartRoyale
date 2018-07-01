using UnityEngine;

namespace Assets.Scripts.Utilities
{
    public static class VectorUtilities
    {
        public static Vector3 Direction(Vector3 fromPoint, Vector3 toPoint)
        {
            return toPoint - fromPoint;
        }

        public static Vector3 PointInDirection(Vector3 fromPoint, Vector3 direction, float distance)
        {
            return fromPoint + direction.normalized * distance;
        }
    }
}
