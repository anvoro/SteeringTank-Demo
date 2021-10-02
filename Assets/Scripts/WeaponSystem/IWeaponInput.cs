
using System;
using UnityEngine;

namespace Tank.WeaponSystem
{
    public interface IWeaponInput
    {
        event Action Fire;
        event Action<bool> ChangeWeapon;
        Vector3 Target { get; }
    }
}
