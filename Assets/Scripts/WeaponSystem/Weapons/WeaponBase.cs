
using Tank.Interfaces.Weapon;
using UnityEngine;

namespace Tank.WeaponSystem.Weapons
{
    internal abstract class WeaponBase : MonoBehaviour, IWeaponView
    {
        [Header("Main")]
        [SerializeField]
        private float _cooldown;

        [SerializeField]
        protected Transform _fireTransform;

        [SerializeField]
        private Sprite _weaponSprite;

        public bool OnCooldown => this.RemainingCooldown > 0;

        public Sprite Icon => this._weaponSprite;

        public float RemainingCooldown { get; private set; }

        public float Cooldown => this._cooldown;

        public Transform FireTransform => this._fireTransform;

        public abstract float Velocity { get; }

        public abstract Vector3 VelocityVector { get; }

        public abstract float ExplosionRadius { get; }

        private void OnDisable()
        {
            this.RemainingCooldown = 0;
        }

        private void Update()
        {
            this.Tick(Time.deltaTime);
        }

        public virtual void Init() { }

        public void Fire()
        {
            if (this.OnCooldown == true)
                return;

            this.FireInternal();

            this.RemainingCooldown = this._cooldown;
        }

        private void Tick(float deltaTime)
        {
            if (this.OnCooldown == true)
                this.RemainingCooldown -= deltaTime;
        }

        protected abstract void FireInternal();
    }
}
