
using System;
using Tank.Helpers;
using Tank.Interfaces.View;
using UnityEngine;

namespace Tank.WeaponSystem.Projectiles
{
    [DisallowMultipleComponent]
    internal sealed class Projectile : MonoBehaviour, IStandaloneVFXEvent
    {
        private static Transform projectileParent;

        public static Transform ProjectileParent => projectileParent ??= GameObject.FindGameObjectWithTag("ProjectileParent").transform;

        private Rigidbody _rigidbody;
        private Collider _collider;

        private ProjectileEffectBase[] _effects;

        public event Action<Projectile> OnTriggered;
        public event Action OnStandaloneVFXEvent;

        private void Awake()
        {
            this._rigidbody = this.GetComponent<Rigidbody>();
            this._collider = this.GetComponent<Collider>();

            this._effects = this.GetComponents<ProjectileEffectBase>();
        }

        public void Init(Collider parentCollider)
        {
            Physics.IgnoreCollision(parentCollider, this._collider);
        }

        public void Launch(Vector3 initialPosition, Quaternion initialRotation, Vector3 velocity)
        {
            this._rigidbody.isKinematic = false;
            this._rigidbody.velocity = velocity;
            this._rigidbody.SetTransform(initialPosition, initialRotation);

            this.gameObject.SetActive(true);
        }

        public void ResetProjectile()
        {
            this._rigidbody.ResetRigidbody();
            this._rigidbody.isKinematic = true;

            this.gameObject.SetActive(false);

            this.SetTriggered(false);
        }

        private void FixedUpdate()
        {
            this._rigidbody.MoveRotation(Quaternion.LookRotation(this._rigidbody.velocity));
        }

        private void OnTriggerEnter(Collider other)
        {
            foreach (ProjectileEffectBase effect in this._effects)
            {
                effect.ProcessCollision(other);
            }

            this.SetTriggered(true);

            this.OnStandaloneVFXEvent?.Invoke();
            this.OnTriggered?.Invoke(this);
        }

        private void SetTriggered(bool isTriggered)
        {
            this._collider.enabled = isTriggered == false;
        }
    }
}
