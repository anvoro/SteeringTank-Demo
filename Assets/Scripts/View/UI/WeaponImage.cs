
using Tank.Interfaces.Weapon;
using UnityEngine;
using UnityEngine.UI;

namespace Tank.View.UI
{
    internal class WeaponImage : MonoBehaviour
    {
        private IWeaponView _weaponView;

        [SerializeField]
        private Image _weaponIcon;

        [SerializeField]
        private Image _cooldownIcon;

        public void Init(IWeaponView weapon)
        {
            this._weaponView = weapon;
            this._weaponIcon.sprite = _weaponView.Icon;
        }

        private void Update()
        {
            if (this._weaponView.RemainingCooldown > 0)
            {
                this._cooldownIcon.fillAmount = this._weaponView.RemainingCooldown / this._weaponView.Cooldown;
            }
            else
            {
                this._cooldownIcon.fillAmount = 0f;
            }
        }
    }
}
