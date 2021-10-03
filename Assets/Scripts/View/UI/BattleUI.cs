
using System;
using System.Collections.Generic;
using Tank.Interfaces.View;
using Tank.Interfaces.Weapon;
using UnityEngine;

namespace Tank.View.UI
{
    internal class BattleUI : MonoBehaviour
    {
        private readonly List<MovableHealthBar> _healthBars = new List<MovableHealthBar>();

        private UnitView _playerView;

        [Header("HealthBars")]
        [SerializeField]
        private HealthBar _playerHealthBar;

        [SerializeField]
        private RectTransform _healthBarsParent;

        [Header("Weapons")]
        [SerializeField]
        private WeaponImage[] _weaponImages;

        [Header("Prefabs")]
        [SerializeField]
        private MovableHealthBar _healthBarPrefab;

        public static BattleUI Instance { get; private set; }

        public Vector3 PlayerPosition => this._playerView == null ? Vector3.zero : this._playerView.Unit.Body.Position;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance == this)
            {
                Destroy(gameObject);
                Debug.LogError("BattleUI is already initialized");
            }
        }

        public void SetUnit(UnitView unitView)
        {
            if (unitView.Unit.IsPlayer == true)
            {
                if (this._playerView != null)
                    throw new InvalidOperationException("Player already set");

                this._playerView = unitView;

                setPlayer(this._playerView);
            }

            setHealthBar();

            void setPlayer(UnitView playerView)
            {
                this._playerHealthBar.Init(playerView.Unit);
                playerView.WeaponHolder.OnChangeWeapon += this.OnChangeWeapon;

                this.OnChangeWeapon(playerView.WeaponHolder);
            }

            void setHealthBar()
            {
                MovableHealthBar bar = this.GetHealthBar();
                bar.Init(unitView.Unit);
                bar.UnitHudPoint = unitView.UnitHudPoint;
            }
        }

        private void OnChangeWeapon(IWeaponHolderView weaponHolder)
        {
            if (this._weaponImages.Length != 3)
                throw new NotImplementedException("Logic for current weapon images count not implemented");

            IWeaponView[] weapons = weaponHolder.WeaponViews;
            int currentWeaponIndex = weaponHolder.CurrentWeaponIndex;

            if (this._weaponImages.Length - weapons.Length > 1)
                throw new ArgumentException("Cannot draw weapons correctly");

            this._weaponImages[1].Init(weapons[currentWeaponIndex]);

            int previousWeaponIndex;
            if (currentWeaponIndex - 1 < 0)
                previousWeaponIndex = weapons.Length - 1;
            else
                previousWeaponIndex = currentWeaponIndex - 1;
            this._weaponImages[0].Init(weapons[previousWeaponIndex]);

            int nextWeaponIndex;
            if (currentWeaponIndex + 1 == weapons.Length)
                nextWeaponIndex = 0;
            else
                nextWeaponIndex = currentWeaponIndex + 1;
            this._weaponImages[2].Init(weapons[nextWeaponIndex]);
        }

        private MovableHealthBar GetHealthBar()
        {
            foreach (MovableHealthBar bar in this._healthBars)
            {
                if (bar.gameObject.activeSelf == false)
                {
                    return bar;
                }
            }

            MovableHealthBar newBar = Instantiate(this._healthBarPrefab, this._healthBarsParent);
            this._healthBars.Add(newBar);

            return newBar;
        }
    }
}
