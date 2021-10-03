
using System;
using Tank.Interfaces.View;
using Tank.Interfaces.Weapon;
using Tank.WeaponSystem.Weapons;
using Tank.WeaponSystem.Rotator;
using UnityEngine;

namespace Tank.WeaponSystem
{
    internal class WeaponHolder : MonoBehaviour, IWeaponHolderView
    {
        [Serializable]
        private struct WeaponCell
        {
            public WeaponBase Weapon;
            public WeaponRotator Rotator;
        }

        private IWeaponInput _input;

        [SerializeField]
        [RequiredInterface(typeof(IWeaponInput))]
        private MonoBehaviour _inputObject;

        [SerializeField]
        private WeaponCell[] _weapons;

        public event Action<IWeaponHolderView> OnChangeWeapon;
        public event Action<IWeaponView> OnFire;

        public int CurrentWeaponIndex { get; private set; }

        public IWeaponView[] WeaponViews { get; private set; }

        private void Awake()
        {
            subscribe();
            initWeapons();

            void subscribe()
            {
                this._input = (IWeaponInput) this._inputObject;
                this._input.Fire += this.TryFire;

                if (this._weapons.Length > 1)
                {
                    this._input.ChangeWeapon += this.ChangeWeapon;
                    this.WeaponViews = new IWeaponView[this._weapons.Length];
                }
            }

            void initWeapons()
            {
                for (int i = 0; i < this._weapons.Length; i++)
                {
                    WeaponCell cell = this._weapons[i];
                    cell.Weapon.Init();

                    if(this.WeaponViews != null)
                        this.WeaponViews[i] = cell.Weapon;
                }

                this.CurrentWeaponIndex = 0;

                for (int i = this.CurrentWeaponIndex + 1; i < this._weapons.Length; i++)
                {
                    this._weapons[i].Weapon.gameObject.SetActive(false);
                }
            }
        }

        private void Update()
        {
            foreach (WeaponCell weaponCell in _weapons)
            {
                weaponCell.Weapon.Tick(Time.deltaTime);
            }

            this._weapons[this.CurrentWeaponIndex].Rotator.RotateToTarget(this._input.Target, Time.deltaTime);
        }

        private void TryFire()
        {
            WeaponCell currentWeapon = this._weapons[this.CurrentWeaponIndex];

            if (currentWeapon.Weapon.OnCooldown == false && currentWeapon.Rotator.IsBusy == false)
            {
                currentWeapon.Weapon.Fire();
                this.OnFire?.Invoke(currentWeapon.Weapon);
            }
        }

        private void ChangeWeapon(bool forward)
        {
            this._weapons[this.CurrentWeaponIndex].Weapon.gameObject.SetActive(false);

            if (forward == true)
            {
                if (this.CurrentWeaponIndex + 1 == this._weapons.Length)
                    this.CurrentWeaponIndex = 0;
                else
                    this.CurrentWeaponIndex++;
            }
            else
            {
                if (this.CurrentWeaponIndex - 1 < 0)
                    this.CurrentWeaponIndex = this._weapons.Length - 1;
                else
                    this.CurrentWeaponIndex--;
            }

            this._weapons[this.CurrentWeaponIndex].Weapon.gameObject.SetActive(true);

            this.OnChangeWeapon?.Invoke(this);
        }
    }
}
