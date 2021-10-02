
using Tank.Helpers.ObjectPool;
using Tank.WeaponSystem.Projectiles;
using Tank.WeaponSystem.Projectiles.ProjectileEffects;
using UnityEngine;

namespace Tank.WeaponSystem.Weapons
{
    internal class ProjectileWeapon : WeaponBase
    {
        private ObjectPool<Projectile> _pool;

        private float _explosionRadius;

        [Header("Projectile")]
        [SerializeField]
        private float _launchVelocity = 30f;

        [SerializeField]
        private Projectile _projectile;

        [SerializeField]
        private Collider _parentCollider;

        public override float Velocity => this._launchVelocity;
        public override Vector3 VelocityVector => this.CalculateVelocity();

        public override float ExplosionRadius => this._explosionRadius;

        public override void Init()
        {
            setExplosionRadius();
            initPool();

            base.Init();

            void initPool()
            {
                this._pool = new ObjectPool<Projectile>(() =>
                {
                    Projectile projectile = Instantiate(this._projectile, this._fireTransform.position, this._fireTransform.rotation, Projectile.ProjectileParent);
                    projectile.Init(this._parentCollider);

                    return projectile;
                },
                projectile =>
                {
                    projectile.ResetProjectile();
                },
                (go, index) =>
                {
                    go.name = string.Concat(go.name, $"_{index}");
                });

                this._pool.Create(1);
            }

            void setExplosionRadius()
            {
                foreach (ExplosionProjectileEffect e in this._projectile.GetComponents<ExplosionProjectileEffect>())
                {
                    if (e.ExplosionRadius > this._explosionRadius)
                        this._explosionRadius = e.ExplosionRadius;
                }
            }
        }

        protected override void FireInternal()
        {
            Projectile projectile = this._pool.GetOrCreate();
            projectile.OnTriggered += this.OnProjectileTriggered;

            projectile.Launch(this._fireTransform.position, this._fireTransform.rotation, this.CalculateVelocity());
        }

        protected virtual Vector3 CalculateVelocity() => this._launchVelocity * this._fireTransform.forward;

        private void OnProjectileTriggered(Projectile projectile)
        {
            projectile.OnTriggered -= this.OnProjectileTriggered;
            this._pool.Return(projectile);
        }
    }
}
