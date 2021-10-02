
using System;
using Tank.Configs;
using Tank.Interfaces.Body;

namespace Tank.Interfaces.Unit
{
    public interface IUnit : IHealth
    {
        event Action<bool> OnUnitActiveStateChange;
        event Action<IUnit> OnUnitDeath;
        event Action<IUnit> OnCollideWithUnit;

        string Name { get; }
        UnitConfig Config { get; }
        UnitTeam Team { get; }
        bool IsPlayer { get; }

        IDynamicBody Body { get; }

        void Init(UnitConfig config);
        void SetActive(bool active);
        void ResetUnit();
    }
}
