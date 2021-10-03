
using Tank.Interfaces.Body;
using Tank.WeaponSystem.Weapons;
using UnityEngine;

namespace Tank.WeaponSystem.Rotator
{
    internal class WeaponRotator : MonoBehaviour
    {
        protected IDynamicBody _drivenBody;

        [SerializeField]
        [RequiredInterface(typeof(IDynamicBody))]
        private MonoBehaviour _bodyObject;

        [SerializeField]
        protected WeaponBase _weapon;

        [SerializeField]
        private Transform _rotateTransform;

        [SerializeField]
        private float _rotateSpeed = 1f;

        [SerializeField]
        private float _minHorizontalAngleToFire = 4f;

        [SerializeField]
        private bool _alwaysReady = false;

        public bool IsBusy { get; private set; }

        private void Awake()
        {
            this._drivenBody = (IDynamicBody)this._bodyObject;

            this.IsBusy = !this._alwaysReady;
        }

        protected virtual void OnEnable()
        {
            this._weapon.FireTransform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }

        public virtual void RotateToTarget(Vector3 target, float deltaTime)
        {
            Vector3 toTarget = this.GetXZToTarget(target);
            Quaternion desiredRot = Quaternion.LookRotation(toTarget, Vector3.up);
            this._rotateTransform.rotation = Quaternion.Lerp(this._rotateTransform.rotation, desiredRot, Time.deltaTime * this._rotateSpeed);

            if (this._alwaysReady == false)
            {
                if (Vector3.Angle(toTarget, this._rotateTransform.forward) >= this._minHorizontalAngleToFire)
                    this.IsBusy = true;
                else
                    this.IsBusy = false;
            }
        }

        private Vector3 GetXZToTarget(Vector3 target)
        {
            return new Vector3(target.x - this._drivenBody.Position.x, 0f, target.z - this._drivenBody.Position.z);
        }
    }
}
