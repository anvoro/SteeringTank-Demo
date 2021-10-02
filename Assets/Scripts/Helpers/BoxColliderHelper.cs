
using UnityEngine;

namespace Tank.Helpers
{
    public static class BoxColliderHelper
    {
        public static Vector3 GetRandomPointInsideCollider(this BoxCollider boxCollider)
        {
            Vector3 extents = boxCollider.size / 2f;
            Vector3 point = new Vector3(
                Random.Range(-extents.x, extents.x),
                Random.Range(-extents.y, extents.y),
                Random.Range(-extents.z, extents.z));

            point += boxCollider.center;

            return boxCollider.transform.TransformPoint(point);
        }

        public static Rect GetRectInXZAxis(this BoxCollider boxCollider)
        {
            Vector3 extents = boxCollider.size / 2f;

            float minX = boxCollider.center.x - extents.x;
            float minZ = boxCollider.center.z - extents.z;

            Vector3 minCorner = new Vector3(minX, 0f, minZ);

            Vector3 worldMinCorner = boxCollider.transform.TransformPoint(minCorner);

            return new Rect(worldMinCorner.x, worldMinCorner.z, boxCollider.size.x, boxCollider.size.z);
        }
    }
}
