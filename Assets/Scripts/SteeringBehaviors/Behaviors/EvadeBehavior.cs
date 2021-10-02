
using UnityEngine;

namespace Tank.SteeringBehaviors.Behaviors
{
    [DisallowMultipleComponent]
    internal class EvadeBehavior : SteeringBehaviorBase
    {
        [SerializeField]
        private float _fleeRadius = 100f;

        [SerializeField]
        private int _slowPower;

        [SerializeField]
        [Range(0f, 1f)]
        private float _predictionMultiplier = .25f;

        protected override Vector3 CalculateVelocity()
        {
            Vector3 futurePosition = this._dataProvider.GetFutureTargetPosition(this._predictionMultiplier);

            return this.Flee(futurePosition, this._fleeRadius, this._slowPower);
        }
    }
}
