
using Tank.Helpers;
using Tank.Interfaces.Body;
using UnityEngine;

namespace Tank.BodySystem
{
    [DisallowMultipleComponent]
    public abstract class DynamicBody : MonoBehaviour, IDynamicBody
    {
        protected Rigidbody _rigidbody;

        [SerializeField]
        protected float _maxSpeed = 60f;

        [SerializeField]
        protected float _turnSpeed = 180f;

        public string Name => gameObject.name;
        public BodyType BodyType => BodyType.Movable;

        public Vector2 Position2D => new Vector2(this._rigidbody.position.x, this._rigidbody.position.z);
        public Vector3 Position => this._rigidbody.position;
        public Vector3 Velocity => this._rigidbody.velocity;

        public float MaxSpeed => this._maxSpeed;

        protected virtual void Awake()
        {
            this._rigidbody = this.GetComponent<Rigidbody>();
        }

        public void AddExplosionForce(float explosionForce, Vector3 explosionPosition, float explosionRadius)
        {
            this._rigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRadius);
        }

        public void SetPosition(Vector3 position, Quaternion rotation)
        {
            this._rigidbody.SetTransform(position, rotation);
            this._rigidbody.ResetRigidbody();
        }

        private void OnEnable()
        {
            this._rigidbody.isKinematic = false;
        }

        private void OnDisable()
        {
            this._rigidbody.ResetRigidbody();
            this._rigidbody.isKinematic = true;
        }

        private void FixedUpdate()
        {
            this.Tick(Time.fixedDeltaTime);
        }

        protected abstract void Tick(float deltaTime);
    }
}
