
using System;
using Tank.Configs;
using Tank.Interfaces.Unit;

namespace Tank.Game.Interfaces
{
    public interface IUnitManager
    {
        event Action<IUnit> OnUnitSpawn;

        void Init(UnitSpawnConfig config);
        void SpawnPlayer();
        void SpawnUnitsToMaximum();
        void ReturnUnit(IUnit unit);
    }
}
