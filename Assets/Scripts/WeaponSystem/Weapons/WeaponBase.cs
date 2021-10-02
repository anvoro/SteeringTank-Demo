
using Assets.Scripts.Interfaces.View;
using UnityEngine;

namespace Tank.WeaponSystem
{
    internal abstract class WeaponBase : MonoBehaviour, IWeapon
    {
        private float _remainingCooldownTime;

        [Header("Main")]
        [SerializeField]
        private float _cooldown;

        [SerializeField]
        protected Transform _fireTransform;

        [SerializeField]
        private Sprite _weaponSprite;

        public bool OnCooldown => this._remainingCooldownTime > 0;

        public Sprite Icon => this._weaponSprite;

        public Transform FireTransform => this._fireTransform;

        public abstract float Velocity { get; }

        public abstract Vector3 VelocityVector { get; }

        public abstract float ExplosionRadius { get; }

        public Sprite Image => this._weaponSprite;

        private void OnDisable()
        {
            this._remainingCooldownTime = 0;
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

            this._remainingCooldownTime = this._cooldown;
        }

        private void Tick(float deltaTime)
        {
            if (this.OnCooldown == true)
                this._remainingCooldownTime -= deltaTime;
        }

        protected abstract void FireInternal();
    }
}
