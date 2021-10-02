
using System;
using System.Collections.Generic;
using Assets.Scripts.Interfaces.View;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.View.UI
{
    internal class BattleUI : MonoBehaviour
    {
        private static BattleUI instance;

        private readonly List<MovableHealthBar> _healthBars = new List<MovableHealthBar>();

        [Header("HealthBars")]
        [SerializeField]
        private HealthBar _playerHealthBar;

        [SerializeField]
        private RectTransform _healthBarsParent;

        [Header("Weapons")]
        [SerializeField]
        private Image[] _weaponImages;

        [Header("Prefabs")]
        [SerializeField]
        private MovableHealthBar _healthBarPrefab;

        public static UnitView PlayerView { get; private set; }

        public static Vector3 PlayerPosition => PlayerView == null ? Vector3.zero : PlayerView.Unit.Body.Position;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance == this)
            {
                Destroy(gameObject);
                Debug.LogError("BattleUI is already initialized");
            }
        }

        public static void SetUnit(UnitView unitView)
        {
            if (unitView.Unit.IsPlayer == true)
            {
                if (PlayerView != null)
                    throw new InvalidOperationException("Player already set");

                PlayerView = unitView;

                setPlayer(PlayerView);
            }

            setHealthBar();

            void setPlayer(UnitView playerView)
            {
                instance._playerHealthBar.Init(playerView.Unit);
                playerView.WeaponHolder.OnChangeWeapon += instance.OnChangeWeapon;

                instance.OnChangeWeapon(playerView.WeaponHolder);
            }

            void  setHealthBar()
            {
                MovableHealthBar bar = instance.GetHealthBar();
                bar.Init(unitView.Unit);
                bar.UnitHudPoint = unitView.UnitHudPoint;
            }
        }

        private void OnChangeWeapon(IWeaponHolderView weaponHolder)
        {
            if (this._weaponImages.Length != 3)
                throw new NotImplementedException("Logic for current weapon images count not implemented");

            IWeapon[] weapons = weaponHolder.WeaponsCache;
            int currentWeaponIndex = weaponHolder.CurrentWeaponIndex;

            if (this._weaponImages.Length - weapons.Length > 1)
                throw new ArgumentException("Cannot draw weapons correctly");

            this._weaponImages[1].sprite = weapons[currentWeaponIndex].Icon;

            int previousWeaponIndex;
            if (currentWeaponIndex - 1 < 0)
                previousWeaponIndex = weapons.Length - 1;
            else
                previousWeaponIndex = currentWeaponIndex - 1;
            this._weaponImages[0].sprite = weapons[previousWeaponIndex].Icon;

            int nextWeaponIndex;
            if (currentWeaponIndex + 1 == weapons.Length)
                nextWeaponIndex = 0;
            else
                nextWeaponIndex = currentWeaponIndex + 1;
            this._weaponImages[2].sprite = weapons[nextWeaponIndex].Icon;
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

            MovableHealthBar newInstance = Instantiate(this._healthBarPrefab, this._healthBarsParent);
            this._healthBars.Add(newInstance);

            return newInstance;
        }
    }
}
