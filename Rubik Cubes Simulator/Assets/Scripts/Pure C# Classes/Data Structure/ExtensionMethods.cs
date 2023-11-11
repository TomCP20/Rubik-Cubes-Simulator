using UnityEngine;
namespace ExtensionMethods
{
    public static class MyExtensions
    {
        public static float ManhattanDistance(this Vector3 a)
        {
            checked
            {
                return Mathf.Abs(a.x) + Mathf.Abs(a.y) + Mathf.Abs(a.z);
            }
        }

        public static float ManhattanDistance(this Vector3 a, Vector3 b)
        {
            checked
            {
                return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z);
            }
        }

        public static Vector3 Abs(this Vector3 a)
        {
            checked
            {
                return new Vector3(Mathf.Abs(a.x), Mathf.Abs(a.y), Mathf.Abs(a.z));
            }
        }

        public static bool EqualOrOposite(this Vector3 a, Vector3 b)
        {
            checked
            {
                return a.Abs() == b.Abs();
            }
        }
    }
}
