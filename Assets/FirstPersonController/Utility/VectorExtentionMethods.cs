using UnityEngine;

namespace FirstPersonController.Scripts
{
    public static class VectorExtentionMethods 
    {
        public static Vector3 TransformDimension(this Vector2 a) => new Vector3(a.x, 0, a.y);
        public static Vector3 HorizontalPlane(this Vector3 a) => new Vector3(a.x, 0, a.z);
    }
}