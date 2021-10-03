
using UnityEngine;

namespace Tank.UnitSystem.DamageControlSystem
{
    internal abstract class DamageControlStrategyBase : ScriptableObject
    {
        public abstract int CalculateDamage(int damageValue, float armorValue);
    }
}
