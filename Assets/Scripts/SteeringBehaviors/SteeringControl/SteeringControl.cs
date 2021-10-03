
using Tank.BodySystem.BoidBody;
using Tank.Interfaces.Body;
using Tank.Interfaces.DataProvider;
using UnityEngine;

namespace Tank.SteeringBehaviors.SteeringControl
{
    [DisallowMultipleComponent]
    internal class SteeringControl : MonoBehaviour, ISteeringBehavior
    {
        protected IDynamicBody _drivenBody;

        private SteeringRuleProcessor _currentRuleProcessor;

        private float _remainingTimeBeforeUpdate;

        [SerializeField]
        private float _updateRulePeriod = .1f;

        [SerializeField]
        private SteeringRuleProcessor[] _ruleProcessors;

        [SerializeField]
        private SteeringBehaviorBase[] _permanentSteerings;

        public void Init(IDataProvider dataProvider)
        {
            this._drivenBody = dataProvider.GetDrivenBody();

            this._remainingTimeBeforeUpdate = 0;

            foreach (SteeringRuleProcessor processor in this._ruleProcessors)
            {
                processor.Init(dataProvider);
            }

            foreach (ISteeringBehavior steering in this._permanentSteerings)
            {
                steering.Init(dataProvider);
            }

            this._currentRuleProcessor = this._ruleProcessors[0];
        }

        public Vector3 GetVelocity(float deltaTime)
        {
            if (this._remainingTimeBeforeUpdate <= 0)
            {
                this._currentRuleProcessor = this.CheckCurrentRule();

                this._remainingTimeBeforeUpdate = this._updateRulePeriod;
            }

            this._remainingTimeBeforeUpdate -= deltaTime;

            return calculateResult(deltaTime); ;

            Vector3 calculateResult(float deltaTime)
            {
                Vector3 result = Vector3.zero;

                foreach (ISteeringBehavior steering in this._permanentSteerings)
                {
                    result += steering.GetVelocity(deltaTime);
                }

                result += this._currentRuleProcessor.GetVelocity(deltaTime);

                return result;
            }
        }

        private SteeringRuleProcessor CheckCurrentRule()
        {
            if (this._currentRuleProcessor.ProcessRule(this._drivenBody) == true)
                return this._currentRuleProcessor;

            foreach (SteeringRuleProcessor processor in this._ruleProcessors)
            {
                if (processor == this._currentRuleProcessor)
                    continue;

                if (processor.ProcessRule(this._drivenBody) == true)
                {
                    return processor;
                }
            }

            return this._ruleProcessors[0];
        }
    }
}
