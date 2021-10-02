
using UnityEngine;

namespace Tank.Configs
{
    [CreateAssetMenu]
    public class UnitConfig : ScriptableObject
    {
        public GameObject UnitPrefab;

        public UnitTeam Team;

        public int MaxHealth = 1000;

        [Range(0, 1f)]
        public float Armor = 0f;
    }
}
