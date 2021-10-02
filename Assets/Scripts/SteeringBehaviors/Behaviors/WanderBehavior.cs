
using UnityEngine;

namespace Tank.SteeringBehaviors.Behaviors
{
    [DisallowMultipleComponent]
    internal class WanderBehavior : SteeringBehaviorBase
    {
        protected override Vector3 CalculateVelocity()
        {
            Vector3 circleCenter = this._drivenBody.Position + this._drivenBody.Velocity * 2f;
            float radius = this._drivenBody.Velocity.magnitude * 2f;

            Vector3 wanderPosition = circleCenter + (Vector3)Random.insideUnitCircle * radius;

            return this.Seek(wanderPosition, 0, 0);
        }

        //private void DrawWander()
        //{
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawWireSphere(circleCenter, radius);
        //    Gizmos.color = Color.green;
        //    Gizmos.DrawLine(circleCenter, wander);
        //}
    }
}
