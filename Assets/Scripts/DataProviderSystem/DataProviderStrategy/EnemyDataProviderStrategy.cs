
using Tank.Game;
using Tank.Interfaces.Unit;
using UnityEngine;

namespace Tank.DataProviderSystem.DataProviderStrategy
{
    [CreateAssetMenu(menuName = "TargetProviderStrategy/Enemy")]
    internal class EnemyDataProviderStrategy : DataProviderStrategyBase
    {
        public override bool NeighborSelector(IUnit drivenUnit, IUnit candidate)
        {
            return drivenUnit.Team == candidate.Team;
        }

        public override IUnit TargetSelector(IUnit drivenUnit)
        {
            return World.Player;
        }
    }
}
