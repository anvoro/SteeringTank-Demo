using System;

namespace Assets.Scripts.Interfaces.View
{
    public interface IWeaponHolderView
    {
        event Action<IWeaponHolderView> OnChangeWeapon;

        int CurrentWeaponIndex { get; }

        IWeapon[] WeaponsCache { get; }
    }
}
