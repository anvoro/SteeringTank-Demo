
using UnityEngine;

namespace Assets.Scripts.UnitSystem.DamageControlSystem
{
    internal abstract class DamageControlStrategyBase : ScriptableObject
    {
        public abstract int CalculateDamage(int damageValue, float armorValue);
    }
}
