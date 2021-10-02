
using Tank.Interfaces.Unit;
using UnityEngine;

namespace Tank.WeaponSystem.Projectiles.ProjectileEffects
{
    internal class DirectDamageProjectileEffect : DamageProjectileEffect
    {
        public override void ProcessCollision(Collider other)
        {
            other.gameObject.GetComponent<IHealth>()?.Hurt(this._maxDamage);
        }
    }
}
