
using Tank.Interfaces.DataProvider;
using UnityEngine;

namespace Tank.BodySystem.BoidBody
{
    public class BoidBody : DynamicBody
    {
        private Vector3 _steeringForceToDraw;

        private ISteeringBehavior _driver;

        [SerializeField]
        private float _maxForce = 100f;

        [SerializeField]
        [RequiredInterface(typeof(ISteeringBehavior))]
        private MonoBehaviour _driverObject;

        [SerializeField]
        [RequiredInterface(typeof(IDataProvider))]
        private MonoBehaviour _dataProviderObject;

        protected override void Awake()
        {
            IDataProvider dataProvider = (IDataProvider)this._dataProviderObject;
            dataProvider.Init(this);

            this._driver = (ISteeringBehavior)this._driverObject;
            this._driver.Init(dataProvider);

            base.Awake();
        }

        protected override void Tick(float deltaTime)
        {
            Vector3 steeringForce = calculateSteering(deltaTime);

            this._steeringForceToDraw = steeringForce;

            this.Move(steeringForce);
            this.TurnTowardsVelocity(deltaTime);

            Vector3 calculateSteering(float deltaTime)
            {
                Vector3 desiredVelocity = this._driver.GetVelocity(deltaTime);
                Vector3 steering = desiredVelocity - this._rigidbody.velocity;

                return Vector3.ClampMagnitude(steering, this._maxForce);
            }
        }

        private void Move(Vector3 steeringForce)
        {
            this._rigidbody.AddForce(steeringForce);
            this._rigidbody.angularVelocity = Vector3.zero;
        }

        private void TurnTowardsVelocity(float deltaTime)
        {
            float turn = Vector3.SignedAngle(this._rigidbody.velocity, this.transform.forward, Vector3.down) * this._turnSpeed * deltaTime;
            Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);

            this._rigidbody.MoveRotation(this._rigidbody.rotation * turnRotation);
        }

        private void OnDrawGizmos()
        {
            if (this._rigidbody == null)
                return;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(this._rigidbody.position, this._rigidbody.position + this._rigidbody.velocity);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(this._rigidbody.position + this._rigidbody.velocity, this._rigidbody.position + this._rigidbody.velocity + _steeringForceToDraw);
        }
    }
}
