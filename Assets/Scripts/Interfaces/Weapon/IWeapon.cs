
using UnityEngine;

namespace Assets.Scripts.Interfaces.View
{
    public interface IWeapon
    {
        Sprite Icon { get; }
        Transform FireTransform { get; }
        Vector3 VelocityVector { get; }
        float ExplosionRadius { get; }
    }
}
