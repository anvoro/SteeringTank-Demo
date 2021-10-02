
using Tank.Helpers;
using UnityEngine;

namespace Tank.SteeringBehaviors.Behaviors
{
    //todo попробовать заменить это на flee от ближайшего обстакла мб повернутого на какой то угол
    [DisallowMultipleComponent]
    internal sealed class CollisionAvoidanceBehavior : SteeringBehaviorBase
    {
        private const float FRONT_RAY = 10f;
        private const float SIDE_RAY = 7f;

        private const float SIDE_MULTIPLIER = .7f;

        private Vector3 _avoidance = Vector3.zero;

        private uint _collisionCounter = 0;

        //todo включать компоненты уклонения только в непосредственной близости от препятствий, сверяться с картой квадрантов
        protected override Vector3 CalculateVelocity()
        {
            if (this._collisionCounter > 0)
            {
                this._avoidance *= Exp.GetInversePower(1);
            }
            else
            {
                this._avoidance *= Exp.GetInversePower(2);
            }

            Vector3 forward = transform.forward;
            Vector3 right = (forward + transform.right).normalized;
            Vector3 left = (forward - transform.right).normalized;

            bool isFront = Physics.Raycast(this._drivenBody.Position, forward, out RaycastHit frontHit, FRONT_RAY);
            bool isRight = Physics.Raycast(this._drivenBody.Position, right, out RaycastHit rightHit, SIDE_RAY);
            bool isLeft = Physics.Raycast(this._drivenBody.Position, left, out RaycastHit leftHit, SIDE_RAY);

            bool hasCollision = isFront || isRight || isLeft;
            if (hasCollision == true)
            {
                this._collisionCounter++;

                if (isFront == true)
                {
                    this._avoidance += (frontHit.normal + transform.forward).normalized;
                }
                if (isRight == true)
                {
                    this._avoidance += (rightHit.normal + left).normalized * SIDE_MULTIPLIER;
                }
                if (isLeft == true)
                {
                    this._avoidance += (leftHit.normal + right).normalized * SIDE_MULTIPLIER;
                }
            }
            else
            {
                this._collisionCounter--;
            }

            return this._avoidance * this._drivenBody.MaxSpeed;
        }

        private void DrawProbe()
        {
            //Gizmos.DrawLine(_body.position, _body.position + transform.forward * 10f);
            //Gizmos.DrawLine(_body.position, _body.position + (transform.forward + transform.right).normalized * 8f);
            //Gizmos.DrawLine(_body.position, _body.position + (transform.forward - transform.right).normalized * 8f);

            //Gizmos.color = Color.blue;
            //Gizmos.DrawLine(_body.position, _body.position + _avoidance);
        }
    }
}
