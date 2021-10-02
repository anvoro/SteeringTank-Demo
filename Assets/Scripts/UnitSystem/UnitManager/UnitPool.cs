
using Tank.Configs;
using Tank.Helpers.ObjectPool;
using Tank.Interfaces.Unit;
using UnityEngine;

namespace Tank.UnitSystem.UnitManager
{
    [DisallowMultipleComponent]
    internal class UnitPool : MonoBehaviour
    {
        private MultiKeyObjectPool<UnitConfig, IUnit> _pool;

        private Transform _unitsParent;

        private void Awake()
        {
            this._pool = new MultiKeyObjectPool<UnitConfig, IUnit>(this.CreateUnit, this.ResetUnit, this.SetNameIndex);
        }

        public void Init(Transform unitParent)
        {
            this._unitsParent = unitParent;
        }

        public void Create(UnitConfig config, int count)
        {
            this._pool.Create(config, count);
        }

        public IUnit GetOrCreate(UnitConfig config)
        {
            return this._pool.GetOrCreate(config);
        }

        public void Return(IUnit unit)
        {
            this._pool.Return(unit.Config, unit);
        }

        private void ResetUnit(IUnit unit)
        {
            unit.ResetUnit();
            unit.SetActive(false);
        }

        private IUnit CreateUnit(UnitConfig config)
        {
            GameObject unitPrefab = Instantiate(config.UnitPrefab, this._unitsParent);

            IUnit unit = unitPrefab.GetComponent<IUnit>();
            unit.Init(config);

            return unit;
        }

        private void SetNameIndex(GameObject go, int index)
        {
            go.name = string.Concat(go.name, $"_{index}");
        }
    }
}
