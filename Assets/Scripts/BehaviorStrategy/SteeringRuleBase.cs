
using Tank.Interfaces.Body;
using Tank.SteeringBehaviors.SteeringControl;
using UnityEngine;

namespace Tank.BehaviorStrategy
{
    public abstract class SteeringRuleBase : MonoBehaviour, ISteeringRule
    {
        public abstract bool ProcessRule(IDynamicBody drivenBody);
    }
}
