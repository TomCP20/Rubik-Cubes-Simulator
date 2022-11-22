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
    }
}
