
using UnityEngine;

namespace Tank.WeaponSystem.Projectiles
{
    internal abstract class ProjectileEffectBase : MonoBehaviour
    {
        public abstract void ProcessCollision(Collider other);
    }
}
