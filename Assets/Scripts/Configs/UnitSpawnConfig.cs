
using System.Collections.Generic;
using UnityEngine;

namespace Tank.Configs
{
    [CreateAssetMenu]
    public class UnitSpawnConfig : ScriptableObject
    {
        public float SpawnDelay = 1f;

        public int MaxUnitCount = 10;

        public UnitConfig Player;

        public List<UnitConfig> Enemies;
    }
}
