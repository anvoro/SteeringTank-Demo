
using Tank.Interfaces.Body;
using UnityEngine;

namespace Tank.SteeringBehaviors.Behaviors
{
    [DisallowMultipleComponent]
    internal class PursuitBehavior : SteeringBehaviorBase
    {
        [SerializeField]
        private float _slowRadius;

        [SerializeField]
        private int _slowPower;

        [SerializeField]
        [Range(0f, 1f)]
        private float _predictionMultiplier = .25f;

        protected override Vector3 CalculateVelocity()
        {
            Vector3 futurePosition = this._dataProvider.GetFutureTargetPosition(this._predictionMultiplier);

            return this.Seek(futurePosition, this._slowRadius, this._slowPower);
        }
    }
}
