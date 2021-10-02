
using Tank.Interfaces.Unit;
using UnityEngine;

namespace Tank.WeaponSystem.Projectiles.ProjectileEffects
{
    internal class ExplosionProjectileEffect : DamageProjectileEffect
    {
        [SerializeField]
        protected LayerMask _unitMask;

        [SerializeField]
        private float _explosionForce = 1000f;

        [SerializeField]
        private float _explosionRadius = 5f;

        public float ExplosionRadius => this._explosionRadius;

        public override void ProcessCollision(Collider other)
        {
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, this._explosionRadius, this._unitMask);

            foreach (Collider c in colliders)
            {
                IUnit unit = c.GetComponent<IUnit>();

                if (unit == null)
                    continue;

                unit.Body.AddExplosionForce(this._explosionForce, this.transform.position, this._explosionRadius);

                int damage = this.CalculateDamage(unit.Body.Position);
                unit.Hurt(damage);
            }
        }

        private int CalculateDamage(Vector3 targetPosition)
        {
            float explosionDistance = Vector3.Distance(targetPosition, this.transform.position);
            float relativeDistance = (this._explosionRadius - explosionDistance) / this._explosionRadius;

            float damage = relativeDistance * this._maxDamage;
            damage = Mathf.Max(0f, damage);

            return Mathf.FloorToInt(damage);
        }
    }
}
