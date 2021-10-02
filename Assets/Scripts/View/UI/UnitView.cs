
using Assets.Scripts.Interfaces.View;
using Tank.Interfaces.Unit;
using Tank.View.VFX;
using UnityEngine;

namespace Assets.Scripts.View.UI
{
    internal class UnitView : MonoBehaviour
    {
        [SerializeField]
        [RequiredInterface(typeof(IWeaponHolderView))]
        private MonoBehaviour _weaponHolderViewObject;

        [SerializeField]
        private Transform _unitHudPoint;

        [SerializeField]
        private VFXSpawner _deathVFX;

        public IUnit Unit { get; private set; }
        public IWeaponHolderView WeaponHolder { get; private set; }

        public Transform UnitHudPoint => this._unitHudPoint;

        private void Awake()
        {
            this.Unit = this.GetComponent<IUnit>();
            this.Unit.OnUnitDeath += unit => this._deathVFX.SpawnVFX();
            this.Unit.OnUnitActiveStateChange += this.OnUnitActiveStateChange;

            this.WeaponHolder = (IWeaponHolderView) this._weaponHolderViewObject;
        }

        private void OnUnitActiveStateChange(bool active)
        {
            if(active == true)
                BattleUI.SetUnit(this);
        }
    }
}
