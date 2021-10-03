
using System.Collections.Generic;
using Tank.Configs;
using Tank.Game.Interfaces;
using Tank.Game.SpawnShapes;
using Tank.Interfaces.Body;
using Tank.Interfaces.Unit;
using UnityEngine;

namespace Tank.Game
{
    [DisallowMultipleComponent]
    public sealed class World : MonoBehaviour
    {
        private readonly Dictionary<IBody, IUnit> _unitsInWorld = new Dictionary<IBody, IUnit>();

        private IUnitManager _unitManager;
        private IEnvironment _environment;

        private ISpawnShape _spawnShape;

        [SerializeField]
        [RequiredInterface(typeof(IUnitManager))]
        private MonoBehaviour _unitManagerObject;

        [SerializeField]
        [RequiredInterface(typeof(IEnvironment))]
        private MonoBehaviour _environmentObject;

        [SerializeField]
        [RequiredInterface(typeof(ISpawnShape))]
        private MonoBehaviour _spawnShapeObject;

        [SerializeField]
        private UnitSpawnConfig _spawnConfig;

        [SerializeField]
        private UnitTeam _playerTeam;

        public static World Instance { get; private set; }

        public IUnit Player { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance == this)
            {
                Destroy(gameObject);
                Debug.LogError("World is already initialized");
            }

            this._unitManager = (IUnitManager)this._unitManagerObject;
            this._environment = (IEnvironment)this._environmentObject;

            this._spawnShape = (ISpawnShape)this._spawnShapeObject;

            this._unitManager.OnUnitSpawn += this.SubscribeUnit;
        }

        private void Start()
        {
            this._unitManager.Init(this._spawnConfig);

            this._unitManager.SpawnPlayer();
            this._unitManager.SpawnUnitsToMaximum();
        }

        public IUnit GetUnit(IBody body)
        {
            return this._unitsInWorld[body];
        }

        public IEnumerable<IUnit> GetUnitsAround(IBody observer, float radius, bool includeSelf)
        {
            foreach (IBody movable in this._environment.GetMovableAround(observer, radius, includeSelf))
            {
                yield return GetUnit(movable);
            }
        }

        private void SubscribeUnit(IUnit unit)
        {
            if (unit.Team == this._playerTeam)
                this.Player = unit;

            this._unitsInWorld.Add(unit.Body, unit);
            this._environment.AddBody(unit.Body);

            setUnit(unit);

            void setUnit(IUnit unit)
            {
                unit.OnUnitDeath += this.RemoveUnit;

                if (unit.Team == this._playerTeam)
                {
                    unit.Body.SetPosition(new Vector3(Random.Range(-2f, 2f), 0f, Random.Range(-2f, 2f)), Quaternion.identity);
                }
                else
                {
                    unit.Body.SetPosition(this._spawnShape.GetRandomPoint(this.Player.Body.Position), Quaternion.identity);
                }

                unit.SetActive(true);
            }
        }

        private void RemoveUnit(IUnit unit)
        {
            this._unitsInWorld.Remove(unit.Body);
            this._environment.RemoveBody(unit.Body);
            this._unitManager.ReturnUnit(unit);

            unit.OnUnitDeath -= this.RemoveUnit;
        }
    }
}
