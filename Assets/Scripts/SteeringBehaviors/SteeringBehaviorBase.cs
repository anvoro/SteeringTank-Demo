
using Tank.BodySystem.BoidBody;
using Tank.Helpers;
using Tank.Interfaces.Body;
using Tank.Interfaces.TargetProvider;
using UnityEngine;

namespace Tank.SteeringBehaviors
{
    internal abstract class SteeringBehaviorBase : MonoBehaviour, ISteeringBehavior
    {
        private Vector3 _currentVelocity;
        private float _remainingTimeBeforeUpdate;

        protected IDataProvider _dataProvider;
        protected IDynamicBody _drivenBody;

        [SerializeField]
        [Range(0f, 2f)]
        private float _weight = 1f;

        [SerializeField]
        private float _updatePeriod = .1f;

        protected abstract Vector3 CalculateVelocity();

        public void Init(IDataProvider dataProvider)
        {
            this._dataProvider = dataProvider;
            this._drivenBody = this._dataProvider.GetDrivenBody();

            this.ResetSteering();
        }

        public Vector3 GetVelocity(float deltaTime)
        {
            if (this._remainingTimeBeforeUpdate <= 0)
            {
                this._currentVelocity = this.CalculateVelocity() * this._weight;

                this._remainingTimeBeforeUpdate = this._updatePeriod;
            }

            this._remainingTimeBeforeUpdate -= deltaTime;

            return this._currentVelocity;
        }

        private void OnDisable()
        {
            this.ResetSteering();
        }

        private void ResetSteering()
        {
            this._remainingTimeBeforeUpdate = 0;
            this._currentVelocity = Vector3.zero;
        }

        protected Vector3 Seek(Vector3 targetPosition, float slowRadius, int slowPower)
        {
            Vector3 desiredVelocity = (targetPosition - this._drivenBody.Position).normalized * this._drivenBody.MaxSpeed;

            float distance = Vector3.Distance(targetPosition, this._drivenBody.Position);
            if (distance < slowRadius)
            {
                desiredVelocity *= (distance / slowRadius) * Exp.GetInversePower(slowPower);
            }

            return desiredVelocity;
        }

        protected Vector3 Flee(Vector3 targetPosition, float fleeRadius, int slowPower)
        {
            Vector3 desiredVelocity = (this._drivenBody.Position - targetPosition).normalized * this._drivenBody.MaxSpeed;

            float distance = Vector3.Distance(targetPosition, this._drivenBody.Position);
            if (distance > fleeRadius)
            {
                desiredVelocity *= (fleeRadius / distance) * Exp.GetInversePower(slowPower);
            }

            return desiredVelocity;
        }
    }
}
