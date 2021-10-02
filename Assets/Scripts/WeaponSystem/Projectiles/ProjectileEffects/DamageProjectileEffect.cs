
using Tank.WeaponSystem.Projectiles;
using UnityEngine;

internal abstract class DamageProjectileEffect : ProjectileEffectBase
{
    [SerializeField]
    protected int _maxDamage = 100;
}
