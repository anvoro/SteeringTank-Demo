
using Tank.Interfaces.Unit;
using Tank.WeaponSystem;
using UnityEngine;

namespace Tank.WeaponSystem.Weapons
{
    internal class RayWeapon : WeaponBase
    {
        [Header("Ray")]
        [SerializeField]
        private int _damage = 100;

        [SerializeField]
        private int _maxDistance = 100;

        [SerializeField]
        private float _rayRadius = 2f;

        [SerializeField]
        private LayerMask _unitMask;

        public override float Velocity => float.MaxValue;

        public override Vector3 VelocityVector => this._fireTransform.forward * this._maxDistance;

        public override float ExplosionRadius => 0f;

        protected override void FireInternal()
        {
            foreach (RaycastHit hit in Physics.SphereCastAll(this._fireTransform.position, this._rayRadius, this._fireTransform.forward, this._maxDistance, this._unitMask))
            {
                hit.transform.gameObject.GetComponent<IHealth>()?.Hurt(this._damage);
            }
        }
    }
}
