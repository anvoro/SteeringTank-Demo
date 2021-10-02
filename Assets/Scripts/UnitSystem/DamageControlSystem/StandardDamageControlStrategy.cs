
using System;
using UnityEngine;

namespace Assets.Scripts.UnitSystem.DamageControlSystem
{
    [CreateAssetMenu(menuName = "DamageControlStrategy/Standard")]
    internal class StandardDamageControlStrategy : DamageControlStrategyBase
    {
        public override int CalculateDamage(int damageValue, float armorValue)
        {
            return Mathf.CeilToInt(damageValue * (1 - Mathf.Clamp01(armorValue)));
        }
    }
}
