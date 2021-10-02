
using System;

namespace Tank.Interfaces.Unit
{
    public interface IHealth
    {
        event Action OnHealthChange;
        int MaxHealth { get; }
        int CurrentHealth { get; }
        bool IsDead { get; }
        void Hurt(int value);
        void Kill();
    }
}
