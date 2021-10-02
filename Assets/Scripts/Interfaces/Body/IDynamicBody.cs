
using UnityEngine;

namespace Tank.Interfaces.Body
{
    public interface IDynamicBody : IBody
    {
        float MaxSpeed { get; }
        Vector3 Velocity { get; }
        void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius);
        void SetPosition(Vector3 position, Quaternion rotation);
    }
}
