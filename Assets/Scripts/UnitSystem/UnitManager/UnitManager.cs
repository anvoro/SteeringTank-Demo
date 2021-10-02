
using System;
using System.Collections.Generic;
using Tank.Configs;
using Tank.Game.Interfaces;
using Tank.Interfaces.Unit;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Tank.UnitSystem.UnitManager
{
    [DisallowMultipleComponent]
    internal sealed class UnitManager : MonoBehaviour, IUnitManager
    {
        private readonly HashSet<IUnit> _units = new HashSet<IUnit>();

        private UnitSpawnTimer _spawnTimer;
        private UnitSpawnConfig _config;

        [SerializeField]
        private UnitPool _unitPool;

        [SerializeField]
        private Transform _unitParent;

        public event Action<IUnit> OnUnitSpawn;

        private void Update()
        {
            trySpawnUnit(Time.deltaTime);

            void trySpawnUnit(float deltaTime)
            {
                if (this._units.Count < this._config.MaxUnitCount + 1)
                {
                    this._spawnTimer.Tick(deltaTime, out bool isZero);

                    if (isZero == true)
                    {
                        this.SpawnRandomUnit();
                        this._spawnTimer.Reset();
                    }
                }
            }
        }

        public void Init(UnitSpawnConfig config)
        {
            this._config = config;

            this._unitPool.Init(this._unitParent);
            this._unitPool.Create(this._config.Player, 1);

            foreach (UnitConfig c in this._config.Enemies)
            {
                int count = Mathf.CeilToInt(this._config.MaxUnitCount / this._config.Enemies.Count) + 1;
                this._unitPool.Create(c, count);
            }

            this._spawnTimer = new UnitSpawnTimer(this._config.SpawnDelay);
        }

        public void SpawnPlayer()
        {
            this.SpawnUnit(this._config.Player);
        }

        public void SpawnUnitsToMaximum()
        {
            for (int i = 0; i < this._config.MaxUnitCount; i++)
            {
                this.SpawnRandomUnit();
            }
        }

        public void ReturnUnit(IUnit unit)
        {
            this._units.Remove(unit);
            this._unitPool.Return(unit);
        }

        private void SpawnRandomUnit()
        {
            UnitConfig configToSpawn = this._config.Enemies[Random.Range(0, this._config.Enemies.Count)];
            this.SpawnUnit(configToSpawn);
        }

        private void SpawnUnit(UnitConfig config)
        {
            IUnit unit = this._unitPool.GetOrCreate(config);
            this._units.Add(unit);

            this.OnUnitSpawn?.Invoke(unit);
        }
    }
}
