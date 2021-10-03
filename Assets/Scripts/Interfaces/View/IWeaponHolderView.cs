
using System;
using Tank.Interfaces.Weapon;

namespace Tank.Interfaces.View
{
    public interface IWeaponHolderView
    {
        event Action<IWeaponHolderView> OnChangeWeapon;
        event Action<IWeaponView> OnFire;

        int CurrentWeaponIndex { get; }

        IWeaponView[] WeaponViews { get; }
    }
}
