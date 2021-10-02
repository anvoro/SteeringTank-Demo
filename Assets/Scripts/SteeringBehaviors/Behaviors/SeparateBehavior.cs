
using Tank.Interfaces.Body;
using UnityEngine;

namespace Tank.SteeringBehaviors.Behaviors
{
    [DisallowMultipleComponent]
    internal class SeparateBehavior : SteeringBehaviorBase
    {
        [SerializeField]
        private float _desiredSeparation = 3f;

        protected override Vector3 CalculateVelocity()
        {
            Vector3 totalSeparation = Vector3.zero;
            int numNeighbors = 0;

            foreach (IDynamicBody body in this._dataProvider.GetNeighborBodies())
            {
                float distance = Vector3.Distance(this._drivenBody.Position, body.Position);
                Vector3 separationVector = (this._drivenBody.Position - body.Position).normalized;

                if (distance < this._desiredSeparation)
                {
                    separationVector /= distance;

                    totalSeparation += separationVector;

                    numNeighbors++;
                }
            }

            if (numNeighbors > 0)
            {
                return (totalSeparation / numNeighbors).normalized * this._drivenBody.MaxSpeed;
            }

            return Vector3.zero;
        }
    }
}
