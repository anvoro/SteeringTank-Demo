
using UnityEngine;

namespace Tank.Interfaces.Weapon
{
    public interface IWeaponView
    {
        Sprite Icon { get; }
        float RemainingCooldown { get; }
        float Cooldown { get; }
        Transform FireTransform { get; }
        Vector3 VelocityVector { get; }
        float ExplosionRadius { get; }
    }
}
