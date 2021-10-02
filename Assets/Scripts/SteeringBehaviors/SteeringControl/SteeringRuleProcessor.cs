
using UnityEngine;
using Tank.BodySystem.BoidBody;
using Tank.Interfaces.Body;
using Tank.Interfaces.TargetProvider;

namespace Tank.SteeringBehaviors.SteeringControl
{
    [DisallowMultipleComponent]
    public sealed class SteeringRuleProcessor : MonoBehaviour
    {
        private ISteeringRule[] _rules;
        private SteeringBehaviorBase[] _steerings;

        public void Init(IDataProvider dataProvider)
        {
            this._rules = this.GetComponents<ISteeringRule>();
            this._steerings = this.GetComponents<SteeringBehaviorBase>();

            foreach (SteeringBehaviorBase steering in this._steerings)
            {
                steering.Init(dataProvider);
            }
        }

        public bool ProcessRule(IDynamicBody drivenBody)
        {
            foreach(ISteeringRule rule in this._rules)
            {
                if (rule.ProcessRule(drivenBody) == false)
                    return false;
            }

            return true;
        }

        public Vector3 GetVelocity(float deltaTime)
        {
            Vector3 result = Vector3.zero;

            foreach (ISteeringBehavior steering in this._steerings)
            {
                result += steering.GetVelocity(deltaTime);
            }

            return result;
        }
    }
}
