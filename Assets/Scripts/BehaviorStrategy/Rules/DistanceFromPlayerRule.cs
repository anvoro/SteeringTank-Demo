
using Tank.Game;
using Tank.Interfaces.Body;
using UnityEngine;

namespace Tank.BehaviorStrategy.Rules
{
    public class DistanceFromPlayerRule : SteeringRuleBase
    {
        [SerializeField]
        private float _minDistance;

        [SerializeField]
        private float _maxDistance;

        public override bool ProcessRule(IDynamicBody drivenBody)
        {
            float distance = Vector3.Distance(drivenBody.Position, World.Instance.Player.Body.Position);

            return distance >= this._minDistance && distance < _maxDistance;
        }
    }
}
