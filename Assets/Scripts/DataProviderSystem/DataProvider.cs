
using System.Collections.Generic;
using Tank.DataProviderSystem.DataProviderStrategy;
using Tank.Game;
using Tank.Interfaces.Body;
using Tank.Interfaces.TargetProvider;
using Tank.Interfaces.Unit;
using UnityEngine;

namespace Tank.DataProviderSystem
{
    internal sealed class DataProvider : MonoBehaviour, IDataProvider
    {
        private IDynamicBody _drivenBody;

        [SerializeField]
        private DataProviderStrategyBase _providerStrategy;

        [SerializeField]
        private float _unitDetectRadius = 10f;

        public void Init(IDynamicBody drivenBody)
        {
            this._drivenBody = drivenBody;
        }

        public IDynamicBody GetDrivenBody() => this._drivenBody;

        public IDynamicBody GetTargetBody()
        {
            IUnit drivenUnit = World.GetUnit(this._drivenBody);

            return this._providerStrategy.TargetSelector(drivenUnit).Body;
        }

        public Vector3 GetFutureTargetPosition(float predictionMultiplier)
        {
            IDynamicBody target = GetTargetBody();

            return target.Position + target.Velocity * predictionMultiplier;
        }

        public IEnumerable<IDynamicBody> GetNeighborBodies()
        {
            IUnit drivenUnit = World.GetUnit(this._drivenBody);

            foreach (IUnit unitCandidate in World.GetUnitsAround(this._drivenBody, this._unitDetectRadius, false))
            {
                if (this._providerStrategy.NeighborSelector(drivenUnit, unitCandidate) == true)
                    yield return unitCandidate.Body;
            }
        }
    }
}
