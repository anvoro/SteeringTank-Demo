
using System;
using Tank.Interfaces.Body;
using Tank.Interfaces.TargetProvider;
using Tank.WeaponSystem;
using UnityEngine;

namespace Tank.InputSystem
{
    internal class AgentWeaponInput : MonoBehaviour, IWeaponInput
    {
        private IDataProvider _dataProvider;

        [SerializeField]
        [RequiredInterface(typeof(IDataProvider))]
        private MonoBehaviour _targetProviderObject;

        [SerializeField]
        private float _minFireDistance;

        [SerializeField]
        private float _maxFireDistance;

        [SerializeField]
        [Range(0f, 3f)]
        private float _predictionMultiplier = .25f;

        public event Action Fire;

        public event Action<bool> ChangeWeapon
        {
            add => throw new NotImplementedException();
            remove => throw new NotImplementedException();
        }

        public Vector3 Target => this._dataProvider.GetFutureTargetPosition(this._predictionMultiplier);

        private void Awake()
        {
            this._dataProvider = (IDataProvider)this._targetProviderObject;
        }

        private void Update()
        {
            IDynamicBody parent = this._dataProvider.GetDrivenBody();
            IDynamicBody target = this._dataProvider.GetTargetBody();

            if(target == null || parent == null)
                return;

            if (this.CheckDistance(parent, target) == true)
                this.Fire?.Invoke();
        }

        private bool CheckDistance(IDynamicBody parent, IDynamicBody target)
        {
            float distance = Vector3.Distance(target.Position, parent.Position);

            if (distance >= this._maxFireDistance || distance < this._minFireDistance)
                return false;

            return true;
        }
    }
}
