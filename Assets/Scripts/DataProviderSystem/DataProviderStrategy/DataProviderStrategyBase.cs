
using Tank.Interfaces.Unit;
using UnityEngine;

namespace Tank.DataProviderSystem.DataProviderStrategy
{
    internal abstract class DataProviderStrategyBase : ScriptableObject
    {
        public abstract bool NeighborSelector(IUnit drivenUnit, IUnit candidate);

        public abstract IUnit TargetSelector(IUnit drivenUnit);
    }
}
