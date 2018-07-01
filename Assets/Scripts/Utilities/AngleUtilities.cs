using UnityEngine;

namespace Assets.Scripts.Utilities
{
    public static class AngleUtilities
    {
        /// <summary>
        /// Returns -1 when to the left, 1 to the right, and 0 for forward/backward
        /// </summary>
        /// <param name="forward"></param>
        /// <param name="targetDirection"></param>
        /// <param name="up"></param>
        /// <returns></returns>
        public static int AngleDirection(
            Vector3 forward, Vector3 targetDirection, Vector3 up)
        {
            var perpendicular = Vector3.Cross(forward, targetDirection);
            var direction = Vector3.Dot(perpendicular, up);

            if (direction > 0.0f)
            {
                return 1;
            }
            if (direction < 0.0f)
            {
                return -1;
            }
            return 0;
        }

        public static bool AngleIsBehind(Vector3 forward, Vector3 targetDirection)
        {
            return Vector3.Dot(forward, targetDirection) < 0;
        }

        public static float AngleBetween(Vector3 fromDirection, Vector3 toDirection)
        {
            return Vector3.Angle(fromDirection, toDirection);
        }
    }
}
